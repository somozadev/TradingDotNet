using System.Threading.Channels;
using Api.Streaming;
using Application.Abstractions;
using Application.Backtesting;
using Domain.Models;
using Infrastructure.Ticks;

var builder = WebApplication.CreateBuilder(args);

//DI registrations
builder.Services.AddControllers();
builder.Services.AddScoped<IBacktestRunner, BacktestRunner>();
// builder.Services.AddSingleton<ITickSource, FakeTickSource>();


//Channel bounded = backpressure (reduce RAM)
var channel = Channel.CreateBounded<Tick>(new BoundedChannelOptions(50_000)
{
    SingleWriter = true,
    SingleReader = false,
    FullMode = BoundedChannelFullMode.Wait
});

builder.Services.AddSingleton(channel);
builder.Services.AddSingleton(channel.Reader);
builder.Services.AddSingleton(channel.Writer);

builder.Services.AddHostedService<MarketDataGenerator>();
builder.Services.AddSingleton<ITickSource>(sp =>
{
    var reader = sp.GetRequiredService<ChannelReader<Tick>>();
    return new ChannelTickSource(reader); 
});

//
// var datasetRoot = builder.Configuration.GetValue<string>("DatasetRoot") ??
//                   Path.Combine(builder.Environment.ContentRootPath, "datasets");
// builder.Services.AddSingleton<ITickSource>(sp =>
// {
//     var logger = sp.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CsvTickSource>>();
//     return new CsvTickSource(logger, datasetRoot); 
// });



var app = builder.Build();

app.MapControllers();
app.Run();


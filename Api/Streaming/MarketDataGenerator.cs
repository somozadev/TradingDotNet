using System.Threading.Channels;
using Domain.Models;

namespace Api.Streaming;

public sealed class MarketDataGenerator : BackgroundService
{
    private readonly ChannelWriter<Tick> _writer;
    private readonly ILogger<MarketDataGenerator> _logger;

    public MarketDataGenerator(ChannelWriter<Tick> writer, ILogger<MarketDataGenerator> logger)
    {
        _writer = writer;
        _logger = logger; 
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("MarketDataGenerator started");
        var symbol = "TEST";
        var rnd = new Random(123);
        long ts = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1_000_000;

        while (!stoppingToken.IsCancellationRequested)
        {
            var price = 100.0 + rnd.NextDouble();
            var tick = new Tick(ts++, symbol, price, 1.0);

            // Backpressure: if the consumer is slow this will wait 
            await _writer.WriteAsync(tick, stoppingToken);

            await Task.Delay(1, stoppingToken); // 1l ticks/sg aprox
        }
    }
}
using Application.Abstractions;
using Application.Backtesting;
using Domain.Models;
using Microsoft.Extensions.Logging.Abstractions;

namespace Tests;

public class BacktestRunnerTests
{
    private sealed class EmptyTickSource : ITickSource
    {
        public async IAsyncEnumerable<Tick> ReadTicksAsync(string datasetId, CancellationToken ct)
        {
            await Task.CompletedTask;
            yield break;
        }
    }

    [Fact]
    public async Task RunAsync_ReturnsId()
    {
        var runner = new BacktestRunner(new EmptyTickSource(), NullLogger<BacktestRunner>.Instance);
        var id = await runner.RunAsync("x", CancellationToken.None); 
        
        Assert.False(string.IsNullOrWhiteSpace(id));
    }
}
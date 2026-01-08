using Application.Abstractions;
using Domain.Models;

namespace Infrastructure.Ticks;

public sealed class FakeTickSource : ITickSource
{
    public async IAsyncEnumerable<Tick> ReadTicksAsync(string datasetId, CancellationToken ct)
    {
        var symbol = "TEST";
        var start = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() * 1_000_000; //ns aprox

        for (int i = 0; i < 100_000; i++)
        {
            ct.ThrowIfCancellationRequested();
            yield return new Tick(start + i, symbol, 100.0 + (i % 100) * 0.01, 1.0);
            if (i % 10_000 == 0) await Task.Yield();
        }
        
    }
}
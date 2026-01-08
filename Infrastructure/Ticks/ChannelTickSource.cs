using System.Threading.Channels;
using Application.Abstractions;
using Domain.Models;

namespace Infrastructure.Ticks;

public sealed class ChannelTickSource : ITickSource
{
    private readonly ChannelReader<Tick> _reader;

    public ChannelTickSource(ChannelReader<Tick> reader) => _reader = reader;
    
    public async IAsyncEnumerable<Tick> ReadTicksAsync(string datasetId, [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        while (await _reader.WaitToReadAsync(ct))
            while (_reader.TryRead(out var tick))
                yield return tick;
    }
}
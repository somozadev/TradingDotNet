using Domain.Models;

namespace Application.Abstractions;

public interface ITickSource
{
    IAsyncEnumerable<Tick> ReadTicksAsync(string datasetId, CancellationToken ct); 
}
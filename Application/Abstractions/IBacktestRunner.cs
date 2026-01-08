namespace Application.Abstractions;

public interface IBacktestRunner
{
    Task<string> RunAsync(string datasetId, CancellationToken ct); 
}
using Application.Abstractions;
using Microsoft.Extensions.Logging;

namespace Application.Backtesting;

public class BacktestRunner : IBacktestRunner
{
    private readonly ITickSource _tickSource;
    private readonly ILogger<BacktestRunner> _logger;

    public BacktestRunner(ITickSource tickSource, ILogger<BacktestRunner> logger)
    {
        _tickSource = tickSource;
        _logger = logger; 
    }
    
    
    public async Task<string> RunAsync(string datasetId, CancellationToken ct)
    {
        _logger.LogInformation("Starting backtest {DatasetId}", datasetId);

        long count = 0;
        await foreach (var _ in _tickSource.ReadTicksAsync(datasetId, ct))
            count++; 
        
        _logger.LogInformation("Finished backtest {DatasetId}. Processed {Count} ticks", datasetId, count);
        return Guid.NewGuid().ToString("N");
    }
}
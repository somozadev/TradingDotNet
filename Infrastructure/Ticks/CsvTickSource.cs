using System.Globalization;
using System.Reflection;
using Application.Abstractions;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Ticks;

public class CsvTickSource : ITickSource
{
    private readonly ILogger<CsvTickSource> _logger;
    private readonly string _datasetsRoot;

    public CsvTickSource(ILogger<CsvTickSource> logger, string datasetsRoot)
    {
        _logger = logger;
        _datasetsRoot = datasetsRoot;
    }

    public async IAsyncEnumerable<Tick> ReadTicksAsync(string datasetId,[System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct)
    {
        //datasetId -> path (simple)
        var path = Path.Combine(_datasetsRoot, $"{datasetId}.csv");
        if (!File.Exists(path))
            throw new FileNotFoundException($"Dataset not found: {path}");
        
        _logger.LogInformation("Reading CSV dataset {DatasetId} from {Path}", datasetId, path);
        
        using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize: 1 << 16, useAsync: true);
        using var sr = new StreamReader(fs); 
        
        //header 
        var header = await sr.ReadLineAsync(ct); 
        if(header is null) yield break;

        string? line;
        long lineNo = 1;

        while ((line = await sr.ReadLineAsync(ct)) is not null)
        {
            lineNo++;
            ct.ThrowIfCancellationRequested();
            if(string.IsNullOrWhiteSpace(line))continue;
            
            //native split (to be optimized with span)
            var parts = line.Split(',');
            if(parts.Length < 0) continue;
            
            if(!long.TryParse(parts[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out var ts))
                throw new FormatException($"Invalid timestamp at line {lineNo}");
            var symbol = parts[1];
            if (!double.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out var price))
                throw new FormatException($"Invalid price at line {lineNo}");
            if(!double.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out var vol))
                throw new FormatException($"Invalid volume at line {lineNo}");
            
            yield return new Tick(ts, symbol, price, vol); 
        }
    }
}
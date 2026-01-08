using Application.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("backtests")]
public class BacktestController : ControllerBase
{
    private readonly IBacktestRunner _runner;

    public BacktestController(IBacktestRunner runner) => _runner = runner;

    [HttpPost("run")]
    public async Task<IActionResult> Run([FromQuery] string datasetId = "demo", CancellationToken ct = default)
    {
        var id = await _runner.RunAsync(datasetId, ct);
        return Ok(new { backtestId = id });
    }
}
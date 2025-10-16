using Microsoft.AspNetCore.Mvc;

namespace AiStatusTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class LogTestController : ControllerBase
{
    private readonly ILogger<LogTestController> _logger;

    public LogTestController(ILogger<LogTestController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public string Get()
    {
        _logger.LogTrace("Trace Log");
        _logger.LogDebug("Debug Log");
        _logger.LogInformation("Information Log");
        _logger.LogWarning("Warning Log");
        _logger.LogError("Error Log");
        _logger.LogCritical("Critical Log");

        return "Check logs in console!";
    }
    
}
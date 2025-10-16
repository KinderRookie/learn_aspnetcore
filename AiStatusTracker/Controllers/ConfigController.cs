using Microsoft.AspNetCore.Mvc;

namespace AiStatusTracker.Controllers;


[ApiController]
[Route("[controller]")]
public class ConfigController : ControllerBase
{
    private readonly IConfiguration _config;

    public ConfigController(IConfiguration config)
    {
        _config = config;
    }

    [HttpGet]
    public string Get()
    {
        var message = _config["AppSettings:WelcomeMessage"];
        return message ?? "Default Message";
    }
}
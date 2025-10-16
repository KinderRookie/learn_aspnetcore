using Microsoft.AspNetCore.Mvc;

namespace AiStatusTracker.Controllers;

[ApiController]
[Route("[controller]")]
public class HelloController : ControllerBase
{
   [HttpGet]
   public string Get() => "Hello ASP.NET Core!";
}
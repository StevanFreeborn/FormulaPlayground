using Microsoft.AspNetCore.Mvc;
using Onspring.API.SDK;

namespace server.Controllers;

[ApiController]
[Route("api/apps")]
public class AppsController : ControllerBase
{
    private readonly ILogger<AppsController> _logger;

    public AppsController(ILogger<AppsController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetApps")]
    public async Task<IActionResult> GetAllAccessibleApps()
    {
        var baseUrl = "https://api.onspring.com";
        var apiKey = this.Request.Headers["x-apikey"];
        var onspringClient = new OnspringClient(baseUrl, apiKey);
        var apps = await onspringClient.GetAppsAsync();

        return Ok(apps.Value.Items);
    }
}

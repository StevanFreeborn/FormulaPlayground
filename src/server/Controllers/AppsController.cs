using Microsoft.AspNetCore.Mvc;
using Onspring.API.SDK;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/apps")]
public class AppsController : ControllerBase
{
  private readonly ILogger<AppsController> _logger;
  private readonly IOnspringService _onspringService;

  public AppsController(ILogger<AppsController> logger, IOnspringService onspringService)
  {
    _logger = logger;
    _onspringService = onspringService;
  }

  [HttpGet(Name = "GetApps")]
  public async Task<IActionResult> GetApps([FromHeader(Name = "x-apikey")] string apiKey)
  {
    if (String.IsNullOrWhiteSpace(apiKey)) {
      return BadRequest(new { error = "Enter a valid api key" });
    }

    try
    {
      var apps = await _onspringService.GetApps(apiKey);
      return Ok(apps);
    }
    catch (Exception e)
    {
      return StatusCode(500, new { error = e.Message });
    }
  }
}

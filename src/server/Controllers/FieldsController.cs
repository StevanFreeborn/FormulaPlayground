using Microsoft.AspNetCore.Mvc;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/fields")]
public class FieldsController : ControllerBase
{
  private readonly ILogger<FieldsController> _logger;
  private readonly IOnspringService _onspringService;

  public FieldsController(ILogger<FieldsController> logger, IOnspringService onspringService)
  {
    _logger = logger;
    _onspringService = onspringService;
  }

  [HttpPost(Name = "GetFields")]
  public async Task<IActionResult> GetFields([FromHeader(Name = "x-apikey")] string apiKey, int appId)
  {
    Console.WriteLine(appId);
    if (String.IsNullOrWhiteSpace(apiKey)) {
      return BadRequest(new { error = "Enter a valid api key" });
    }

    if (appId <= 0) {
      return BadRequest( new { error = "Invalid appId" });
    }

    try
    {
      var apps = await _onspringService.GetFields(apiKey, appId);
      return Ok(apps);
    }
    catch (Exception e)
    {
      return StatusCode(500, new { error = e.Message });
    }
  }
}

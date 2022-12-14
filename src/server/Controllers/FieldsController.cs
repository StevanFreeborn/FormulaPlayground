using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Models;

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

  [HttpPost]
  public async Task<IActionResult> GetFields([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] GetFieldsRequest request)
  {
    if (String.IsNullOrWhiteSpace(apiKey)) {
      return BadRequest(new { error = "Enter a valid api key" });
    }

    if (request.AppId <= 0) {
      return BadRequest( new { error = "Invalid appId" });
    }

    try
    {
      var fields = await _onspringService.GetFields(apiKey, request.AppId);
      return Ok(fields);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return StatusCode(500, new { error = "Failed to retrieve fields." });
    }
  }
}

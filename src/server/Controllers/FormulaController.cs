using System.Text.Json;
using Jint;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Services;

namespace server.Controllers;

// TODO: Implement running formula
// TODO: Implement validating formula
// TODO: Extract formula engine to a service or separate file
[ApiController]
[Route("api/formulas")]
public class FormulaController : ControllerBase
{
  private readonly ILogger<AppsController> _logger;
  private readonly IOnspringService _onspringService;

  public FormulaController(ILogger<AppsController> logger, IOnspringService onspringService)
  {
    _logger = logger;
    _onspringService = onspringService;
  }

  [HttpPost("validate")]
  public IActionResult ValidateFormula([FromBody] ValidateFormulaRequest request)
  {
    // if (String.IsNullOrWhiteSpace(apiKey))
    // {
    //   return BadRequest(new { error = "Enter a valid api key" });
    // }

    try
    {
      var engine = new Engine(new Options
      {
        TimeZone = TimeZoneInfo.Utc,
      });

      engine.Execute(request.Formula);
      return Ok(new { result = "The formula syntax is valid" });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return StatusCode(500, new { error = "The formula syntax is invalid.", message = e.Message });
    }
  }

  [HttpPost("run")]
  public IActionResult RunFormula([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] RunFormulaRequest request)
  {
    // if (String.IsNullOrWhiteSpace(apiKey))
    // {
    //   return BadRequest(new { error = "Enter a valid api key" });
    // }

    try
    {
      var engine = new Engine(new Options
      {
        TimeZone = TimeZoneInfo.Utc,
      });
      var result = engine.Evaluate(request.Formula);
      var response = JsonSerializer.Serialize(result.ToObject());
      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return StatusCode(500, new { error = "Failed to run formula." });
    }
  }
}

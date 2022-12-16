using Esprima;
using Jint;
using Jint.Runtime;
using Microsoft.AspNetCore.Mvc;
using server.Dtos;
using server.Services;
using Jint.Native.Json;
using Jint.Native;
using Jint.Native.Error;

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
    try
    {
      var engine = new Engine(new Options
      {
        TimeZone = TimeZoneInfo.Utc,
      });

      engine.Execute(request.Formula);
      return Ok(new { result = "The formula syntax is valid" });
    }
    catch (JavaScriptException je)
    {
      return BadRequest(new { result = "The formula syntax is invalid.", message = je.ToString() });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return StatusCode(500, new { error = "We're sorry we were unable to validate the formula.", message = e.Message });
    }
  }

  [HttpPost("run")]
  public IActionResult RunFormula([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] RunFormulaRequest request)
  {
    try
    {
      var engine = new Engine(cfg => cfg.AllowClr().LocalTimeZone(TimeZoneInfo.Utc));
      var parserOptions = new ParserOptions
      {
        Tolerant = true,
        AdaptRegexp = true,
      };

      var result = engine.Evaluate(request.Formula, parserOptions);

      return Ok(new { result = result is JsError ? result.ToString() : result.ToObject() });
    }
    catch(Exception e) when (e is JavaScriptException || e is ParserException)
    {
      return Ok(new { result = e.Message });
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      return StatusCode(500, new { error = "Failed to run formula." });
    }
  }
}
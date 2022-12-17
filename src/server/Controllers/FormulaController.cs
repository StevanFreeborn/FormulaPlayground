using Microsoft.AspNetCore.Mvc;
using System.Net;
using server.Dtos;
using server.Services;

namespace server.Controllers;

[ApiController]
[Route("api/formulas")]
public class FormulaController : ControllerBase
{
  private readonly ILogger<AppsController> _logger;
  private readonly IOnspringService _onspringService;
  private readonly IFormulaService _formulaService;

  public FormulaController(ILogger<AppsController> logger, IOnspringService onspringService, IFormulaService formulaService)
  {
    _logger = logger;
    _onspringService = onspringService;
    _formulaService = formulaService;
  }

  [HttpPost("validate")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(RunFormulaResult), StatusCodes.Status400BadRequest)]
  public ActionResult<ValidateFormulaResult> ValidateFormula([FromBody] ValidateFormulaRequest request)
  {
    var response = new ValidateFormulaResult();
    // TODO: Need to parse the formula to account for onspring specific tokens and validation issues.
    try
    {
      var validationResult = _formulaService.ValidateFormula(request.Formula);
      
      if (validationResult.IsValid is false) 
      {
        response.Message = validationResult.Exception.Message;
        return BadRequest(response);
      }

      response.Message = "The formula syntax is valid";
      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      response.Message = "We're sorry we were unable to validate the formula.";
      return StatusCode(500, response);
    }
  }

  [HttpPost("run")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(RunFormulaResult), StatusCodes.Status400BadRequest)]
  public ActionResult<RunFormulaResult> RunFormula([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] RunFormulaRequest request)
  {
    var response = new RunFormulaResult();
    // TODO: Need to parse the formula to account for onspring specific tokens and validation issues.
    try
    {
      var runResult = _formulaService.RunFormula(request.Formula);
      response.Result = runResult.Value;

      if (runResult.IsValid is false)
      {
        response.Error = runResult.Exception.Message;
        return BadRequest(response);
      }

      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      response.Error = "Failed to run formula.";
      return StatusCode(500, response);
    }
  }

  private static string ObjectToString(object obj)
  {
    switch (obj)
    {
      case null:
        return null;
      case string s:
        return s;
      case DateTime time:
        var dt = DateTime.SpecifyKind(time, DateTimeKind.Unspecified);
        return dt.ToLongDateString();
      case object[] objArray:
        return string.Join(", ", objArray.Where(a => a != null).Select(ObjectToString));
    }

    return obj.ToString();
  }
}


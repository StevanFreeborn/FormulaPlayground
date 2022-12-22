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
  public async Task<ActionResult<ValidateFormulaResult>> ValidateFormula([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] FormulaRequest request)
  {
    var response = new ValidateFormulaResult();
    // TODO: Need to parse the formula to account for onspring specific tokens and validation issues.
    try
    {
      var formulaContext = await _onspringService.GetFormulaContext(apiKey, request.Timezone, request.AppId, request.RecordId);
      var validationResult = _formulaService.ValidateFormula(request.Formula, formulaContext);
      
      if (validationResult.IsValid is false) 
      {
        var messages = validationResult.Exceptions.Select(e => e.Message).ToList();
        response.Messages.AddRange(messages);
        return BadRequest(response);
      }

      response.Messages.Add("The formula syntax is valid");
      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      response.Messages.Add("We're sorry we were unable to validate the formula.");
      return StatusCode(500, response);
    }
  }

  [HttpPost("run")]
  [ProducesResponseType(StatusCodes.Status200OK)]
  [ProducesResponseType(typeof(RunFormulaResult), StatusCodes.Status400BadRequest)]
  public async Task<ActionResult<RunFormulaResult>> RunFormula([FromHeader(Name = "x-apikey")] string apiKey, [FromBody] FormulaRequest request)
  {
    var response = new RunFormulaResult();
    // TODO: Need to parse the formula to account for onspring specific tokens and validation issues.
    try
    {
      var formulaContext = await _onspringService.GetFormulaContext(apiKey, request.Timezone, request.AppId, request.RecordId);
      var runResult = _formulaService.RunFormula(request.Formula, formulaContext);
      response.Result = runResult.Value;

      if (runResult.IsValid is false)
      {
        var errors = runResult.Exceptions.Select(e => e.Message).ToList();
        response.Errors.AddRange(errors);
        return BadRequest(response);
      }

      return Ok(response);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      response.Errors.Add("Failed to run formula.");
      return StatusCode(500, response);
    }
  }
}


using server.Models;
using Jint;
using Jint.Runtime;
using Jint.Native.Json;
using Esprima;

namespace server.Services;

public class JintFormulaService : IFormulaService
{
  private readonly Engine _engine;
  private readonly ParserOptions _parserOptions;

  private readonly JsonSerializer _serializer;

  public JintFormulaService()
  {
    _engine = new Engine(cfg => cfg.AllowClr().LocalTimeZone(TimeZoneInfo.Utc));
    _parserOptions = new ParserOptions
    {
      Tolerant = true,
      AdaptRegexp = true,
    };
    _serializer = new JsonSerializer(_engine);
  }

  public FormulaRunResult RunFormula(string formula)
  {
    var result = new FormulaRunResult();

    try
    {
      var engineResult = _engine.Evaluate(formula, _parserOptions);
      var serializedResult = _serializer.Serialize(engineResult);
      result.Value = serializedResult.ToObject();
    }
    catch(Exception e) when (e is JavaScriptException || e is ParserException)
    {
      result.Exception = e;
    }

    return result;
  }

  public FormulaValidationResult ValidateFormula(string formula)
  {
    var result = new FormulaValidationResult();
    try
    {
      _engine.Execute(formula);
    }
    catch (Exception e) when (e is JavaScriptException || e is ParserException)
    {
      result.Exception = e;
    }

    return result;
  }
}
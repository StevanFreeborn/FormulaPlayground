using server.Models;
using Jint;
using Jint.Runtime;
using Esprima;

namespace server.Services;

public class JintFormulaService : IFormulaService
{
  private readonly Engine _engine;
  private readonly ParserOptions _parserOptions;

  public JintFormulaService()
  {
    _engine = new Engine(cfg => cfg.AllowClr().LocalTimeZone(TimeZoneInfo.Utc));
    _parserOptions = new ParserOptions
    {
      Tolerant = true,
      AdaptRegexp = true,
    };
  }

  public FormulaRunResult RunFormula(string formula)
  {
    var result = new FormulaRunResult();

    try
    {
      result.Value = _engine.Evaluate(formula, _parserOptions).ToObject();
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
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

  public FormulaRunResult RunFormula(string formula, FormulaContext formulaContext)
  {
    var result = new FormulaRunResult();

    try
    {
      var parsedFormula = FormulaParser.ParseFormula(formula, formulaContext);
      SetFieldVariableValues(_engine, formulaContext.FieldVariableToValueMap);
      var engineResult = _engine.Evaluate(parsedFormula, _parserOptions).ToObject();
      result.Value = FormulaProcessor.GetResultAsString(engineResult, formulaContext.InstanceTimezone);
    }
    catch(Exception e) when (e is JavaScriptException || e is ParserException)
    {
      result.Exception = e;
    }

    return result;
  }

  private void SetFieldVariableValues(Engine engine, Dictionary<string, object> fieldVariableToValueMap)
  {
    foreach(KeyValuePair<string, object> entry in fieldVariableToValueMap)
    {
      engine.SetValue(entry.Key, entry.Value);
    }
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
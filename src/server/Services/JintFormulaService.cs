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
      var parsedFormula = GetParsedFormula(formula, formulaContext);
      var engineResult = _engine.Evaluate(parsedFormula, _parserOptions).ToObject();
      result.Value = FormulaProcessor.GetResultAsString(engineResult, formulaContext.InstanceTimezone);
    }
    catch (Exception e) when (e is JavaScriptException || e is ParserException || e is AggregateException)
    {
      HandleFormulaException(e, result);
    }
    return result;
  }

  public FormulaValidationResult ValidateFormula(string formula, FormulaContext formulaContext)
  {
    var result = new FormulaValidationResult();
    try
    {
      var parsedFormula = GetParsedFormula(formula, formulaContext);
      _engine.Execute(parsedFormula);
    }
    catch (Exception e) when (e is JavaScriptException || e is ParserException || e is AggregateException)
    {
      HandleFormulaException(e, result);
    }
    return result;
  }

  private void HandleFormulaException(Exception e, FormulaResultBase result)
  {
    if (e is AggregateException)
    {
      var aggregateException = e as AggregateException;
      result.Exceptions.AddRange(aggregateException.InnerExceptions);
    }
    else
    {
      result.Exceptions.Add(e);
    }
  }

  private string GetParsedFormula(string formula, FormulaContext formulaContext)
  {
    var parsedFormula = FormulaParser.ParseFormula(formula, formulaContext);
    SetFieldVariableValues(_engine, formulaContext.FieldVariableToValueMap);
    return parsedFormula;
  }

  private void SetFieldVariableValues(Engine engine, Dictionary<string, object> fieldVariableToValueMap)
  {
    foreach (KeyValuePair<string, object> entry in fieldVariableToValueMap)
    {
      engine.SetValue(entry.Key, entry.Value);
    }
  }
}
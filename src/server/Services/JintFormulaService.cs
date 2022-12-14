using server.Models;
using Jint;
using Jint.Runtime;
using Esprima;
using System.Reflection;
using server.Models.Functions;

namespace server.Services;

public class JintFormulaService : IFormulaService
{
  private readonly Engine _engine;
  private readonly ParserOptions _parserOptions;

  public JintFormulaService()
  {
    var modulesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Models", "Functions", "Scripts");
    _engine = new Engine(cfg => 
    cfg
    .AllowClr()
    .LocalTimeZone(TimeZoneInfo.Utc)
    .EnableModules(modulesPath));
    _engine.ImportModule("./Date.js");
    SetFunctions();
    _parserOptions = new ParserOptions
    {
      Tolerant = true,
      AdaptRegexp = true,
    };
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

  private string GetParsedFormula(string formula, FormulaContext formulaContext)
  {
    var fieldTokens = FormulaParser.GetFieldTokens(formula);
    var listTokens = FormulaParser.GetListTokens(formula);
    FormulaParser.ValidateTokens(fieldTokens, listTokens, formulaContext);
    var parsedFormula = FormulaParser.ReplaceTokensWithValidVariableNames(formula, fieldTokens, listTokens);
    var tokenVariableToValueMap = FormulaParser.GetVariableToValueMap(fieldTokens, listTokens, formulaContext);
    SetTokenVariableValues(tokenVariableToValueMap);
    return parsedFormula;
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

  private void SetTokenVariableValues(Dictionary<string, object> tokenVariableToValueMap)
  {
    foreach (KeyValuePair<string, object> entry in tokenVariableToValueMap)
    {
      _engine.SetValue(entry.Key, entry.Value);
    }
  }

  private void SetFunctions()
  {
    var functionType = typeof(FunctionBase);
    var functions = Assembly
    .GetAssembly(functionType)
    .GetTypes()
    .Where(type => type.IsSubclassOf(functionType))
    .ToList();

    foreach(var function in functions)
    {
      var instance = Activator.CreateInstance(function) as FunctionBase;
      var nameFunctionPair = instance.GetNameFunctionPair();
      _engine.SetValue(nameFunctionPair.Key, nameFunctionPair.Value);
    }
  }
}
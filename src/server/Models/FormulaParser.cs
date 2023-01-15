using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using server.Extensions;

namespace server.Models;

public class FormulaParser
{
  private static readonly string fieldTokenStart = "{:";
  private static readonly string fieldTokenEnd = "}";
  private static readonly Regex fieldTokenRegex = new Regex(@"(?<!""|')\{:(.+?)\}(?!""|')");
  private static readonly string listTokenStart = "[:";
  private static readonly string listTokenEnd = "]";
  private static readonly Regex listTokenRegex = new Regex(@"\[:(.+?)\]");
  private static readonly Regex invalidNameCharactersRegex = new Regex(@"(\s|\+|-|\*|/|=|>|<|>=|<=|&|\||%|!|\^|\(|\))");

  private static readonly Regex funcRegex = new Regex(@"(ListNum|DateAddSpan|DateSubtractSpan|GetNextFutureDateBySpan)\(.*\)");

  public static List<string> GetFunctionParameterFieldTokens(string formula, FormulaContext context)
  {
    var funcMatches = funcRegex.Matches(formula).Select(funcMatch => funcMatch.Value).ToList();
    var fieldTokens = new List<string>();

    foreach (var funcMatch in funcMatches)
    {
      var fieldTokenMatches = fieldTokenRegex.Matches(funcMatch).Select(fieldTokenMatch => fieldTokenMatch.Value).ToList();
      foreach (var fieldTokenMatch in fieldTokenMatches)
      {
        var fieldName = GetFieldNameFromFieldToken(fieldTokenMatch);
        var field = context.Fields.FirstOrDefault(field => field.Name == fieldName);
        if (field is not null && field.Type is FieldType.List or FieldType.TimeSpan)
        {
          fieldTokens.Add(fieldTokenMatch);
        }
      }
    }

    return fieldTokens.Distinct().ToList();
  }

  public static List<string> GetFieldTokens(string formula)
  {
    return fieldTokenRegex.Matches(formula).Select(fieldTokenMatch => fieldTokenMatch.Value).Distinct().ToList();
  }

  public static List<string> GetListTokens(string formula)
  {
    return listTokenRegex.Matches(formula).Select(listTokenMatch => listTokenMatch.Value).Distinct().ToList();
  }

  public static void ValidateTokens(List<string> functionFieldTokens, List<string> fieldTokens, List<string> listTokens, FormulaContext formulaContext)
  {
    var exceptions = new List<Exception>();
    var combinedFieldTokens = new List<string>();
    combinedFieldTokens.AddRange(functionFieldTokens);
    combinedFieldTokens.AddRange(fieldTokens);
    var uniqueFieldTokens = combinedFieldTokens.Distinct().ToList();
    try
    {
      ValidateFieldTokens(uniqueFieldTokens, formulaContext.Fields);
    }
    catch (Exception e) when (e is AggregateException)
    {
      var aggregateException = e as AggregateException;
      exceptions.AddRange(aggregateException.InnerExceptions);
    }
    try
    {
      ValidateListTokens(listTokens, uniqueFieldTokens, formulaContext.Fields);
    }
    catch (Exception e) when (e is AggregateException)
    {
      var aggregateException = e as AggregateException;
      exceptions.AddRange(aggregateException.InnerExceptions);
    }
    if (exceptions.Count > 0)
    {
      throw new AggregateException("formula parsing errors", exceptions);
    }
  }

  public static string ReplaceTokensWithValidVariableNames(string formula, List<string> functionFieldTokens, List<string> fieldTokens, List<string> listTokens)
  {
    foreach (var functionFieldToken in functionFieldTokens)
    {
      var validFieldParameterVariable = ConvertFunctionFieldTokenToValidVariableName(functionFieldToken);
      formula = formula.Replace(functionFieldToken, validFieldParameterVariable);
    }
    foreach (var fieldToken in fieldTokens)
    {
      var validFieldVariable = ConvertFieldTokenToValidVariableName(fieldToken);
      formula = formula.Replace(fieldToken, validFieldVariable);
    }
    foreach (var listToken in listTokens)
    {
      var validListVariable = ConvertListTokenToValidVariableName(listToken);
      formula = formula.Replace(listToken, validListVariable);
    }
    return formula;
  }

  public static Dictionary<string, object> GetVariableToValueMap(List<string> functionFieldTokens, List<string> fieldTokens, List<string> listTokens, FormulaContext context)
  {
    var dict = new Dictionary<string, object>();
    foreach (var functionFieldToken in functionFieldTokens)
    {
      var functionFieldVariable = ConvertFunctionFieldTokenToValidVariableName(functionFieldToken);
      var fieldName = GetFieldNameFromFieldToken(functionFieldToken);
      var field = context.Fields.First(f => f.Name == fieldName);
      var variableValue = field.Id;
      dict.Add(functionFieldVariable, variableValue);
    }
    foreach (var fieldToken in fieldTokens)
    {
      var fieldVariable = ConvertFieldTokenToValidVariableName(fieldToken);
      var fieldName = GetFieldNameFromFieldToken(fieldToken);
      var field = context.Fields.First(f => f.Name == fieldName);
      var variableValue = context.FieldValues.FirstOrDefault(fv => fv.FieldId == field.Id).GetValue();
      if (variableValue is Guid?)
      {
        var variableValueAsGuid = variableValue as Guid?;
        variableValue = GetListValueName(field, variableValueAsGuid);
      }
      if (variableValue is List<Guid>)
      {
        var variableValueAsList = variableValue as List<Guid>;
        variableValue = GetMultiSelectListAsString(field, variableValueAsList);
      }
      if (variableValue is TimeSpanData timeSpanData)
      {
        variableValue = timeSpanData.GetAsString();
      }

      dict.Add(fieldVariable, variableValue);
    }
    foreach (var listToken in listTokens)
    {
      var listVariable = ConvertListTokenToValidVariableName(listToken);
      var listName = GetListNameFromListToken(listToken);
      dict.Add(listVariable, listName);
    }

    return dict;
  }

  private static string GetMultiSelectListAsString(Field field, List<Guid> fieldValue)
  {
    var listNames = new List<string>();
    foreach (var value in fieldValue)
    {
      var listName = GetListValueName(field, value);
      listNames.Add(listName);
    }
    return String.Join(", ", listNames);
  }

  private static string GetListValueName(Field field, Guid? listValueId)
  {
    var listField = field as ListField;
    return listField.Values.FirstOrDefault(v => v.Id == listValueId).Name;
  }

  private static string ConvertFunctionFieldTokenToValidVariableName(string functionFieldToken)
  {
    var fieldName = GetFieldNameFromFieldToken(functionFieldToken);
    var validFieldName = invalidNameCharactersRegex.Replace(fieldName, "_");
    return "fn" + validFieldName + "_";
  }

  private static string ConvertFieldTokenToValidVariableName(string fieldToken)
  {
    var fieldName = GetFieldNameFromFieldToken(fieldToken);
    var validFieldName = invalidNameCharactersRegex.Replace(fieldName, "_");
    return "__" + validFieldName + "_";
  }

  private static string ConvertListTokenToValidVariableName(string listToken)
  {
    var listName = GetListNameFromListToken(listToken);
    var validListName = invalidNameCharactersRegex.Replace(listName, "$");
    return "$$" + validListName + "$";
  }

  private static void ValidateFieldTokens(List<string> fieldTokens, List<Field> fields)
  {
    var exceptions = new List<Exception>();
    foreach (var fieldToken in fieldTokens)
    {
      var fieldName = GetFieldNameFromFieldToken(fieldToken);
      var field = fields.FirstOrDefault(f => f.Name == fieldName);
      if (field is null)
      {
        var exception = new ParserException($"'{fieldName}' was not recongized as a valid field.");
        exceptions.Add(exception);
      }
    }
    if (exceptions.Count > 0)
    {
      throw new AggregateException("field token errors", exceptions);
    }
  }

  private static void ValidateListTokens(List<string> listTokens, List<string> fieldTokens, List<Field> fields)
  {
    var exceptions = new List<Exception>();
    var fieldNames = fieldTokens.Select(fieldToken => GetFieldNameFromFieldToken(fieldToken)).ToList();
    var listValues = fields
    .Where(f => f.Type == FieldType.List && fieldNames.Contains(f.Name))
    .Select(f => f as ListField)
    .SelectMany(f => f.Values)
    .ToList();
    foreach (var listToken in listTokens)
    {
      var listName = GetListNameFromListToken(listToken);
      var listValue = listValues.FirstOrDefault(lv => lv.Name == listName);
      if (listValue is null)
      {
        var exception = new ParserException($"List value {listToken} is not recongized as a valid list value.");
        exceptions.Add(exception);
      }
    }
    if (exceptions.Count > 0)
    {
      throw new AggregateException("list token errors", exceptions);
    }
  }

  private static string GetListNameFromListToken(string listToken)
  {
    var startOffset = listTokenStart.Length;
    var lengthOffset = listTokenStart.Length + listTokenEnd.Length;
    return listToken.Substring(startOffset, listToken.Length - lengthOffset);
  }

  private static string GetFieldNameFromFieldToken(string fieldToken)
  {
    var startOffset = fieldTokenStart.Length;
    var lengthOffset = fieldTokenStart.Length + fieldTokenEnd.Length;
    return fieldToken.Substring(startOffset, fieldToken.Length - lengthOffset);
  }
}
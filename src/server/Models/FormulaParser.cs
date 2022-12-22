using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;

namespace server.Models;

public class FormulaParser
{
  public static string ParseFormula(string formula, FormulaContext formulaContext)
  {
    var exceptions = new List<Exception>();
    var parsedFormula = formula;
    var fieldTokens = GetFieldTokens(formula);
    var listTokens = GetListTokens(formula);
    try
    {
      ValidateFieldTokens(fieldTokens, formulaContext.Fields);
    }
    catch (Exception e) when (e is AggregateException)
    {
      var aggregateException = e as AggregateException;
      exceptions.AddRange(aggregateException.InnerExceptions);
    }
    try
    {
      ValidateListTokens(listTokens, formulaContext.Fields);
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

    parsedFormula = ReplaceFieldTokensWithValidVariableNames(parsedFormula, fieldTokens);
    formulaContext.FieldVariableToValueMap = GetFieldVariableToValueMap(fieldTokens, formulaContext);

    return parsedFormula;
  }

  private static void ValidateTokens()
  {
    throw new NotImplementedException();
  }

  private static Dictionary<string, object> GetFieldVariableToValueMap(List<string> fieldTokens, FormulaContext context)
  {
    var dict = new Dictionary<string, object>();
    foreach (var fieldToken in fieldTokens)
    {
      var fieldVariable = ConvertFieldTokenToValidVariableName(fieldToken);
      var fieldName = GetFieldNameFromFieldToken(fieldToken);
      var field = GetFieldFromFieldsContext(fieldName, context);
      var variableValue = GetRecordFieldValueFromContext(field, context);
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
      dict.Add(fieldVariable, variableValue);
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

  private static string ReplaceFieldTokensWithValidVariableNames(string formula, List<string> fieldTokens)
  {
    foreach (var fieldToken in fieldTokens)
    {
      var validFieldName = ConvertFieldTokenToValidVariableName(fieldToken);
      formula = formula.Replace(fieldToken, validFieldName);
    }
    return formula;
  }

  private static object GetRecordFieldValueFromContext(Field field, FormulaContext context)
  {
    return context.FieldValues.FirstOrDefault(fv => fv.FieldId == field.Id).GetValue();
  }

  private static Field GetFieldFromFieldsContext(string fieldName, FormulaContext context)
  {
    var field = context.Fields.First(f => f.Name == fieldName);
    return field;
  }

  private static string ConvertFieldTokenToValidVariableName(string fieldToken)
  {
    var fieldName = GetFieldNameFromFieldToken(fieldToken);
    var invalidFieldNameCharactersRegex = new Regex(@"(\s|\+|-|\*|/|=|>|<|>=|<=|&|\||%|!|\^|\(|\))");
    var validFieldName = invalidFieldNameCharactersRegex.Replace(fieldName, "_");
    return "__" + validFieldName + "_";
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

  private static void ValidateListTokens(List<string> listTokens, List<Field> fields)
  {
    var exceptions = new List<Exception>();
    var listValues = fields
    .Where(f => f.Type == FieldType.List)
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
    return listToken.Substring(2, listToken.Length - 3);
  }

  private static string GetFieldNameFromFieldToken(string fieldToken)
  {
    return fieldToken.Substring(2, fieldToken.Length - 3);
  }

  private static List<string> GetListTokens(string formula)
  {
    var listTokenRegex = new Regex(@"\[:(.+?)\]");
    return listTokenRegex.Matches(formula).Select(listTokenMatch => listTokenMatch.Value).ToList();
  }

  private static List<string> GetFieldTokens(string formula)
  {
    var fieldTokenRegex = new Regex(@"\{:(.+?)\}");
    return fieldTokenRegex.Matches(formula).Select(fieldTokenMatch => fieldTokenMatch.Value).ToList();
  }
}
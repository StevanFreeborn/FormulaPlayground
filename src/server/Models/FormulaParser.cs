using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Onspring.API.SDK.Models;

namespace server.Models;

public class FormulaParser
{
  public static string ParseFormula(string formula, FormulaContext formulaContext)
  {
    var parsedFormula = formula;
    var fieldTokens = GetFieldTokens(formula);
    ValidateFieldTokens(fieldTokens, formulaContext.Fields);
    parsedFormula = ReplaceFieldTokensWithValidVariableNames(parsedFormula, fieldTokens);
    formulaContext.FieldVariableToValueMap = GetFieldVariableToValueMap(fieldTokens, formulaContext);
    return parsedFormula;
  }

  private static Dictionary<string, object> GetFieldVariableToValueMap(List<string> fieldTokens, FormulaContext context)
  {
    var dict = new Dictionary<string, object>();
    foreach(var fieldToken in fieldTokens)
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
    foreach(var value in fieldValue)
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
    foreach(var fieldToken in fieldTokens)
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

  private static string GetFieldNameFromFieldToken(string fieldToken)
  {
    return fieldToken.Substring(2, fieldToken.Length - 3);
  }

  private static List<string> GetFieldTokens(string formula)
  {
    var fieldTokenRegex = new Regex(@"\{:(.+?)\}");
    return fieldTokenRegex.Matches(formula).Select(fieldTokenMatch => fieldTokenMatch.Value).ToList();
  }
}
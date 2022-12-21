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
      var fieldId = GetFieldIdFromFieldsContext(fieldName, context);
      var variableValue = GetRecordFieldValueFromContext(fieldId, context).GetValue();
      dict.Add(fieldVariable, variableValue);
    }
    return dict;
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

  private static RecordFieldValue GetRecordFieldValueFromContext(int fieldId, FormulaContext context)
  {
    return context.FieldValues.FirstOrDefault(fv => fv.FieldId == fieldId);
  }

  private static int GetFieldIdFromFieldsContext(string fieldName, FormulaContext context)
  {
    var field = context.Fields.First(f => f.Name == fieldName);
    return field.Id;
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
    foreach (var fieldToken in fieldTokens)
    {
      var fieldName = GetFieldNameFromFieldToken(fieldToken);
      var field = fields.FirstOrDefault(f => f.Name == fieldName);
      
      if (field is null)
      {
        throw new ParserException($"'{fieldName}' was not recongized as a valid field.");
      }
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
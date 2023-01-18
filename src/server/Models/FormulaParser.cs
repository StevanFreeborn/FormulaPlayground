using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using server.Extensions;
using server.Services;

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

  // TODO: Look at how this class could be better organized or methods further refined
  public static List<string> GetFunctionParameterFieldTokens(string formula, FormulaContext context)
  {
    var funcMatches = funcRegex.Matches(formula).Select(funcMatch => funcMatch.Value).ToList();
    var fieldTokens = new List<string>();

    foreach (var funcMatch in funcMatches)
    {
      var fieldTokenMatches = fieldTokenRegex.Matches(funcMatch).Select(fieldTokenMatch => fieldTokenMatch.Value).ToList();
      foreach (var fieldTokenMatch in fieldTokenMatches)
      {
        fieldTokens.Add(fieldTokenMatch);
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

  public static async Task ValidateTokens(List<string> functionFieldTokens, List<string> fieldTokens, List<string> listTokens, FormulaContext formulaContext)
  {
    var exceptions = new List<Exception>();
    var combinedFieldTokens = new List<string>();
    combinedFieldTokens.AddRange(functionFieldTokens);
    combinedFieldTokens.AddRange(fieldTokens);
    var uniqueFieldTokens = combinedFieldTokens.Distinct().ToList();
    try
    {
      await ValidateFieldTokens(uniqueFieldTokens, formulaContext);
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

  public static string ReplaceTokensWithValidVariableNames(string formula, List<string> functionFieldTokens, List<string> fieldTokens, List<string> listTokens, FormulaContext context)
  {
    foreach (var functionFieldToken in functionFieldTokens)
    {
      var validFieldParameterVariable = ConvertFunctionFieldTokenToValidVariableName(functionFieldToken, context);
      formula = formula.Replace(functionFieldToken, validFieldParameterVariable);
    }
    foreach (var fieldToken in fieldTokens)
    {
      var validFieldVariable = ConvertFieldTokenToValidVariableName(fieldToken, context);
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
      var functionFieldVariable = ConvertFunctionFieldTokenToValidVariableName(functionFieldToken, context);
      var fieldName = GetFieldNameFromFieldToken(functionFieldToken);
      var field = GetFieldFromFieldName(fieldName, context);
      
      if (field.Type is FieldType.List or FieldType.TimeSpan)
      {
        var ids = GetFieldIdsAsString(fieldName, context);
        dict.Add(functionFieldVariable, ids);
        continue;
      }

      var variableValue = GetVariableValue(field, context);
      dict.Add(functionFieldVariable, variableValue);
    }
    foreach (var fieldToken in fieldTokens)
    {
      var fieldVariable = ConvertFieldTokenToValidVariableName(fieldToken, context);
      var fieldName = GetFieldNameFromFieldToken(fieldToken);
      var field = GetFieldFromFieldName(fieldName, context);
      var variableValue = GetVariableValue(field, context);
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

  private static object GetVariableValue(Field field, FormulaContext context)
  {
    var recordValues = context
      .FieldValues
      .Where(fv => fv.FieldId == field.Id).ToList();

    if (recordValues.Count is 0)
    {
      return null;
    }

    var variableValues = recordValues
    .Select(fv => GetRecordValuesVariableValue(fv, field))
    .ToArray();

    if (variableValues.Length is 1)
    {
      return variableValues[0];
    }

    return variableValues;
  }

  private static object GetRecordValuesVariableValue(RecordFieldValue recordFieldValue, Field field)
  {
    var variableValue = recordFieldValue.GetValue();
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
    if (variableValue is List<int> ints)
    {
      variableValue = ints.ToArray();
    }
    return variableValue;
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

  private static string ConvertFunctionFieldTokenToValidVariableName(string functionFieldToken, FormulaContext context)
  {
    var fieldName = GetFieldNameFromFieldToken(functionFieldToken);
    var field = GetFieldFromFieldName(fieldName, context);
    var validFieldName = invalidNameCharactersRegex.Replace(field.Name, "_");
    return "fn" + validFieldName + "_" + field.Id;
  }

  private static string ConvertFieldTokenToValidVariableName(string fieldToken, FormulaContext context)
  {
    var fieldName = GetFieldNameFromFieldToken(fieldToken);
    var field = GetFieldFromFieldName(fieldName, context);
    var validFieldName = invalidNameCharactersRegex.Replace(field.Name, "_");
    return "__" + validFieldName + "_" + field.Id;
  }

  private static string ConvertListTokenToValidVariableName(string listToken)
  {
    var listName = GetListNameFromListToken(listToken);
    var validListName = invalidNameCharactersRegex.Replace(listName, "$");
    return "$$" + validListName + "$";
  }

  private static async Task ValidateFieldTokens(List<string> fieldTokens, FormulaContext context)
  {
    var exceptions = new List<Exception>();
    
    foreach (var fieldToken in fieldTokens)
    {
      var fieldName = GetFieldNameFromFieldToken(fieldToken);

      if (IsReferenceChain(fieldName, out List<string> referenceChain) is true)
      {
        var referenceChainException = await ValidateReferenceChain(referenceChain, context);
        if (referenceChainException is not null)
        {
          exceptions.Add(referenceChainException);
        }
        continue;
      }

      var field = context.Fields.FirstOrDefault(field => field.Name == fieldName);

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

  private static async Task<Exception> ValidateReferenceChain(List<string> referenceChain, FormulaContext context)
  {
    var referenceFieldName = referenceChain[0];
    var referencedFieldName = referenceChain[1];
    var referenceField = context.Fields.FirstOrDefault(field => field.Name == referenceFieldName) as ReferenceField;

    if (referenceField is null || referenceField.Type is not FieldType.Reference)
    {
      return new ParserException($"'{referenceFieldName}' was not recognized as a valid field in the '{String.Join("::", referenceChain)}' field reference.");
    }

    var onspringService = new OnspringService();
    var referenceFieldValue = context.FieldValues.FirstOrDefault(fieldValue => fieldValue.FieldId == referenceField.Id).GetValue();
    var referenceFieldRecordIds = new List<int>();
    
    if (referenceFieldValue is int?)
    {
      var recordId = referenceFieldValue as int?;
      if (recordId.HasValue)
      {
        referenceFieldRecordIds.Add(recordId.Value);
      }
    }
    if (referenceFieldValue is List<int> recordIds)
    {
      referenceFieldRecordIds.AddRange(recordIds);
    }

    var referencedAppId = referenceField.ReferencedAppId;
    var referencedAppFields = await onspringService.GetFields(context.ApiKey, referencedAppId);
    var referencedField = referencedAppFields.FirstOrDefault(field => field.Name == referencedFieldName);

    if (referencedField is null)
    {
      return new ParserException($"'{referencedFieldName}' was not recognized as a valid field in the '{String.Join("::", referenceChain)}' field reference.");
    }

    context.Fields.Add(referencedField);

    foreach(var recordId in referenceFieldRecordIds)
    {
      var recordValue = await onspringService.GetRecordFieldValue(context.ApiKey, referencedAppId, recordId, referencedField.Id);
      if (recordValue is not null)
      {
        context.FieldValues.Add(recordValue);
      }
    }

    var newChain = referenceChain.Skip(1).ToList();

    if (referenceField.Type is FieldType.Reference && newChain.Count > 1)
    {
      context.Fields.Add(referencedField);
      await ValidateReferenceChain(newChain, context);
    }

    return null;
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

  private static Field GetFieldFromFieldName(string fieldName, FormulaContext context)
  {
    if (IsReferenceChain(fieldName, out List<string> referenceChain) is false)
    {
      var name = referenceChain[0];
      return context.Fields.FirstOrDefault(field => field.Name == name);
    }

    return GetLastFieldInReferenceChain(referenceChain, context, context.PrimaryAppId);
  }

  private static string GetFieldIdsAsString(string fieldName, FormulaContext context)
  {
    if (IsReferenceChain(fieldName, out List<string> referenceChain) is false)
    {
      var name = referenceChain[0];
      return context.Fields.FirstOrDefault(field => field.Name == name).Id.ToString();
    }

    return GetReferenceChainAsStringOfIds(referenceChain, context, context.PrimaryAppId, new List<int>());
  }

  private static string GetReferenceChainAsStringOfIds(List<string> referenceChain, FormulaContext context, int currentApp, List<int> ids)
  {
    var fieldIds = new List<int>();
    fieldIds.AddRange(ids);
    
    var referenceFieldName = referenceChain[0];
    var referencedFieldName = referenceChain[1];
    var referenceField = context.Fields.FirstOrDefault(field => field.Name == referenceFieldName && field.AppId == currentApp) as ReferenceField;
    var referencedField = context.Fields.FirstOrDefault(field => field.Name == referencedFieldName && field.AppId == referenceField.ReferencedAppId);
    
    if (referenceField is not null)
    {
      fieldIds.Add(referenceField.Id);
    }

    if (referencedField is not null)
    {
      fieldIds.Add(referencedField.Id);
    }

    var newChain = referenceChain.Skip(1).ToList();
    
    if (newChain.Count > 1)
    {
      return GetReferenceChainAsStringOfIds(newChain, context, referenceField.ReferencedAppId, fieldIds);
    }

    return String.Join(",", fieldIds);
  }

  private static Field GetLastFieldInReferenceChain(List<string> referenceChain, FormulaContext context, int currentApp)
  {
    var referenceFieldName = referenceChain[0];
    var referencedFieldName = referenceChain[1];
    var referenceField = context.Fields.FirstOrDefault(field => field.Name == referenceFieldName && field.AppId == currentApp) as ReferenceField;
    var referencedField = context.Fields.FirstOrDefault(field => field.Name == referencedFieldName && field.AppId == referenceField.ReferencedAppId);
    var newChain = referenceChain.Skip(1).ToList();
    
    if (newChain.Count > 1)
    {
      return GetLastFieldInReferenceChain(newChain, context, referenceField.ReferencedAppId);
    }

    return referencedField;
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

  private static bool IsReferenceChain(string fieldName, out List<string> referenceChain)
  {
    referenceChain = fieldName.Split("::").ToList();
    if (referenceChain.Count > 1)
    {
      return true;
    }
    return false;
  }
}
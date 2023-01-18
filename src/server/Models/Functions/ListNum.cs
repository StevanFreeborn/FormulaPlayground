using Esprima;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using server.Models;
using server.Models.Functions;

public class ListNum : FunctionBase
{
  public ListNum(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "ListNum";

  protected override object Function(params object[] arguments)
  {
    var fieldIds = ArgumentHelper.GetArgByIndex(arguments, 0);
    var isString = ArgumentHelper.TryParseToType(fieldIds, out string fieldIdsAsString);
    var fieldIdsAsInt = fieldIdsAsString
    .Split(",")
    .Where(id => int.TryParse(id, out var result) is true)
    .Select(id => int.Parse(id)).ToList();

    var multiRefFields = Context
    .Fields
    .Where(field => fieldIdsAsInt.Contains(field.Id) && field.Type is FieldType.Reference)
    .Select(field => field as ReferenceField)
    .Where(field => field.Multiplicity is Multiplicity.MultiSelect)
    .ToList();

    var listFieldIdAsInt = fieldIdsAsInt.Count > 0 ? fieldIdsAsInt.Last() : 0;
    var listField = Context
    .Fields
    .FirstOrDefault(
      field => field.Id == listFieldIdAsInt &&
      field.Type == Onspring.API.SDK.Enums.FieldType.List
    ) as ListField;

    if (isString is false || listField is null || multiRefFields.Count > 0)
    {
      throw new ParserException($"{fieldIdsAsString} is not a valid list reference");
    }

    var recordFieldValue = Context
    .FieldValues
    .FirstOrDefault(fieldValue => fieldValue.FieldId == listFieldIdAsInt)
    .GetValue();

    if (recordFieldValue is null)
    {
      return null;
    }

    if (recordFieldValue is List<Guid> guids)
    {
      return listField
      .Values
      .Where(value => guids.Contains(value.Id))
      .Select(value => value.NumericValue)
      .ToArray();
    }

    var guid = recordFieldValue as Guid?;
    return listField.Values.FirstOrDefault(value => value.Id == guid).NumericValue;
  }
}
using Esprima;
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
    var listFieldId = ArgumentHelper.GetArgByIndex(arguments, 0);
    var isInt = ArgumentHelper.TryParseToType(listFieldId, out int listFieldIdAsInt);
    var listField = Context
    .Fields
    .FirstOrDefault(
      field => field.Id == listFieldIdAsInt &&
      field.Type == Onspring.API.SDK.Enums.FieldType.List
    ) as ListField;

    if (isInt is false || listField is null)
    {
      throw new ParserException($"{listFieldId} is not a valid list reference");
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
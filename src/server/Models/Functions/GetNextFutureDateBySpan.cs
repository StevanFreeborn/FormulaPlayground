using Esprima;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using server.Models;
using server.Models.Functions;

public class GetNextFutureDateBySpan : FunctionBase
{
  public GetNextFutureDateBySpan(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "GetNextFutureDateBySpan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var fieldIds = ArgumentHelper.GetArgByIndex(arguments, 1);
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

    var timespanFieldIdAsInt = fieldIdsAsInt.Count > 0 ? fieldIdsAsInt.Last() : 0;
    var timespanField = Context
    .Fields
    .FirstOrDefault(
      field => field.Id == timespanFieldIdAsInt && 
      field.Type is FieldType.TimeSpan
    );

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetNextFutureDateBySpan() takes a date, a timespan.");
    }

    if (isString is false || timespanField is null || multiRefFields.Count > 0)
    {
      throw new ParserException("The second parameter to GetNextFutureDateBySpan() does not appear to be a timespan.");
    }

    var timespanData = Context
    .FieldValues
    .FirstOrDefault(fieldValue => fieldValue.FieldId == timespanFieldIdAsInt)
    .AsTimeSpanData();

    var next = dateAsDateTime;

    while (next <= DateTime.UtcNow)
    {
      switch (timespanData.Increment)
      {
        case TimeSpanIncrement.Years:
          next = next.AddYears((int)  timespanData.Quantity);
          break;
        case TimeSpanIncrement.Months:
          next = next.AddMonths((int) timespanData.Quantity);
          break;
        case TimeSpanIncrement.Weeks:
          next = next.AddDays((double) timespanData.Quantity * 7);
          break;
        case TimeSpanIncrement.Days:
          next = next.AddDays((double) timespanData.Quantity);
          break;
        case TimeSpanIncrement.Hours:
          next = next.AddHours((double) timespanData.Quantity);
          break;
        case TimeSpanIncrement.Minutes:
          next = next.AddMinutes((double) timespanData.Quantity);
          break;
        case TimeSpanIncrement.Seconds:
        default:
          next = next.AddSeconds((double) timespanData.Quantity);
          break;
      }
    }

    return next;
  }
}
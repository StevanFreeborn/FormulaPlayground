using Esprima;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;

namespace server.Models.Functions;

public class DateAddSpan : FunctionBase
{
  public DateAddSpan(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "DateAddSpan";

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
      throw new ParserException("DateAddSpan() takes a date, a timespan.");
    }

    if (isString is false || timespanField is null || multiRefFields.Count > 0)
    {
      throw new ParserException("The second parameter to DateAddSpan() does not appear to be a timespan.");
    }

    var timespanValues = Context
    .FieldValues
    .Where(fieldValue => fieldValue.FieldId == timespanFieldIdAsInt)
    .ToList();

    if (timespanValues.Count != 1)
    {
      return null;
    }

    var timespanData = timespanValues[0].AsTimeSpanData();

    switch (timespanData.Increment)
    {
      case TimeSpanIncrement.Years:
        return dateAsDateTime.AddYears((int) timespanData.Quantity);
      case TimeSpanIncrement.Months:
        return dateAsDateTime.AddMonths((int) timespanData.Quantity);
      case TimeSpanIncrement.Weeks:
        return dateAsDateTime.AddDays((double) timespanData.Quantity * 7);
      case TimeSpanIncrement.Days:
        return dateAsDateTime.AddDays((double) timespanData.Quantity);
      case TimeSpanIncrement.Hours:
        return dateAsDateTime.AddHours((double) timespanData.Quantity);
      case TimeSpanIncrement.Minutes:
        return dateAsDateTime.AddMinutes((double) timespanData.Quantity);
      case TimeSpanIncrement.Seconds:
      default:
        return dateAsDateTime.AddSeconds((double) timespanData.Quantity);
    }
  }
}
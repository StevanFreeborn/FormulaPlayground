using Esprima;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;

namespace server.Models.Functions;

public class DateSubtractSpan : FunctionBase
{
  public DateSubtractSpan(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "DateSubtractSpan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var timespanFieldId = ArgumentHelper.GetArgByIndex(arguments, 1);
    var isInt = ArgumentHelper.TryParseToType(timespanFieldId, out int timespanFieldIdAsInt);
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
      throw new ParserException("DateSubtractSpan() takes a date, a timespan.");
    }

    if (isInt is false || timespanField is null)
    {
      throw new ParserException("The second parameter to DateSubtractSpan() does not appear to be a timespan.");
    }

    var timespanData = Context
    .FieldValues
    .FirstOrDefault(fieldValue => fieldValue.FieldId == timespanFieldIdAsInt)
    .AsTimeSpanData();

    var invertedQuantity = timespanData.Quantity * -1;

    switch (timespanData.Increment)
    {
      case TimeSpanIncrement.Years:
        return dateAsDateTime.AddYears((int) invertedQuantity);
      case TimeSpanIncrement.Months:
        return dateAsDateTime.AddMonths((int) invertedQuantity);
      case TimeSpanIncrement.Weeks:
        return dateAsDateTime.AddDays((double) invertedQuantity * 7);
      case TimeSpanIncrement.Days:
        return dateAsDateTime.AddDays((double) invertedQuantity);
      case TimeSpanIncrement.Hours:
        return dateAsDateTime.AddHours((double) invertedQuantity);
      case TimeSpanIncrement.Minutes:
        return dateAsDateTime.AddMinutes((double) invertedQuantity);
      case TimeSpanIncrement.Seconds:
      default:
        return dateAsDateTime.AddSeconds((double) invertedQuantity);
    }
  }
}
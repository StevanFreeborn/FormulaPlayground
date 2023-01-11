using Esprima;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;

namespace server.Models.Functions;

public class DateSubtractSpan : FunctionBase
{
  protected override string Name => "DateSubtractSpan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var timespan = ArgumentHelper.GetArgByIndex(arguments, 1);
    
    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(timespan, out TimeSpanData timespanAsTimeSpanData) is false
    )
    {
      throw new ParserException("DateSubtractSpan() takes a date, a timespan.");
    }

    var invertedQuantity = timespanAsTimeSpanData.Quantity * -1;

    switch (timespanAsTimeSpanData.Increment)
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
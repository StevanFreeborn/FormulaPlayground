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
    var timespan = ArgumentHelper.GetArgByIndex(arguments, 1);
    
    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("DateAddSpan() takes a date, a timespan.");
    }

    if (
      ArgumentHelper.TryParseToType(timespan, out TimeSpanData timespanAsTimeSpanData) is false
    )
    {
      throw new ParserException("The second parameter to DateAddSpan() does not appear to be a timespan.");
    }

    switch (timespanAsTimeSpanData.Increment)
    {
      case TimeSpanIncrement.Years:
        return dateAsDateTime.AddYears((int) timespanAsTimeSpanData.Quantity);
      case TimeSpanIncrement.Months:
        return dateAsDateTime.AddMonths((int) timespanAsTimeSpanData.Quantity);
      case TimeSpanIncrement.Weeks:
        return dateAsDateTime.AddDays((double) timespanAsTimeSpanData.Quantity * 7);
      case TimeSpanIncrement.Days:
        return dateAsDateTime.AddDays((double) timespanAsTimeSpanData.Quantity);
      case TimeSpanIncrement.Hours:
        return dateAsDateTime.AddHours((double) timespanAsTimeSpanData.Quantity);
      case TimeSpanIncrement.Minutes:
        return dateAsDateTime.AddMinutes((double) timespanAsTimeSpanData.Quantity);
      case TimeSpanIncrement.Seconds:
      default:
        return dateAsDateTime.AddSeconds((double) timespanAsTimeSpanData.Quantity);
    }
  }
}
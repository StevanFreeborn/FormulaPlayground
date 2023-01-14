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
    var timespan = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetNextFutureDateBySpan() takes a date, a timespan.");
    }

    if (
      ArgumentHelper.TryParseToType(timespan, out TimeSpanData timespanAsTimeSpanData) is false
    )
    {
      throw new ParserException("The second parameter to GetNextFutureDateBySpan() does not appear to be a timespan.");
    }

    var next = dateAsDateTime;

    while (next <= DateTime.UtcNow)
    {
      switch (timespanAsTimeSpanData.Increment)
      {
        case TimeSpanIncrement.Years:
          next = next.AddYears((int)  timespanAsTimeSpanData.Quantity);
          break;
        case TimeSpanIncrement.Months:
          next = next.AddMonths((int) timespanAsTimeSpanData.Quantity);
          break;
        case TimeSpanIncrement.Weeks:
          next = next.AddDays((double) timespanAsTimeSpanData.Quantity * 7);
          break;
        case TimeSpanIncrement.Days:
          next = next.AddDays((double) timespanAsTimeSpanData.Quantity);
          break;
        case TimeSpanIncrement.Hours:
          next = next.AddHours((double) timespanAsTimeSpanData.Quantity);
          break;
        case TimeSpanIncrement.Minutes:
          next = next.AddMinutes((double) timespanAsTimeSpanData.Quantity);
          break;
        case TimeSpanIncrement.Seconds:
        default:
          next = next.AddSeconds((double) timespanAsTimeSpanData.Quantity);
          break;
      }
    }

    return next;
  }
}
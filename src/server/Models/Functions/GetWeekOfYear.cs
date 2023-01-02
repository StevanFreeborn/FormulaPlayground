using System.Globalization;
using Esprima;
using server.Models.Functions;

public class GetWeekOfYear : FunctionBase
{
  protected override string Name => "GetWeekOfYear";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetWeekOfYear() takes a single date.");
    }

    var formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
    var calendar = formatInfo.Calendar;
    return calendar.GetWeekOfYear(dateAsDateTime, CalendarWeekRule.FirstDay, formatInfo.FirstDayOfWeek);
  }
}
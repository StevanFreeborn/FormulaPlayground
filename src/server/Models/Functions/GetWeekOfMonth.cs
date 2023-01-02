using System.Globalization;
using Esprima;
using server.Models.Functions;

public class GetWeekOfMonth : FunctionBase
{
  protected override string Name => "GetWeekOfMonth";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetWeekOfMonth() takes a single date.");
    }

    var dayOfMonth = dateAsDateTime.Day;
		var firstOfMonth = new DateTime(dateAsDateTime.Year, dateAsDateTime.Month, 1); 
		var formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
    var calendar = formatInfo.Calendar;
    var weekOfYear = calendar.GetWeekOfYear(dateAsDateTime, CalendarWeekRule.FirstDay, formatInfo.FirstDayOfWeek);
		var firstWeekOfYear = calendar.GetWeekOfYear(firstOfMonth, CalendarWeekRule.FirstDay, formatInfo.FirstDayOfWeek);
		var diff = weekOfYear - firstWeekOfYear;
    return diff + 1;
  }
}
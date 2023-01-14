using Esprima;
using server.Enums;

namespace server.Models.Functions;

public partial class DateAdd : FunctionBase
{
  public DateAdd(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "DateAdd";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);
    var format = ArgumentHelper.GetArgByIndex(arguments, 2);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false ||
      ArgumentHelper.TryParseToType(format, out string formatAsString) is false ||
      Enum.IsDefined(typeof(FormatOptions), formatAsString) is false
    )
    {
      throw new ParserException(@"DateAdd() takes a date, a number, and a format string. Use ""y"" for years, ""M"" for months, ""w"" for weeks, ""wd"" for work days, ""d"" for days, ""h"" for hours, ""m"" for minutes, ""s"" for seconds");
    }

    var formatAsFormatOption = Enum.Parse(typeof(FormatOptions), formatAsString);

    switch (formatAsFormatOption)
    {
      case FormatOptions.y:
        return dateAsDateTime.AddYears(numberAsInt);
      case FormatOptions.M:
        return dateAsDateTime.AddMonths(numberAsInt);
      case FormatOptions.w:
        return dateAsDateTime.AddDays(numberAsInt * 7);
      case FormatOptions.wd:
        while (numberAsInt != 0)
        {
          // handle number being negative or positive
          var modifier = Math.Sign(numberAsInt);
          dateAsDateTime = dateAsDateTime.AddDays(modifier);
          var currDayOfWeek = dateAsDateTime.DayOfWeek;
          // only decrement the number if date advanced too
          // is a non-work date.
          if (currDayOfWeek is not DayOfWeek.Saturday or DayOfWeek.Sunday)
          {
            numberAsInt -= modifier;
          }
        }
        return dateAsDateTime;
      case FormatOptions.d:
        return dateAsDateTime.AddDays(numberAsInt);
      case FormatOptions.h:
        return dateAsDateTime.AddHours(numberAsInt);
      case FormatOptions.m:
        return dateAsDateTime.AddMinutes(numberAsInt);
      case FormatOptions.s:
      default:
        return dateAsDateTime.AddSeconds(numberAsInt);
    }
  }
}


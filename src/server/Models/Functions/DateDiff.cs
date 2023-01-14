using Esprima;
using server.Enums;

namespace server.Models.Functions;

public class DateDiff : FunctionBase
{
  public DateDiff(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "DateDiff";

  protected override object Function(params object[] arguments)
  {
    var endDate = ArgumentHelper.GetArgByIndex(arguments, 0);
    var startDate = ArgumentHelper.GetArgByIndex(arguments, 1);
    var format = ArgumentHelper.GetArgByIndex(arguments, 2);

    if (
      ArgumentHelper.TryParseToType(endDate, out DateTime endAsDateTime) is false ||
      ArgumentHelper.TryParseToType(startDate, out DateTime startAsDateTime) is false
    )
    {
      throw new ParserException("Two date parameters are required for the DateDiff() function.");
    }

    if (
      ArgumentHelper.TryParseToType(format, out string formatAsString) is false ||
      Enum.IsDefined(typeof(FormatOptions), formatAsString) is false
    )
    {
      throw new ParserException(@"Incorrect date difference format for DateDiff() function. Use ""y"" for years, ""M"" for months, ""w"" for weeks, ""wd"" for work days, ""d"" for days, ""h"" for hours, ""m"" for minutes, ""s"" for seconds");
    }

    var formatAsFormatOption = Enum.Parse(typeof(FormatOptions), formatAsString);
    var difference = endAsDateTime - startAsDateTime;

    switch (formatAsFormatOption)
    {
      case FormatOptions.y:
        return DateHelper.GetYears(startAsDateTime, endAsDateTime, difference.TotalDays);
      case FormatOptions.M:
        return DateHelper.GetMonths(startAsDateTime, endAsDateTime, difference.TotalDays);
      case FormatOptions.w:
        return DateHelper.GetWholeNumber(difference.TotalDays / 7);
      case FormatOptions.wd:
        return DateHelper.GetWorkDays(startAsDateTime, difference.TotalDays);
      case FormatOptions.d:
        return DateHelper.GetWholeNumber(difference.TotalDays);
      case FormatOptions.h:
        return DateHelper.GetWholeNumber(difference.TotalHours);
      case FormatOptions.m:
        return DateHelper.GetWholeNumber(difference.TotalMinutes);
      case FormatOptions.s:
      default:
        return DateHelper.GetWholeNumber(difference.TotalSeconds);
    }
  }
}
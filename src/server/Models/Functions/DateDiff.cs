using Esprima;
using server.Enums;

namespace server.Models.Functions;

public class DateDiff : FunctionBase
{
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
        return GetYears(startAsDateTime, endAsDateTime, difference.TotalDays);
      case FormatOptions.M:
        return GetMonths(startAsDateTime, endAsDateTime, difference.TotalDays);
      case FormatOptions.w:
        return GetWholeNumber(difference.TotalDays / 7);
      case FormatOptions.wd:
        return GetWorkDays(startAsDateTime, difference.TotalDays);
      case FormatOptions.d:
        return GetWholeNumber(difference.TotalDays);
      case FormatOptions.h:
        return GetWholeNumber(difference.TotalHours);
      case FormatOptions.m:
        return GetWholeNumber(difference.TotalMinutes);
      case FormatOptions.s:
      default:
        return GetWholeNumber(difference.TotalSeconds);
    }
  }

  // this feels like a brute force solution
  // TODO: Research how others have solved this calculation
  private int GetYears(DateTime startDate, DateTime endDate, double TotalDays)
  {
    // if passed in total days is negative
    // then call method again with the dates inverted
    // so that logic in do while loop is correct
    if (TotalDays < 0)
    {
      return GetYears(endDate, startDate, TotalDays);
    }

    int years = 0;

    do
    {
      startDate = startDate.AddYears(1);

      if (startDate > endDate)
      {
        // return year count with proper sign according
        // to passed in total days.
        return years * Math.Sign(TotalDays);
      }

      years++;
    } while (true);
  }

  // this feels like a brute force solution
  // TODO: Research how others have solved this calculation
  private int GetMonths(DateTime startDate, DateTime endDate, double TotalDays)
  {
    // if passed in total days is negative
    // then call method again with the dates inverted
    // so that logic in do while loop is correct
    if (TotalDays < 0)
    {
      return GetMonths(endDate, startDate, TotalDays);
    }

    int months = 0;

    do
    {
      startDate = startDate.AddMonths(1);

      if (startDate > endDate)
      {
        // return month count with proper sign according
        // to passed in total days.
        return months * Math.Sign(TotalDays);
      }

      months++;
    } while (true);
  }

  private int GetWorkDays(DateTime startDate, double TotalDays)
  {
    // no requirement for start date to actually be before end date
    // so total days might be negative
    var absDaysAsInt = (int) Math.Abs(TotalDays);

    // prob not most efficient approach...but could be extended to account for 
    // holidays which might be useful.
    var count = Enumerable
    .Range(1, absDaysAsInt)
    .Select(num => startDate.AddDays(num))
    .Count(date => date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday);

    // return count with correct sign based on sign of total days
    return count * Math.Sign(TotalDays);
  }

  private int GetWholeNumber(double number)
  {
    return (int) Math.Floor(number);
  }
}
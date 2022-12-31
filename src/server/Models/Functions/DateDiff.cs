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
        var years = GetMonths(startAsDateTime, endAsDateTime, difference.TotalDays) / 12;
        return GetWholeNumber(years);
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

  private int GetMonths(DateTime startDate, DateTime endDate, double TotalDays)
  {
    // if difference in dates total days is negative
    // then call method again with the dates inverted
    // so that logic in do while loop is correct
    var diff = endDate - startDate;

    if (diff.TotalDays < 0)
    {
      return GetMonths(endDate, startDate, TotalDays);
    }

    var months = 0;
    var originalStartDateDay = startDate.Day;

    while (startDate < endDate)
    {
      startDate = startDate.AddMonths(1);
      var daysInMonth = DateTime.DaysInMonth(startDate.Year, startDate.Month);

      // account for the original start date's day
      // of month possibly being greater than the 
      // number of days in the current start date's 
      // month so on each loop make sure to correct 
      // for when this occurs.
      // i.e. 12/30/2022 -> 1/30/2023 -> 2/28/2023 -> 3/30/2023
      if (startDate.Day != originalStartDateDay)
      {
        var offset = daysInMonth < originalStartDateDay
        ? daysInMonth - startDate.Day
        : originalStartDateDay - startDate.Day;
        startDate.AddDays(offset);
      }

      var isSameYearAndMonth = startDate.Year == endDate.Year && startDate.Month == endDate.Month;
      var isEndDateDayAfterOrginalStartDateDay = endDate.Day >= originalStartDateDay;
      var isEndDateAfterCurrentStartDate = endDate.Date > startDate.Date;

      if (
        isSameYearAndMonth is false && (isEndDateDayAfterOrginalStartDateDay is true || isEndDateAfterCurrentStartDate is true)
      )
      {
        months++;
      }
    }

    // return month count with proper sign according
    // to passed in total days.
    return months * Math.Sign(TotalDays);
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
using System.Globalization;

public class DateHelper
{
  public static int GetWeekOfMonth(DateTime date)
  {
    var dayOfMonth = date.Day;
		var firstOfMonth = new DateTime(date.Year, date.Month, 1); 
    var weekOfYear = GetWeekOfYear(date);
		var firstWeekOfYear = GetWeekOfYear(date);
		var diff = weekOfYear - firstWeekOfYear;
    return diff + 1;  
  }

  public static int GetWeekOfYear(DateTime date)
  {
    var formatInfo = CultureInfo.CurrentCulture.DateTimeFormat;
    var calendar = formatInfo.Calendar;
    return calendar.GetWeekOfYear(date, CalendarWeekRule.FirstDay, formatInfo.FirstDayOfWeek);
  }

  public static int GetYears(DateTime startDate, DateTime endDate, double totalDays)
  {
    var years = GetMonths(startDate, endDate, totalDays) / 12;
    return GetWholeNumber(years);
  }

  public static int GetMonths(DateTime startDate, DateTime endDate, double totalDays)
  {
    // if difference in dates total days is negative
    // then call method again with the dates inverted
    // so that logic in do while loop is correct
    var diff = endDate - startDate;

    if (diff.TotalDays < 0)
    {
      return GetMonths(endDate, startDate, totalDays);
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
    return months * Math.Sign(totalDays);
  }

  public static int GetWorkDays(DateTime startDate, double totalDays)
  {
    // no requirement for start date to actually be before end date
    // so total days might be negative
    var absDaysAsInt = (int) Math.Abs(totalDays);

    // prob not most efficient approach...but could be extended to account for 
    // holidays which might be useful.
    var count = Enumerable
    .Range(1, absDaysAsInt)
    .Select(num => startDate.AddDays(num))
    .Count(date => date.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday);

    // return count with correct sign based on sign of total days
    return count * Math.Sign(totalDays);
  }

  public static int GetWholeNumber(double number)
  {
    return (int) Math.Floor(number);
  }
}
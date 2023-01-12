using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;

namespace server.Extensions;

public static class TimeSpanDataExtensions
{
  public static string GetAsString(this TimeSpanData timeSpanData)
  {
    var everyText = "Every";
    var quantityText = GetQuantityText(timeSpanData);

    if (timeSpanData.Recurrence is TimeSpanRecurrenceType.None)
    {
      return quantityText;
    }

    if (timeSpanData.Recurrence is TimeSpanRecurrenceType.EndByDate)
    {
      if (timeSpanData.EndByDate.HasValue)
      {
        var dateText = timeSpanData.EndByDate.Value.ToString("M/d/yyyy h:mm tt");
        return $"{everyText} {quantityText} End Before {dateText}";
      }
      
      return $"{everyText} {quantityText} Indefinitely";
    }

    if (timeSpanData.Recurrence is TimeSpanRecurrenceType.EndAfterOccurrences)
    {
      if (timeSpanData.EndAfterOccurrences.HasValue)
      {
        return $"{everyText} {quantityText} End After {timeSpanData.EndAfterOccurrences.Value} Occurrence(s)";
      }

      return $"{everyText} {quantityText} Indefinitely";
    }

    return timeSpanData.ToString();
  }

  private static string GetQuantityText(TimeSpanData timeSpanData)
  {
    var quantity = timeSpanData.Quantity;
    switch (timeSpanData.Increment)
    {
      case TimeSpanIncrement.Years:
        return $"{quantity} Year(s)";
      case TimeSpanIncrement.Months:
        return $"{quantity} Month(s)";
      case TimeSpanIncrement.Weeks:
        return $"{quantity} Week(s)";
      case TimeSpanIncrement.Days:
        return $"{quantity} Day(s)";
      case TimeSpanIncrement.Hours:
        return $"{quantity} Hour(s)";
      case TimeSpanIncrement.Minutes:
        return $"{quantity} Minute(s)";
      case TimeSpanIncrement.Seconds:
      default:
        return $"{quantity} Second(s)";
    }
  }
}
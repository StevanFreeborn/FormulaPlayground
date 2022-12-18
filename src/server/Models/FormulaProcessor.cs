namespace server.Models;

public class FormulaProcessor
{
  public static string GetResultAsString(object obj, TimeZoneInfo timezone)
  {
    var objectAsString = ConvertObjectToString(obj, timezone);
    return objectAsString;
  }

  private static string ConvertObjectToString(object obj, TimeZoneInfo timezone)
  {
    if (obj is null) {
      return null;
    }

    if (obj is string) {
      return obj as string;
    }

    if (obj is DateTime date) {
      date = TimeZoneInfo.ConvertTimeFromUtc(date, timezone);
      return date.ToString();
    }

    if (obj is object[]) {
      var array = obj as object[];
      return string.Join(", ", array.Where(element => element != null).Select(element => ConvertObjectToString(element, timezone)));
    }

    return obj.ToString();
  }
}
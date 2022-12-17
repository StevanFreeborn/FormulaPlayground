namespace server.Models;

public class FormulaProcessor
{
  public static string GetResultAsString(object obj)
  {
    var objectAsString = ConvertObjectToString(obj);
    return objectAsString;
  }

  private static string ConvertObjectToString(object obj)
  {
    if (obj is null) {
      return null;
    }

    if (obj is string) {
      return obj as string;
    }

    if (obj is DateTime?) {
      var date = obj as DateTime?;
      return date.Value.ToLongDateString();
    }

    if (obj is object[]) {
      var array = obj as object[];
      return string.Join(", ", array.Where(element => element != null).Select(ConvertObjectToString));
    }

    return obj.ToString();
  }

  public static DateTime GetResultAsDateTime()
  {
    throw new NotImplementedException();
  }

  public static decimal GetResultAsNumber()
  {
    throw new NotImplementedException();
  }

  public static string GetResultAsListValue()
  {
    throw new NotImplementedException();
  }
}
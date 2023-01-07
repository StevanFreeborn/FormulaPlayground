using Esprima;
using server.Models.Functions;

public class GetNextAnnualDate : FunctionBase
{
  protected override string Name => "GetNextAnnualDate";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var month = ArgumentHelper.GetArgByIndex(arguments, 0);
    var day = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      if (
        ArgumentHelper.TryParseToType(month, out int monthAsInt) is false ||
        ArgumentHelper.TryParseToType(day, out int dayAsInt) is false
      )
      {
        throw new ParserException("GetNextAnnualDate() takes a single date OR a month and day as two numbers");
      }

      try
      {
        dateAsDateTime = new DateTime(2016, monthAsInt, dayAsInt, 0, 0, 0, DateTimeKind.Utc);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw new ParserException("Month and Day parameters for GetNextAnnualDate() do not appear to be a valid date");
      }
    }

    var next = dateAsDateTime;
    var yearsToAdd = 1;

    while (next <= DateTime.UtcNow)
    {
      next = dateAsDateTime.AddYears(yearsToAdd);
      yearsToAdd++;
    }

    return next;
  }
}
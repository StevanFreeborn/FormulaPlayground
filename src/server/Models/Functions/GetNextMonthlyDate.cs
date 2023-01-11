using Esprima;
using server.Models.Functions;

public class GetNextMonthlyDate : FunctionBase
{
  protected override string Name => "GetNextMonthlyDate";

  protected override object Function(params object[] arguments)
  {
    var dateOrDay = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(dateOrDay, out DateTime dateAsDateTime) is false
    )
    {
      if (
        ArgumentHelper.TryParseToType(dateOrDay, out int dayAsInt) is false
      )
      {
        throw new ParserException("GetNextMonthlyDate() takes a single date OR a day as a single number");
      }

      try
      {
        dateAsDateTime = new DateTime(2016, 1, dayAsInt, 0, 0, 0, DateTimeKind.Utc);
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw new ParserException("Day parameter for GetNextMonthlyDate() does not appear to be a valid date");
      }
    }

    var next = dateAsDateTime;
    var monthsToAdd = 1;

    while (next <= DateTime.UtcNow)
    {
      next = dateAsDateTime.AddMonths(monthsToAdd);
      monthsToAdd++;
    }

    return next;
  }
}
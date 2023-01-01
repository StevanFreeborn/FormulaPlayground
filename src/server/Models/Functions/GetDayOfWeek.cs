using Esprima;
using server.Models.Functions;

public class GetDayOfWeek : FunctionBase
{
  protected override string Name => "GetDayOfWeek";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetDayOfWeek() takes a single date.");
    }

    return dateAsDateTime.DayOfWeek;
  }
}
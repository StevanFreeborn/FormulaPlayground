using Esprima;
using server.Models.Functions;

public class IsToday : FunctionBase
{
  protected override string Name => "IsToday";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsToday() takes a single date.");
    }

    var todayStart = DateTime.UtcNow.Date;
    var tomorrowStart = todayStart.AddDays(1);

    return dateAsDateTime >= todayStart && dateAsDateTime < tomorrowStart;
  }
}
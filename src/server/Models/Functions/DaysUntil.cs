using Esprima;
using server.Models.Functions;

public class DaysUntil : FunctionBase
{
  protected override string Name => "DaysUntil";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("DaysUntil() takes a single date.");
    }

    var diff = dateAsDateTime - DateTime.UtcNow;

    if (diff.TotalDays < 0)
    {
      return 0;
    }

    return (int) diff.TotalDays;
  }
}
using Esprima;
using server.Models.Functions;

public class IsBeforeToday : FunctionBase
{
  protected override string Name => "IsBeforeToday";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsBeforeToday() takes a single date.");
    }

    return dateAsDateTime < DateTime.UtcNow.Date;
  }
}
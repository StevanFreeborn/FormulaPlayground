using Esprima;
using server.Models.Functions;

public class IsOnOrAfterToday : FunctionBase
{
  protected override string Name => "IsOnOrAfterToday";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsOnOrAfterToday() takes a single date.");
    }

    return dateAsDateTime >= DateTime.UtcNow.Date;
  }
}
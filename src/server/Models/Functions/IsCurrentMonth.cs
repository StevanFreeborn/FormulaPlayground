using Esprima;
using server.Models.Functions;

public class IsCurrentMonth : FunctionBase
{
  protected override string Name => "IsCurrentMonth";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsCurrentMonth() takes a single date.");
    }

    return dateAsDateTime.Month == DateTime.UtcNow.Month;
  }
}
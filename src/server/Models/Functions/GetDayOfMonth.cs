using Esprima;
using server.Models.Functions;

public class GetDayOfMonth : FunctionBase
{
  protected override string Name => "GetDayOfMonth";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetDayOfMonth() takes a single date.");
    }

    return dateAsDateTime.Day;
  }
}
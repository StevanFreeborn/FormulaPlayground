using Esprima;
using server.Models.Functions;

public class GetMonth : FunctionBase
{
  protected override string Name => "GetMonth";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetMonth() takes a single date.");
    }

    return dateAsDateTime.Month;
  }
}
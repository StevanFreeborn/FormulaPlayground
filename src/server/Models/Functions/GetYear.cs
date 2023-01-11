using Esprima;
using server.Models.Functions;

public class GetYear : FunctionBase
{
  protected override string Name => "GetYear";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetYear() takes a single date.");
    }

    return dateAsDateTime.Year;
  }
}
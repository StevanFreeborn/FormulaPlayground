using Esprima;
using server.Models;
using server.Models.Functions;

public class GetWeekOfMonth : FunctionBase
{
  public GetWeekOfMonth(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "GetWeekOfMonth";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetWeekOfMonth() takes a single date.");
    }

    return DateHelper.GetWeekOfMonth(dateAsDateTime);
  }
}
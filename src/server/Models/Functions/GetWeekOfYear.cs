using Esprima;
using server.Models;
using server.Models.Functions;

public class GetWeekOfYear : FunctionBase
{
  public GetWeekOfYear(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "GetWeekOfYear";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetWeekOfYear() takes a single date.");
    }

    return DateHelper.GetWeekOfYear(dateAsDateTime);
  }
}
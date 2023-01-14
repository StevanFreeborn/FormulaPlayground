using Esprima;
using server.Models;
using server.Models.Functions;

public class GetDayOfYear : FunctionBase
{
  public GetDayOfYear(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "GetDayOfYear";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("GetDayOfYear() takes a single date.");
    }

    return dateAsDateTime.DayOfYear;
  }
}
using Esprima;
using server.Models;
using server.Models.Functions;

public class IsCurrentWeek : FunctionBase
{
  public IsCurrentWeek(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "IsCurrentWeek";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsCurrentWeek() takes a single date.");
    }

    var todaysWeek = DateHelper.GetWeekOfYear(DateTime.UtcNow);
    var dateWeek = DateHelper.GetWeekOfYear(dateAsDateTime);

    return dateWeek == todaysWeek;
  }
}
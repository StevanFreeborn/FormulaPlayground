using Esprima;
using server.Models;
using server.Models.Functions;

public class WorkdaysSince : FunctionBase
{
  public WorkdaysSince(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "WorkdaysSince";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("WorkdaysSince() takes a single date.");
    }

    var difference = DateTime.UtcNow - dateAsDateTime;

    if (difference.TotalDays < 0)
    {
      return 0;
    }

    return DateHelper.GetWorkDays(dateAsDateTime, difference.TotalDays);
  }
}
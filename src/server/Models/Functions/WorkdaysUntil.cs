using Esprima;
using server.Models;
using server.Models.Functions;

public class WorkdaysUntil : FunctionBase
{
  public WorkdaysUntil(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "WorkdaysUntil";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("WorkdaysUntil() takes a single date.");
    }

    var now = DateTime.UtcNow;
    var difference = dateAsDateTime - now;

    if (difference.TotalDays < 0)
    {
      return 0;
    }

    return DateHelper.GetWorkDays(now, difference.TotalDays);
  }
}
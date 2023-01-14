using Esprima;
using server.Models;
using server.Models.Functions;

public class Workdays : FunctionBase
{
  public Workdays(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Workdays";

  protected override object Function(params object[] arguments)
  {
    var startDate = ArgumentHelper.GetArgByIndex(arguments, 0);
    var endDate = ArgumentHelper.GetArgByIndex(arguments, 1);
    
    if (
      ArgumentHelper.TryParseToType(endDate, out DateTime endDateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(startDate, out DateTime startDateAsDateTime) is false
    )
    {
      throw new ParserException("Workdays() takes two dates.");
    }

    var isStartAfterEnd = startDateAsDateTime > endDateAsDateTime;

    var start = isStartAfterEnd ? endDateAsDateTime : startDateAsDateTime;
    var end = isStartAfterEnd ? startDateAsDateTime.AddDays(1) : endDateAsDateTime.AddDays(1);
    var difference = end - start;

    return DateHelper.GetWorkDays(start, difference.TotalDays);
  }
}
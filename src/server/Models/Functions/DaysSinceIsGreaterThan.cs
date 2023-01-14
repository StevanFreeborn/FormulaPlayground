using Esprima;

namespace server.Models.Functions;

public class DaysSinceIsGreaterThan : FunctionBase
{
  public DaysSinceIsGreaterThan(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "DaysSinceIsGreaterThan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false
    )
    {
      throw new ParserException("DaysSinceIsGreaterThan() takes a date and a number.");
    }

    var diff = DateTime.UtcNow - dateAsDateTime;
    var totalDays = (int) diff.TotalDays;

    return totalDays > numberAsInt;
  }
}
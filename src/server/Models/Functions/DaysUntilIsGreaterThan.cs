using Esprima;
using server.Models.Functions;

public class DaysUntilIsGreaterThan : FunctionBase
{
  protected override string Name => "DaysUntilIsGreaterThan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false
    )
    {
      throw new ParserException("DaysUntilIsGreaterThan() takes a date and a number.");
    }

    var diff = dateAsDateTime - DateTime.UtcNow;
    var totalDays = (int) diff.TotalDays;

    return totalDays > numberAsInt;
  }
}
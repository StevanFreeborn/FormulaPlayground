using Esprima;
using server.Models;
using server.Models.Functions;

public class IsWithinPriorDays : FunctionBase
{
  public IsWithinPriorDays(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "IsWithinPriorDays";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false
    )
    {
      throw new ParserException("IsWithinPriorDays() takes a date and a number.");
    }

    var isDateOnly = dateAsDateTime == dateAsDateTime.Date;
    var end = isDateOnly ? dateAsDateTime.AddDays(1) : DateTime.UtcNow;
    var start = isDateOnly ? DateTime.UtcNow.Date.AddDays(numberAsInt * -1) : end.AddDays(numberAsInt * -1);

    return dateAsDateTime >= start && dateAsDateTime < end;
  }
}
using Esprima;
using server.Models.Functions;

public class IsWithinNextDays : FunctionBase
{
  protected override string Name => "IsWithinNextDays";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false
    )
    {
      throw new ParserException("IsWithinNextDays() takes a date and a number.");
    }

    var isDateOnly = dateAsDateTime == dateAsDateTime.Date;
    var start = isDateOnly ? DateTime.UtcNow.Date : DateTime.UtcNow;
    var end = isDateOnly ? start.AddDays(numberAsInt + 1) : start.AddDays(numberAsInt);

    return dateAsDateTime >= start && dateAsDateTime < end;
  }
}

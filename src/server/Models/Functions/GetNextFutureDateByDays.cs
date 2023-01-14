using Esprima;
using server.Models;
using server.Models.Functions;

public class GetNextFutureDateByDays : FunctionBase
{
  public GetNextFutureDateByDays(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "GetNextFutureDateByDays";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var number = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(number, out int numberAsInt) is false
    )
    {
      throw new ParserException("GetNextFutureDateByDays() takes a date, a number.");
    }

    if (numberAsInt < 1)
    {
      throw new ParserException("Number parameter for GetNextFutureDateByDays() cannot be less than 1.");
    }

    var next = dateAsDateTime;

    while (next <= DateTime.UtcNow)
    {
      next = next.AddDays(numberAsInt);
    }

    return next;
  }
}
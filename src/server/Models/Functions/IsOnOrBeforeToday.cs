using Esprima;
using server.Models;
using server.Models.Functions;

public class IsOnOrBeforeToday : FunctionBase
{
  public IsOnOrBeforeToday(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "IsOnOrBeforeToday";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsOnOrBeforeToday() takes a single date.");
    }

    return dateAsDateTime < DateTime.UtcNow.Date.AddDays(1);
  }
}
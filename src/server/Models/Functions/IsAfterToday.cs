using Esprima;
using server.Models;
using server.Models.Functions;

public class IsAfterToday : FunctionBase
{
  public IsAfterToday(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "IsAfterToday";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsAfterToday() takes a single date.");
    }

    return dateAsDateTime > DateTime.UtcNow.Date.AddDays(1);
  }
}
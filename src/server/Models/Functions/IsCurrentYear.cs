using Esprima;
using server.Models;
using server.Models.Functions;

public class IsCurrentYear : FunctionBase
{
  public IsCurrentYear(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "IsCurrentYear";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("IsCurrentYear() takes a single date.");
    }

    return dateAsDateTime.Year == DateTime.UtcNow.Year;
  }
}
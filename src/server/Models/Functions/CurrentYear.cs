namespace server.Models.Functions;

public class CurrentYear : FunctionBase
{
  public CurrentYear(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "CurrentYear";

  protected override object Function(params object[] arguments)
  {
    return DateTime.UtcNow.Year;
  }
}
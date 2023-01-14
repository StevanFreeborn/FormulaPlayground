namespace server.Models.Functions;

public class CurrentMonth : FunctionBase
{
  public CurrentMonth(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "CurrentMonth";

  protected override object Function(params object[] arguments)
  {
    return DateTime.UtcNow.Month;
  }
}
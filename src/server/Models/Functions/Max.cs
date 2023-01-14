namespace server.Models.Functions;

public class Max : FunctionBase
{
  public Max(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Max";

  protected override object Function(params object[] arguments)
  {
    var args = ArgumentHelper.FlattenArgumentsArray(arguments);
    var numbers = ArgumentHelper.GetArgsAsDoubles(args);
    if (numbers.Count == 0)
    {
      return null;
    }
    return numbers.Max();
  }
}
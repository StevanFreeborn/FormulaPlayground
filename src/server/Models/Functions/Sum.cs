namespace server.Models.Functions;

public class Sum : FunctionBase
{
  public Sum(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Sum";

  protected override object Function(params object[] arguments)
  {
    var args = ArgumentHelper.FlattenArgumentsArray(arguments);
    var numbers = ArgumentHelper.GetArgsAsDoubles(args);
    if (numbers.Count == 0)
    {
      return null;
    }
    return numbers.Sum();
  }
}
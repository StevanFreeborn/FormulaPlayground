using server.Models;
using server.Models.Functions;

public class Count : FunctionBase
{
  public Count(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Count";

  protected override object Function(params object[] arguments)
  {
    var args = ArgumentHelper.FlattenArgumentsArray(arguments);
    return args.Count;
  }
}
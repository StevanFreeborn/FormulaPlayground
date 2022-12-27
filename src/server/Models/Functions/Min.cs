namespace server.Models.Functions;

public class Min : FunctionBase
{
  protected override string Name => "Min";

  protected override object Function(params object[] arguments)
  {
    var args = ArgumentHelper.FlattenArgumentsArray(arguments);
    var numbers = ArgumentHelper.GetArgsAsDoubles(args);
    if (numbers.Count == 0)
    {
      return null;
    }
    return numbers.Min();
  }
}
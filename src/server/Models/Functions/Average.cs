namespace server.Models.Functions;

public class Average : FunctionBase
{
  protected override string Name => "Average";

  protected override object Function(params object[] arguments)
  {
    if (arguments == null)
    {
      return null;
    }
    var args = ArgumentHelper.FlattenArgumentsArray(arguments);
    var numbers = ArgumentHelper.GetArgsAsDoubles(args);
    return numbers.Average();
  }
}
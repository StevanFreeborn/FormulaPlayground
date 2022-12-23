namespace server.Models.Functions;

public class Sum : FunctionBase
{
  protected override string Name => "Sum";

  protected override object Function(params object[] arguments)
  {
    if (arguments == null)
    {
      return null;
    }
    var args = FlattenArgumentsArray(arguments);
    var numbers = GetArgsAsDoubles(args);
    return numbers.Sum();
  }

  private static List<double> GetArgsAsDoubles(List<object> args)
  {
    var numbers = new List<double>();
    foreach (var arg in args)
    {
      if (arg is double argAsDouble)
      {
        numbers.Add(argAsDouble);
      }
      if (arg is string argAsString)
      {
        if (double.TryParse(argAsString, out double number))
        {
          numbers.Add(number);
        }
      }
    }
    return numbers;
  }

  private static List<object> FlattenArgumentsArray(object[] arguments)
  {
    var args = new List<object>();
    foreach (var argument in arguments)
    {
      if (argument is object[] array)
      {
        var subArguments = FlattenArgumentsArray(array);
        foreach (var subArgument in subArguments)
        {
          args.Add(subArgument);
        }
      }
      else
      {
        args.Add(argument);
      }
    }
    return args;
  }
}
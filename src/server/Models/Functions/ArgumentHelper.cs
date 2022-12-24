namespace server.Models.Functions;

public class ArgumentHelper
{
  public static List<double> GetArgsAsDoubles(List<object> args)
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

  public static List<object> FlattenArgumentsArray(object[] arguments)
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
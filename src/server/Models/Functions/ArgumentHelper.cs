using System.Globalization;
using Jint;

namespace server.Models.Functions;

public class ArgumentHelper
{
  public static List<double> GetArgsAsDoubles(List<object> args)
  {
    var numbers = new List<double>();
    foreach (var arg in args)
    {
      if (TryParseToType(arg, out double argAsDouble))
      {
        numbers.Add(argAsDouble);
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

  public static bool TryParseToType<T>(object arg, out T argAsType)
  {
    var engine = new Engine();
    object converted;

    if (engine.ClrTypeConverter.TryConvert(arg, typeof(T), CultureInfo.InvariantCulture, out converted))
    {
      argAsType = (T) converted;
      return true;
    };

    argAsType = default(T);
    return false;
  }
}
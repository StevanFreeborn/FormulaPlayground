using Jint.Runtime;

namespace server.Models.Functions;

public class RoundUp : FunctionBase
{
  protected override string Name => "RoundUp";

  protected override object Function(params object[] arguments)
  {
    var numberIndex = 0;
    var precisionIndex = 1;

    var number = arguments.Length <= numberIndex ? null : arguments[numberIndex];
    var precision = arguments.Length <= precisionIndex ? 0 : arguments[precisionIndex];

    if (number is null)
    {
      return null;
    }

    if (
      Double.TryParse(number.ToString(), out double numberAsDouble) is false ||
      int.TryParse(precision.ToString(), out int precisionAsInt) is false
      )
    {
      throw new JavaScriptException("RoundUp() takes a number and a number of digits.");
    }

    var multiplier = Math.Pow(10, precisionAsInt);
    return Math.Ceiling(numberAsDouble * multiplier) / multiplier;
  }
}
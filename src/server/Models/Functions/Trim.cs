using System.Globalization;
using Esprima;
using Jint;

namespace server.Models.Functions;

public class Trim : FunctionBase
{
  protected override string Name => "Trim";

  protected override object Function(params object[] arguments)
  {
    var textIndex = 0;
    var text = arguments.Length <= textIndex ? null : arguments[textIndex];

    if (text is null)
    {
      return null;
    }

    if (TryParseToType<string>(text, out string textAsString) is false)
    {
      throw new ParserException("Trim() takes a string.");
    }

    return textAsString.Trim();
  }

  private bool TryParseToType<T>(object arg, out T argAsType) where T : class
  {
    var engine = new Engine();
    object converted;

    if (engine.ClrTypeConverter.TryConvert(arg, typeof(T), CultureInfo.InvariantCulture, out converted))
    {
      argAsType = converted as T;
      return true;
    };

    argAsType = default(T);
    return false;
  }
}
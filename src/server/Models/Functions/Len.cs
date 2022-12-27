using Esprima;

namespace server.Models.Functions;

public class Len : FunctionBase
{
  protected override string Name => "Len";

  protected override object Function(params object[] arguments)
  {
    var textIndex = 0;
    var text = arguments.Length <= textIndex ? null : arguments[textIndex];

    if (text is null)
    {
      return null;
    }

    if (ArgumentHelper.TryParseToType(text, out string textAsString) is false)
    {
      throw new ParserException("Len() takes a string.");
    }

    return textAsString.Length;
  }
}
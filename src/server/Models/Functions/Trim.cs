using Esprima;

namespace server.Models.Functions;

public class Trim : FunctionBase
{
  public Trim(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Trim";

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
      throw new ParserException("Trim() takes a string.");
    }

    return textAsString.Trim();
  }
}
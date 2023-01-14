using Esprima;

namespace server.Models.Functions;

public class Left : FunctionBase
{
  public Left(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Left";

  protected override object Function(params object[] arguments)
  {
    var textIndex = 0;
    var numOfCharsIndex = 1;
    var text = arguments.Length <= textIndex ? null : arguments[textIndex];
    var numOfChars = arguments.Length <= numOfCharsIndex ? null : arguments[numOfCharsIndex];

    if (
      ArgumentHelper.TryParseToType(text, out string textAsString) is false ||
      ArgumentHelper.TryParseToType(numOfChars, out int numOfCharsAsInt) is false
    )
    {
      throw new ParserException("Left() takes a string and a number.");
    }

    // account for case when length
    // of text is shorter than the number
    // of characters entered.
    var length = Math.Min(numOfCharsAsInt, textAsString.Length);
    return textAsString.Substring(0, length);
  }
}
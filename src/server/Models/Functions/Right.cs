using Esprima;

namespace server.Models.Functions;

public class Right : FunctionBase
{
  public Right(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "Right";

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
      throw new ParserException("Right() takes a string and a number.");
    }

    // account for case when length
    // of text is shorter than the number
    // of characters entered.
    var offset = Math.Min(numOfCharsAsInt, textAsString.Length);
    var startIndex = textAsString.Length - offset;
    return textAsString.Substring(startIndex);
  }
}
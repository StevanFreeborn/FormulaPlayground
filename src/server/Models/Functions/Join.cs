using Esprima;

namespace server.Models.Functions;

public class Join : FunctionBase
{
  protected override string Name => "Join";

  protected override object Function(params object[] arguments)
  {
    var stringsToJoinIndex = 0;
    var separatorIndex = 1;
    var stringsToJoin = arguments.Length <= stringsToJoinIndex ? null : arguments[stringsToJoinIndex];
    var separator = arguments.Length <= separatorIndex ? null : arguments[separatorIndex];

    if (stringsToJoin is null || separator is null)
    {
      return null;
    }

    if (
      ArgumentHelper.TryParseToType(separator, out string separatorAsString) is false ||
      ArgumentHelper.TryParseToType(stringsToJoin, out object[] stringsToJoinAsArray) is false
    )
    {
      throw new ParserException("Join() takes a list of values and a separator.");
    }

    return String.Join(separatorAsString, stringsToJoinAsArray);
  }
}
using Esprima;

namespace server.Models.Functions;

public class DaysSince : FunctionBase
{
  protected override string Name => "DaysSince";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    
    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("DaysSince() takes a single date.");
    }
    
    var diff = DateTime.UtcNow - dateAsDateTime;

    return (int) diff.TotalDays;
  }
}
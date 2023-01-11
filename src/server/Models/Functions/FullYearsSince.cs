using Esprima;
using server.Models.Functions;

public class FullYearsSince : FunctionBase
{
  protected override string Name => "FullYearsSince";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    
    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false
    )
    {
      throw new ParserException("FullYearsSince() takes a single date.");
    }
    
    var diff = DateTime.UtcNow - dateAsDateTime;

    if (diff.TotalDays < 0)
    {
      return 0;
    }

    return DateHelper.GetYears(dateAsDateTime, DateTime.UtcNow, diff.TotalDays);
  }
}
using Esprima;

namespace server.Models.Functions;

public class _Date : FunctionBase
{
  public _Date(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "_Date";

  protected override object Function(params object[] arguments)
  {
    var yearIndex = 0;
    var monthIndex = 1;
    var dayIndex = 2;
    var year = arguments.Length <= yearIndex ? null : arguments[yearIndex];
    var month = arguments.Length <= monthIndex ? null : arguments[monthIndex];
    var day = arguments.Length <= dayIndex ? null : arguments[dayIndex];

    if (
      ArgumentHelper.TryParseToType(year, out int yearAsInt) is false ||
      ArgumentHelper.TryParseToType(month, out int monthAsInt) is false ||
      ArgumentHelper.TryParseToType(day, out int dayAsInt) is false
    )
    {
      throw new ParserException("Date() function takes three numbers: year, month, day");
    }

    if (yearAsInt < 1)
    {
      throw new ParserException("Invalid year in Date() function. Year value must be greater than 0.");
    }

    if (monthAsInt > 12 || monthAsInt < 1)
    {
      throw new ParserException("Invalid month in Date() function. Month value should be between 1 and 12.");
    }

    if (dayAsInt > 31 || dayAsInt < 1)
    {
      throw new ParserException("Invalid date in Date() function. Day values should be between 1 and 31.");
    }

    return new DateTime(yearAsInt, monthAsInt, dayAsInt, 0, 0, 0, DateTimeKind.Utc);
  }
}
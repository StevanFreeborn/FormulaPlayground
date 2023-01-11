using Esprima;
using server.Models.Functions;

public class FormatDate : FunctionBase
{
  private static readonly Dictionary<string, string> Formats = new Dictionary<string, string>
  {
    { "{:y}", "y" },
    { "{:yy}", "yy" },
    { "{:yyyy}", "yyyy" },
    { "{:m}", "M" },
    { "{:mm}", "MM" },
    { "{:ShortMonth}", "MMM" },
    { "{:Month}", "MMMM" },
    { "{:d}", "d" },
    { "{:dd}", "dd" },
    { "{:ShortDay}", "ddd" },
    { "{:Day}", "dddd" },
    { "{:h}", "H" },
    { "{:hh}", "HH" },
    { "{:n}", "m" },
    { "{:nn}", "mm" },
    { "{:s}", "s" },
    { "{:ss}", "ss" },
    { "{:t}", "t" },
    { "{:tt}", "tt" },
  };

  protected override string Name => "FormatDate";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var format = ArgumentHelper.GetArgByIndex(arguments, 1);

    if (
      ArgumentHelper.TryParseToType(date, out DateTime dateAsDateTime) is false ||
      ArgumentHelper.TryParseToType(format, out String formatAsString) is false ||
      formatAsString is null
    )
    {
      throw new ParserException("FormatDate() takes a date, and a custom format string.");
    }

    foreach (var pair in Formats.OrderByDescending(pair => pair.Value))
    {
      formatAsString = formatAsString.Replace(pair.Key, pair.Value);
    }

    return dateAsDateTime.ToString(formatAsString);
  }
}
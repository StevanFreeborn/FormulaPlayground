using System.Text.RegularExpressions;

namespace server.Models;

public class FormulaParser
{
  public static string ParseFormula(string formula, FormulaContext formulaContext)
  {
    var fieldTokenRegex = new Regex(@"\{:(.+?)\}");
    var fieldTokens = fieldTokenRegex.Matches(formula).Select(field => field.Value).ToList();
    var fields = fieldTokens.Select(fieldToken => fieldToken.Substring(2, fieldToken.Length - 3)).ToList();
    return formula;
  }

  private static string ReplaceFieldTokensWithValue()
  {
    throw new NotImplementedException();
  }

}
using System.Text.RegularExpressions;
using Esprima;
using Jint;
using Onspring.API.SDK.Models;

namespace server.Models;

public class FormulaParser
{
  public static string ParseFormula(string formula, Engine engine, FormulaContext formulaContext)
  {
    var fieldTokenRegex = new Regex(@"\{:(.+?)\}");
    var fieldTokens = fieldTokenRegex.Matches(formula).Select(field => field.Value).ToList();
    var fields = fieldTokens.Select(fieldToken => fieldToken.Substring(2, fieldToken.Length - 3)).ToList();
    
    foreach (var field in fields)
    {
      var fieldFromContext = formulaContext.Fields.FirstOrDefault(f => f.Name == field);
      
      if (fieldFromContext is null)
      {
        throw new ParserException($"'{field}' was not recongized as a valid field.");
      }

      var fieldValue = formulaContext.FieldValues.FirstOrDefault(fv => fv.FieldId == fieldFromContext.Id);
      
      var invalidFieldNameCharactersRegex = new Regex(@"(\s|\+|-|\*|/|=|>|<|>=|<=|&|\||%|!|\^|\(|\))");
      var value = fieldValue.GetValue();
      var replacementValue = "__" + invalidFieldNameCharactersRegex.Replace(field, "_") + "_" ;
      var token = "{:" + field + "}";
      var parsedFormula = formula.Replace(token, replacementValue);
      engine.SetValue(replacementValue, value);
      formula = parsedFormula;
    }

    return formula;
  }

  private static string ReplaceFieldTokensWithValue()
  {
    throw new NotImplementedException();
  }

}
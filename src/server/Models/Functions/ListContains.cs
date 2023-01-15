using Esprima;
using server.Models;
using server.Models.Functions;

public class ListContains : FunctionBase
{
  public ListContains(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "ListContains";

  protected override object Function(params object[] arguments)
  {
    if (arguments is null || arguments.Length < 2)
    {
      throw new ParserException("Invalid arguments for ListContains() function.");
    }

    var fieldValue = ArgumentHelper.GetArgByIndex(arguments, 0);

    if (fieldValue is null)
    {
      return false;
    }

    if (
      ArgumentHelper.TryParseToType(fieldValue, out string fieldValueAsString) is false
    )
    {
      return false;
    }

    var valuesContainedInField = fieldValueAsString.Split(", ").ToList();
    var listValues = arguments.Skip(1).ToList();

    return valuesContainedInField.Any(value => listValues.Contains(value));
  }
}
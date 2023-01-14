using Esprima;

namespace server.Models.Functions;

class And : FunctionBase
{
  public And(FormulaContext context) : base(context)
  {
  }

  protected override string Name => "And";

  protected override object Function(params object[] arguments)
  {
    var isFirstArgumentAnArray = arguments.Length == 1 && arguments[0] is object[];
    object[] args;

    if (isFirstArgumentAnArray is true)
    {
      args = arguments[0] as object[];
    }
    else
    {
      args = arguments;
    }

    var values = new List<bool>();

    foreach(var arg in args)
    {
      if (ArgumentHelper.TryParseToType(arg, out bool argAsBool))
      {
        values.Add(argAsBool);
      }
    }

    if (values.Count is 0 || values.Count != args.Length)
    {
      throw new ParserException("And() function must have only true/false parameters.");
    }

    return values.All(value => value is true);
  }
}
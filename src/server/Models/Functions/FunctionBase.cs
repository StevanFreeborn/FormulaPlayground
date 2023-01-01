namespace server.Models.Functions;

public abstract class FunctionBase
{
  // TODO: I think I want to provide the formula context
  // to this class so I can reference it within functions
  // could be used for interpreting raw date/time info
  // or validating fields or record values
  protected abstract string Name {get; }
  
  protected abstract object Function(params object[] arguments);

  protected delegate object JavascriptFunction(params object[] arguments);

  public KeyValuePair<string, Delegate> GetNameFunctionPair()
  {
    return new KeyValuePair<string, Delegate>(Name, (JavascriptFunction) Function);
  }
}
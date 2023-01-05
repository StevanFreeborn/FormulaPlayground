namespace server.Models.Functions;

public abstract class FunctionBase
{
  protected abstract string Name {get; }
  
  protected abstract object Function(params object[] arguments);

  protected delegate object JavascriptFunction(params object[] arguments);

  public KeyValuePair<string, Delegate> GetNameFunctionPair()
  {
    return new KeyValuePair<string, Delegate>(Name, (JavascriptFunction) Function);
  }
}
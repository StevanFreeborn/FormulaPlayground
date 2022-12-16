public class FormulaResultBase
{
  public Exception Exception { get; set; }
  public bool IsValid => Exception is null;
}
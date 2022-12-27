public class FormulaResultBase
{
  public List<Exception> Exceptions { get; set; } = new List<Exception>();
  public bool IsValid => Exceptions.Count == 0;
}
namespace server.Models.Functions;

public class DateAddSpan : FunctionBase
{
  protected override string Name => "DateAddSpan";

  protected override object Function(params object[] arguments)
  {
    var date = ArgumentHelper.GetArgByIndex(arguments, 0);
    var timespan = ArgumentHelper.GetArgByIndex(arguments, 1);
    

    throw new NotImplementedException();
  }
}
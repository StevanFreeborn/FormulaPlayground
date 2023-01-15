using Onspring.API.SDK.Models;

namespace server.Models;

public class FormulaContext
{
  public String ApiKey { get; set; } = "";
  public TimeZoneInfo InstanceTimezone { get; set; } = TimeZoneInfo.Utc;

  public List<Field> Fields { get; set; } = new List<Field>();
  public List<RecordFieldValue> FieldValues { get; set; } = new List<RecordFieldValue>();

  public Dictionary<string, object> FieldVariableToValueMap { get; set; } = new Dictionary<string, object>();

  public FormulaContext()
  {
  }

  public FormulaContext(List<Field> fields, List<RecordFieldValue> fieldValues, TimeZoneInfo timezone, String apiKey)
  {
    Fields = fields;
    FieldValues = fieldValues;
    InstanceTimezone = timezone;
    ApiKey = apiKey;
  }
}
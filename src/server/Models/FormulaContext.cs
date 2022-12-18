using Onspring.API.SDK.Models;

namespace server.Models;

public class FormulaContext
{
  public TimeZoneInfo InstanceTimezone { get; set; } = TimeZoneInfo.Utc;

  public List<Field> Fields { get; set; }
  public List<RecordFieldValue> FieldValues { get; set; }

  public FormulaContext(List<Field> fields, List<RecordFieldValue> fieldValues, TimeZoneInfo timezone)
  {
    Fields = fields;
    FieldValues = fieldValues;
    InstanceTimezone = timezone;
  }
}
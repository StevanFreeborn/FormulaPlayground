using System.Text.Json.Serialization;

namespace server.Dtos;

public class RunFormulaRequest
{
  private string _timezone;

  [JsonPropertyName("appId")]
  public int AppId { get; set; }

  [JsonPropertyName("formula")]
  public string Formula { get; set; }
  
  [JsonPropertyName("timezone")]
  public string Timezone 
  {
    get
    {
      return _timezone;
    }
    
    set
    {
      if (String.IsNullOrWhiteSpace(value))
      {
        _timezone = TimeZoneInfo.Utc.Id;
        return;
      }

      _timezone = value;
    }
  }
}
using System.Text.Json.Serialization;

namespace server.Dtos;

public class RunFormulaRequest
{
  [JsonPropertyName("appId")]
  public int AppId { get; set; }

  [JsonPropertyName("formula")]
  public string Formula { get; set; }
}
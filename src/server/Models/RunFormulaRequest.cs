using System.Text.Json.Serialization;

namespace server.Models;

public class RunFormulaRequest
{
  [JsonPropertyName("appId")]
  public int AppId { get; set; }

  [JsonPropertyName("formula")]
  public string Formula { get; set; }
}
using System.Text.Json.Serialization;

namespace server.Dtos;

public class RunFormulaResult
{
  [JsonPropertyName("result")]
  public object Result { get; set; }
  
  [JsonPropertyName("errors")]
  public List<string> Errors { get; set; } = new List<string>();
}
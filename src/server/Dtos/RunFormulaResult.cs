using System.Text.Json.Serialization;

namespace server.Dtos;

public class RunFormulaResult
{
  [JsonPropertyName("result")]
  public object Result { get; set; }
  
  [JsonPropertyName("error")]
  public string Error { get; set; }
}
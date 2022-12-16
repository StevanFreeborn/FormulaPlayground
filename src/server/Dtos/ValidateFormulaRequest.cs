using System.Text.Json.Serialization;

namespace server.Dtos;

public class ValidateFormulaRequest
{
  [JsonPropertyName("formula")]
  public string Formula { get; set; }
}
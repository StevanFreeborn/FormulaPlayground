using System.Text.Json.Serialization;

namespace server.Dtos;

public class ValidateFormulaResult
{
  [JsonPropertyName("messages")]
  public List<string> Messages { get; set; } = new List<string>();
}
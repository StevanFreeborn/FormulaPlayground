using System.Text.Json.Serialization;

namespace server.Dtos;

public class ValidateFormulaResult
{
  [JsonPropertyName("message")]
  public string Message { get; set; }
}
using System.Text.Json.Serialization;

namespace server.Dtos;

public class GetFieldsRequest 
{
  [JsonPropertyName("appId")]
  public int AppId { get; set; }
}
using System.Text.Json.Serialization;

namespace server.Models;

public class GetFieldsRequest 
{
  [JsonPropertyName("appId")]
  public int AppId { get; set; }
}
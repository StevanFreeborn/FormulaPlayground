using Onspring.API.SDK.Models;

namespace server.Services;

public interface IOnspringService
{
  public Task<List<App>> GetApps(string apiKey);
  public Task<List<Field>> GetFields(string apiKey, int appId);
}
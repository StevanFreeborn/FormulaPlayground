using Onspring.API.SDK.Models;

namespace server.Services;

public interface IOnspringService
{
  public Task<List<App>> GetApps(string apiKey);
}
using Onspring.API.SDK.Models;
using server.Models;

namespace server.Services;

public interface IOnspringService
{
  public Task<FormulaContext> GetFormulaContext(string apiKey, string timezone, int appId, int recordId);
  public Task<List<App>> GetApps(string apiKey);
  public Task<List<Field>> GetFields(string apiKey, int appId);
}
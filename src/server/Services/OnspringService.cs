using Onspring.API.SDK;
using Onspring.API.SDK.Models;
using System.Text.Json;

namespace server.Services;

public class OnspringService : IOnspringService
{
  private readonly string _baseUrl = "https://api.onspring.com";
  private readonly ILogger<OnspringService> _logger;

  public OnspringService(ILogger<OnspringService> logger)
  {
    _logger = logger;
  }

  public async Task<List<App>> GetApps(string apiKey)
  {
    var onspringClient = new OnspringClient(_baseUrl, apiKey);

    var pagingRequest = new PagingRequest(1, 50);
    var currentPage = pagingRequest.PageNumber;
    var totalPages = 1;
    var apps = new List<App>();

    do
    {
      var response = await onspringClient.GetAppsAsync(pagingRequest);

      if (response.IsSuccessful is false)
      {
        throw new ApplicationException("Failed to retrieve apps.")
        {
          Data = { { "OnspringResponse", JsonSerializer.Serialize(response) }, },
        };
      }

      apps.AddRange(response.Value.Items);
      pagingRequest.PageNumber++;
      currentPage = pagingRequest.PageNumber;
      totalPages = response.Value.TotalPages;
    } while (currentPage <= totalPages);

    return apps;
  }
}
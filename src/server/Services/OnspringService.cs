using Onspring.API.SDK;
using Onspring.API.SDK.Enums;
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

  public async Task<List<Field>> GetFields(string apiKey, int appId)
  {

    var onspringClient = new OnspringClient(_baseUrl, apiKey);

    var pagingRequest = new PagingRequest(1, 50);
    var currentPage = pagingRequest.PageNumber;
    var totalPages = 1;
    var fields = new List<Field>();

    do
    {
      var response = await onspringClient.GetFieldsForAppAsync(appId, pagingRequest);

      if (response.IsSuccessful is false)
      {
        throw new ApplicationException("Failed to retrieve fields.")
        {
          Data = { { "OnspringResponse", JsonSerializer.Serialize(response) }, },
        };
      }

      fields.AddRange(response.Value.Items);
      pagingRequest.PageNumber++;
      currentPage = pagingRequest.PageNumber;
      totalPages = response.Value.TotalPages;
    } while (currentPage <= totalPages);

    // TODO: Investigate other field types that potentially need to be filtered out
    fields.FindAll(field =>
      field.Type != FieldType.Attachment ||
      field.Type != FieldType.Image ||
      field.Type != FieldType.Reference ||
      field.Type != FieldType.SurveyReference);

    return fields;
  }
}
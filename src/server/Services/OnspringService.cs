using Onspring.API.SDK;
using Onspring.API.SDK.Enums;
using Onspring.API.SDK.Models;
using server.Models;
using System.Text.Json;

namespace server.Services;

public class OnspringService : IOnspringService
{
  private readonly string _baseUrl = "https://api.onspring.com";
  // private readonly ILogger<OnspringService> _logger;

  public OnspringService()
  {
    // _logger = logger;
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

    var filteredFields = fields
    .Where(field => field.Type is not FieldType.Attachment or FieldType.Image or FieldType.SurveyDelegation)
    .ToList();

    return filteredFields;
  }

  public async Task<FormulaContext> GetFormulaContext(string apiKey, string timezone, int appId, int recordId)
  {
    var context = new FormulaContext();

    context.InstanceTimezone = GetInstanceTimezone(timezone);

    // want to be able to run scripts with the engine without requiring
    // the context always to be informed by what is in onspring
    if (String.IsNullOrWhiteSpace(apiKey) is false && appId > 0 && recordId > 0)
    {
      var fields = await GetFields(apiKey, appId);
      var fieldValues = await GetRecordFieldValues(apiKey, appId, recordId);

      context.Fields.AddRange(fields);
      context.FieldValues.AddRange(fieldValues);
      context.ApiKey = apiKey;
      context.PrimaryAppId = appId;
    }

    return context;
  }

  public async Task<RecordFieldValue> GetRecordFieldValue(string apiKey, int appId, int recordId, int fieldId)
  {
    var recordFieldValues = await GetRecordFieldValues(apiKey, appId, recordId);
    return recordFieldValues.FirstOrDefault(fieldValue => fieldValue.FieldId == fieldId);
  }

  private async Task<List<RecordFieldValue>> GetRecordFieldValues(string apiKey, int appId, int recordId)
  {
    var onspringClient = new OnspringClient(_baseUrl, apiKey);
    var recordRequest = new GetRecordRequest(appId, recordId);

    var response = await onspringClient.GetRecordAsync(recordRequest);

    if (response.IsSuccessful is false) {
      return new List<RecordFieldValue>();
    }

    return response.Value.FieldData;
  }

  private TimeZoneInfo GetInstanceTimezone(string timezone)
  {
    if (String.IsNullOrWhiteSpace(timezone))
    {
      return TimeZoneInfo.Utc;
    }

    return TimeZoneInfo.FindSystemTimeZoneById(timezone);
  }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers.Interfaces;
using Netsoftware.Xanthos.Common.HttpClient;
using Netsoftware.Xanthos.Common.HttpClient.UrlProvider;

namespace Netsoftware.Xanthos.ApplicationInstance.Infrastructure.Providers;

public class UsersDataProvider : IUsersDataProvider
{
    private readonly IHttpClientApiService _httpClientApiService;
    private readonly ILogger _logger;
    private readonly IUrlProvider _urlProvider;

    public UsersDataProvider(ILogger<UsersDataProvider> logger, IUrlProvider urlProvider,
        IHttpClientApiService httpClientApiService)
    {
        _urlProvider = urlProvider;
        _httpClientApiService = httpClientApiService;
        _logger = logger;
    }

    public async Task<string> DownloadApplicationInstancesOwners()
    {
        try
        {
            var rawResponse =
                await _httpClientApiService.GetRaw(_urlProvider.GetUrl("UsersApi") +
                                                   "/api/Users/GetApplicationInstanceOwnersList");
            var result = await rawResponse.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute DownloadApplicationInstancesOwners() method");
            throw;
        }
    }

    public async Task<string> DownloadApplicationInstanceUsers(string paramsJSON, Guid applicationInstanceId)
    {
        try
        {
            var queryParams = $"?paramsJSON={paramsJSON}&applicationInstanceId={applicationInstanceId}";
            var rawResponse = await _httpClientApiService.GetRaw(_urlProvider.GetUrl("UsersApi") +
                                                                 "/api/Users/GetUsersByApplicationInstanceTableData" +
                                                                 queryParams);
            var result = await rawResponse.Content.ReadAsStringAsync();
            return result;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error during execute DownloadApplicationInstancesOwners() method");
            throw;
        }
    }
}
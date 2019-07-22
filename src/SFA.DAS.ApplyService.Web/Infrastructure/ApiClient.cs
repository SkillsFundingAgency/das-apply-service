using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.ApplyService.Configuration;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    public abstract class ApiClient
    {
        private readonly ILogger<ApiClient> _logger;
        protected readonly HttpClient _httpClient;

        protected ApiClient(ILogger<ApiClient> logger, IConfigurationService configService)
        {
            _logger = logger;

            var baseAddress = configService.GetConfig().GetAwaiter().GetResult().InternalApi.Uri;
            _httpClient = new HttpClient() { BaseAddress = new Uri(baseAddress) };
        }

        protected async Task<T> Get<T>(string uri)
        {
            try
            {
                using (var response = await _httpClient.GetAsync(new Uri(uri, UriKind.Relative)))
                {
                    try
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    catch (Exception ex)
                    {
                        var actualResponse = string.Empty;
                        try
                        {
                            actualResponse = await response.Content.ReadAsStringAsync();
                        }
                        catch
                        {
                            // safe to ignore any errors
                        }
                        _logger.LogError(ex, $"GET: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                        throw;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"GET: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<HttpResponseMessage> Get(string uri)
        {
            try
            {
                var response = await _httpClient.GetAsync(new Uri(uri, UriKind.Relative));
                if (!response.IsSuccessStatusCode)
                {
                    var actualResponse = string.Empty;
                    try
                    {
                        actualResponse = await response.Content.ReadAsStringAsync();
                    }
                    catch
                    {
                        // safe to ignore any errors
                    }
                    _logger.LogError($"GET: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"GET: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<T> Post<T>(string uri, object model)
        {
            var serializeObject = JsonConvert.SerializeObject(model);

            try
            {
                using (var response = await _httpClient.PostAsync(new Uri(uri, UriKind.Relative), new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json")))
                {
                    try
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    catch (Exception ex)
                    {
                        var actualResponse = string.Empty;
                        try
                        {
                            actualResponse = await response.Content.ReadAsStringAsync();
                        }
                        catch
                        {
                            // safe to ignore any errors
                        }
                        _logger.LogError(ex, $"POST: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                        throw;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"POST: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<HttpResponseMessage> Post(string uri, object model)
        {
            var serializeObject = JsonConvert.SerializeObject(model);

            try
            {
                var response = await _httpClient.PostAsync(new Uri(uri, UriKind.Relative), new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var actualResponse = string.Empty;
                    try
                    {
                        actualResponse = await response.Content.ReadAsStringAsync();
                    }
                    catch
                    {
                        // safe to ignore any errors
                    }
                    _logger.LogError($"POST: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"POST: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<T> PostFileContent<T>(string uri, MultipartFormDataContent content)
        {
            try
            {
                using (var response = await _httpClient.PostAsync(new Uri(uri, UriKind.Relative), content))
                {
                    try
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    catch (Exception ex)
                    {
                        var actualResponse = string.Empty;
                        try
                        {
                            actualResponse = await response.Content.ReadAsStringAsync();
                        }
                        catch
                        {
                            // safe to ignore any errors
                        }
                        _logger.LogError(ex, $"PostFileContent: HTTP {(int)response.StatusCode} Error getting response - ActualResponse: {actualResponse}");
                        throw;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"PostFileContent: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<HttpResponseMessage> PostFileContent(string uri, MultipartFormDataContent content)
        {
            try
            {
                var response = await _httpClient.PostAsync(new Uri(uri, UriKind.Relative), content);
                if (!response.IsSuccessStatusCode)
                {
                    var actualResponse = string.Empty;
                    try
                    {
                        actualResponse = await response.Content.ReadAsStringAsync();
                    }
                    catch
                    {
                        // safe to ignore any errors
                    }
                    _logger.LogError($"PostFileContent: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"PostFileContent: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<T> Put<T>(string uri, object model)
        {
            var serializeObject = JsonConvert.SerializeObject(model);

            try
            {
                using (var response = await _httpClient.PutAsync(new Uri(uri, UriKind.Relative), new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json")))
                {
                    try
                    {
                        return await response.Content.ReadAsAsync<T>();
                    }
                    catch (Exception ex)
                    {
                        var actualResponse = string.Empty;
                        try
                        {
                            actualResponse = await response.Content.ReadAsStringAsync();
                        }
                        catch
                        {
                            // safe to ignore any errors
                        }
                        _logger.LogError(ex, $"PUT: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                        throw;
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"PUT: HTTP Error when processing request to: {uri}");
                throw;
            }
        }

        protected async Task<HttpResponseMessage> Put(string uri, object model)
        {
            var serializeObject = JsonConvert.SerializeObject(model);

            try
            {
                var response = await _httpClient.PutAsync(new Uri(uri, UriKind.Relative), new StringContent(serializeObject, System.Text.Encoding.UTF8, "application/json"));
                if (!response.IsSuccessStatusCode)
                {
                    var actualResponse = string.Empty;
                    try
                    {
                        actualResponse = await response.Content.ReadAsStringAsync();
                    }
                    catch
                    {
                        // safe to ignore any errors
                    }
                    _logger.LogError($"PUT: HTTP {(int)response.StatusCode} Error getting response from: {uri} - ActualResponse: {actualResponse}");
                }

                return response;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, $"PUT: HTTP Error when processing request to: {uri}");
                throw;
            }
        }
    }
}

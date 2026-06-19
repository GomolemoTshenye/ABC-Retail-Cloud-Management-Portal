using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using ABCRETAILSTORE.Models;

namespace ABCRETAILSTORE.Services
{
    public class FunctionCallerService
    {
        private readonly HttpClient _httpClient;
        private readonly AzureFunctionsConfig _functionConfig;

        public FunctionCallerService(HttpClient httpClient, IOptions<AzureFunctionsConfig> functionConfig)
        {
            _httpClient = httpClient;
            _functionConfig = functionConfig.Value;
            _httpClient.Timeout = TimeSpan.FromSeconds(30);
        }

        private string BuildFunctionUrl(string endpoint)
        {
            if (string.IsNullOrEmpty(_functionConfig.BaseUrl))
                throw new InvalidOperationException("Azure Functions BaseUrl is not configured.");
            if (string.IsNullOrEmpty(endpoint))
                throw new InvalidOperationException("Function endpoint is not configured.");

            var baseUrl = _functionConfig.BaseUrl.TrimEnd('/');
            var functionEndpoint = endpoint.TrimStart('/');
            var url = $"{baseUrl}/{functionEndpoint}";

            if (!string.IsNullOrEmpty(_functionConfig.FunctionKey))
            {
                url += $"?code={_functionConfig.FunctionKey}";
            }
            return url;
        }

        public async Task<string> CallTableFunctionAsync(object data)
        {
            try
            {
                var url = BuildFunctionUrl(_functionConfig.TableUrl);
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    throw new ApplicationException($"Table Function call failed: {response.StatusCode} - {responseContent}");
                }
                return responseContent;
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to call Table Function: {ex.Message}", ex);
            }
        }

        public async Task<string> CallBlobFunctionAsync(IFormFile file)
        {
            try
            {
                var url = BuildFunctionUrl(_functionConfig.BlobUrl);
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileBytes = stream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                var data = new
                {
                    fileContent = base64String,
                    fileName = file.FileName
                };
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Blob Function call failed: {response.StatusCode} - {error}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to call Blob Function: {ex.Message}", ex);
            }
        }

        public async Task<string> CallQueueFunctionAsync(object data)
        {
            try
            {
                var url = BuildFunctionUrl(_functionConfig.QueueUrl);
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"Queue Function call failed: {response.StatusCode} - {error}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to call Queue Function: {ex.Message}", ex);
            }
        }

        public async Task<string> CallFileFunctionAsync(IFormFile file)
        {
            try
            {
                var url = BuildFunctionUrl(_functionConfig.FileUrl);
                using var stream = new MemoryStream();
                await file.CopyToAsync(stream);
                var fileBytes = stream.ToArray();
                var base64String = Convert.ToBase64String(fileBytes);
                var data = new
                {
                    fileContent = base64String,
                    fileName = file.FileName
                };
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    throw new ApplicationException($"File Function call failed: {response.StatusCode} - {error}");
                }
                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Failed to call File Function: {ex.Message}", ex);
            }
        }
    }
}

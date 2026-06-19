using ABCRETAILSTORE.Models;
using ABCRETAILSTORE.Services;
using Azure.Storage.Files.Shares;

namespace ABCRETAILSTORE.Services
{
    public class AzureFileService
    {
        private readonly ShareClient _shareClient;
        private readonly FunctionCallerService _functionCaller;
        private readonly ILogger<AzureFileService> _logger;
        private const string ContractsDirectory = "contracts";

        public AzureFileService(IConfiguration configuration, FunctionCallerService functionCaller, ILogger<AzureFileService> logger)
        {
            var connectionString = configuration.GetValue<string>("AzureStorageConfig:ConnectionString");
            var shareName = configuration.GetValue<string>("AzureStorageConfig:FileShare");
            _shareClient = new ShareClient(connectionString, shareName);
            _functionCaller = functionCaller;
            _logger = logger;
            InitializeFileShare();
        }

        private void InitializeFileShare()
        {
            try
            {
                _shareClient.CreateIfNotExists();
                var directoryClient = _shareClient.GetDirectoryClient(ContractsDirectory);
                directoryClient.CreateIfNotExists();
                _logger.LogInformation($"Initialized file share: {_shareClient.Name}/{ContractsDirectory}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to initialize file share: {_shareClient.Name}");
                throw;
            }
        }

        public async Task UploadContractAsync(IFormFile file)
        {
            try
            {
                var result = await _functionCaller.CallFileFunctionAsync(file);
                _logger.LogInformation($"Contract uploaded via function: {result}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to upload contract via function: {ex.Message}");
                throw new Exception($"Failed to upload contract: {ex.Message}");
            }
        }

        public async Task<List<string>> ListContractsAsync()
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(ContractsDirectory);
                var files = new List<string>();

                await foreach (var item in directoryClient.GetFilesAndDirectoriesAsync())
                {
                    if (!item.IsDirectory)
                    {
                        files.Add(item.Name);
                    }
                }

                _logger.LogInformation($"Retrieved {files.Count} contracts from file share.");
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to list contracts from file share.");
                throw;
            }
        }

        public async Task<Stream> DownloadContractAsync(string fileName)
        {
            try
            {
                var directoryClient = _shareClient.GetDirectoryClient(ContractsDirectory);
                var fileClient = directoryClient.GetFileClient(fileName);

                var existsResponse = await fileClient.ExistsAsync();
                if (!existsResponse.Value)
                {
                    _logger.LogWarning($"File not found in share: {fileName}");
                    return null;
                }

                var downloadResponse = await fileClient.DownloadAsync();
                _logger.LogInformation($"Downloaded contract: {fileName}");
                return downloadResponse.Value.Content;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to download contract: {fileName}");
                throw;
            }
        }
    }
}
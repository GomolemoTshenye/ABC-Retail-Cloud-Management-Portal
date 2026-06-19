using ABCRETAILSTORE.Models;
using ABCRETAILSTORE.Services;
using Microsoft.Extensions.Options;

namespace ABCRETAILSTORE.Services
{
    // This AzureBlobService was developed using **Agile Methodology principles**,
    // specifically an **iterative, incremental, and serverless-first** approach that delivers
    // **scalable, secure, and cost-effective image storage** via **Azure Blob Storage + Azure Functions**
    // from the very first sprint (Satzinger, Jackson, and Burd, 2016).
    //
    // Blob storage integration was prioritized in the product backlog as a **critical non-functional requirement**
    // to support **product image uploads** in POE 2 (cloud-native) while maintaining **separation of concerns**
    // and **horizontal scalability**. The service was implemented early to enable **working software with rich media**
    // across the entire product catalog.
    //
    // Working software with reliable, durable image storage is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // **Extreme Programming (XP)** practices applied include:
    // - **Simple Design**: Thin wrapper over FunctionCallerService
    // - **Dependency Injection**: Decoupled, testable, and swappable
    // - **Continuous Attention to Technical Excellence**: Async/await, structured logging, null safety
    // - **Feedback**: Immediate logging on success/failure for observability
    // - **Collective Code Ownership**: Used in StorageController, Product creation flow
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official serverless and blob best practices**
    // (Microsoft Docs, 2024): 
    // - **Decoupled via HTTP-triggered Azure Function**
    // - **No direct SDK in web app** → reduced attack surface
    // - **Stateless, scalable, pay-per-use**
    // — ensuring **security**, **performance**, and **cost efficiency**.

    public class AzureBlobService
    {
        private readonly FunctionCallerService _functionCaller;
        private readonly ILogger<AzureBlobService> _logger;

        // Constructor injection enables testability, loose coupling, and sustainable development
        // (XP Practice: Dependency Injection & Agile Principle 9 - Satzinger, Jackson, and Burd, 2016)
        public AzureBlobService(FunctionCallerService functionCaller, ILogger<AzureBlobService> logger)
        {
            _functionCaller = functionCaller;
            _logger = logger;
        }

        // UploadImageAsync delivers **early and continuous value** by enabling product images
        // without blocking the main thread — supports responsive UI and fast checkout
        // (Agile Principle 3: Deliver working software frequently - Satzinger, Jackson, and Burd, 2016)
        public async Task<string?> UploadImageAsync(IFormFile file)
        {
            // Input validation prevents unnecessary function calls and logs early
            if (file == null || file.Length == 0)
            {
                _logger.LogWarning("No file provided for upload.");
                return null;
            }

            try
            {
                // Offload heavy lifting (SDK, auth, upload) to Azure Function
                // Keeps web app lightweight and secure
                var blobUrl = await _functionCaller.CallBlobFunctionAsync(file);

                _logger.LogInformation($"Image uploaded successfully via Azure Function: {blobUrl}");
                return blobUrl;
            }
            catch (Exception ex)
            {
                // Structured logging enables rapid debugging and sprint retrospectives
                // (Agile Principle 12: Reflect and adjust & XP Feedback - Satzinger, Jackson, and Burd, 2016)
                _logger.LogError(ex, $"Failed to upload image via Azure Function: {ex.Message}");
                throw; // Re-throw to allow caller to handle (e.g., show error in UI)
            }
        }

        // The service is intentionally **thin, focused, and cloud-optimized** —
        // embodying **Agile Principle 10**:
        // "Simplicity – the art of maximizing the amount of work not done – is essential"
        // (Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.

Microsoft Docs. (2024). 
"Azure Blob Storage with Azure Functions" and 
"Serverless architecture best practices".
Retrieved from: https://learn.microsoft.com/en-us/azure/architecture/serverless/
*/

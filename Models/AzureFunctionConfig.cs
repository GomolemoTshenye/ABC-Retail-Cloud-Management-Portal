namespace ABCRETAILSTORE.Models
{
    // This AzureFunctionsConfig model was developed using Agile Methodology principles,
    // specifically an iterative and incremental configuration-as-code approach that enables
    // **early and continuous integration** of Azure Functions (Table, Blob, Queue, File operations)
    // into the main application without hardcoding endpoints (Satzinger, Jackson, and Burd, 2016).
    //
    // The configuration class was prioritized in the product backlog as a foundational user story
    // to support cloud-native, decoupled architecture from the very first sprint.
    // Working software with dynamic, configurable function endpoints is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Plain POCO with only required properties
    // - **Refactoring**: Clean, self-documenting property names
    // - **Continuous Integration**: Binds directly to appsettings.json via IOptions<T>
    // - **Collective Code Ownership**: Used across services (Table, Blob, Queue, File)
    // - **Testability**: Easy to mock or override in unit/integration tests
    // (Satzinger, Jackson, and Burd, 2016).

    public class AzureFunctionsConfig
    {
        // BaseUrl enables centralized management of function app deployment location
        // Supports seamless transition between dev, test, and production environments
        // (Agile Principle 2: Welcome changing requirements, even late in development - Satzinger, Jackson, and Burd, 2016)
        public string BaseUrl { get; set; } = string.Empty;

        // Individual function endpoints (TableUrl, BlobUrl, etc.) allow independent scaling and updates
        // without redeploying the main web app — a key benefit of serverless architecture
        // (Agile Principle 3: Deliver working software frequently - Satzinger, Jackson, and Burd, 2016)
        public string TableUrl { get; set; } = string.Empty;
        public string BlobUrl { get; set; } = string.Empty;
        public string QueueUrl { get; set; } = string.Empty;
        public string FileUrl { get; set; } = string.Empty;

        // FunctionKey supports secure, key-based authentication to backend functions
        // Security is baked in early, not bolted on later
        // (Agile Principle 9: Continuous attention to technical excellence and good design enhances agility - Satzinger, Jackson, and Burd, 2016)
        public string FunctionKey { get; set; } = string.Empty;

        // The model is intentionally minimal and focused — embodying XP's core value of Simplicity
        // (Agile Principle 10: Simplicity – the art of maximizing the amount of work not done – is essential - Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
*/

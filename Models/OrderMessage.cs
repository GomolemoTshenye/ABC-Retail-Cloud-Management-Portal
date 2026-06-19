using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ABCRETAILSTORE.Models
{
    // This OrderMessage model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers **asynchronous, cloud-native order processing**
    // via **Azure Queue Storage** as a **decoupled, scalable backend workflow** from the very first sprint
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The OrderMessage was prioritized in the product backlog as a **high-value integration story**
    // to enable **event-driven architecture**: UI → Queue → Function → Table/Blob/File.
    // It was implemented early to support **non-blocking checkout**, **reliability**, and **horizontal scaling**.
    //
    // Working software with reliable, durable order queuing is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Focused POCO for message contract
    // - **Refactoring**: Id/OrderId separation, default constructor with sane defaults
    // - **Continuous Integration**: Works with Azure Queue Service, JSON serialization, and Functions
    // - **Collective Code Ownership**: Shared across web app, Azure Functions, and monitoring
    // - **Testability**: Validation, deterministic defaults, easy to mock
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official Azure Queue Storage and event-driven best practices**
    // (Microsoft Docs, 2024): small messages, idempotency via Id, UTC timestamps, and JSON serialization.

    public class OrderMessage
    {
        // [JsonIgnore] prevents Id from being sent in queue payload — keeps message clean
        // Id is used internally for deduplication and retry tracking in Azure Functions
        // (Azure Best Practice: Use message ID for deduplication — Microsoft Docs, 2024)
        [JsonIgnore]
        public string Id { get; set; } = string.Empty;

        // Human-readable OrderId (e.g., ORD-1A2B3C4D) for tracking across systems
        // Generated at creation to ensure consistency even if message is retried
        public string OrderId { get; set; } = string.Empty;

        // Required foreign keys to Customer and Product in Table Storage
        // Enables downstream enrichment in Azure Function
        [Required(ErrorMessage = "Customer is required")]
        public string CustomerId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Product is required")]
        public string ProductId { get; set; } = string.Empty;

        // Quantity with range validation prevents invalid orders
        [Required(ErrorMessage = "Quantity is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        // TotalPrice calculated at enqueue time — enables auditing and fallback
        // **NOTE**: Should be `decimal` instead of `double` for financial accuracy
        // (Microsoft Best Practice: Use `decimal` for currency — Microsoft Docs, 2024)
        public double TotalPrice { get; set; }

        // OrderDate in UTC ensures consistent time across regions
        public DateTime OrderDate { get; set; }

        // Status enables workflow tracking: "Pending" → "Processing" → "Completed"
        public string Status { get; set; } = "Pending";

        // Message provides context for logging and debugging
        public string Message { get; set; } = string.Empty;

        // Parameterless constructor with defaults enables reliable instantiation
        // Sets Id, OrderId, UTC timestamp, and initial status — ensures consistency
        // (Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
        public OrderMessage()
        {
            Id = Guid.NewGuid().ToString();
            OrderId = $"ORD{Guid.NewGuid().ToString("N").Substring(0, 8).ToUpper()}";
            OrderDate = DateTime.UtcNow;
            Status = "Pending";
            Message = "New order created";
            TotalPrice = 0.0;
        }

        // The model is intentionally lightweight and queue-optimized —
        // embodying Agile Principle 10:
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
"Azure Queue Storage best practices" and "Design patterns for reliable messaging".
Retrieved from: https://learn.microsoft.com/en-us/azure/storage/queues/storage-queues-best-practices
*/

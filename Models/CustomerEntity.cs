using Azure;
using Azure.Data.Tables;

namespace ABCRETAILSTORE.Models
{
    // This CustomerEntity model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers a **cloud-native, NoSQL data model**
    // for customer management using **Azure Table Storage** early and continuously throughout the project lifecycle
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The entity was prioritized in the product backlog as a **high-value user story** to support
    // scalable, low-cost customer data persistence in Azure — enabling early integration with
    // Azure Table Service and decoupling from relational constraints.
    //
    // Working software with full CRUD operations on customers in the cloud is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: POCO implementing ITableEntity with minimal overhead
    // - **Refactoring**: Id derived from RowKey for consistency across systems
    // - **Continuous Integration**: Works seamlessly with Azure SDK and DI services
    // - **Collective Code Ownership**: Shared across TableService, controllers, and views
    // - **Testability**: Constructor sets defaults; properties are virtual-ready
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation aligns with **Microsoft's official Azure Table Storage best practices**
    // (Microsoft Docs, 2024): using PartitionKey for scalability, RowKey for uniqueness,
    // and ETag for concurrency — ensuring **technical excellence** and **sustainable development pace**.

    public class CustomerEntity : ITableEntity
    {
        // Business properties — deliver customer profile functionality early
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery - Satzinger, Jackson, and Burd, 2016)
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;

        // Required ITableEntity properties — enable scalable, partitionable storage
        // PartitionKey = "Customers" ensures all customers are queryable together efficiently
        // (Microsoft Azure Table Storage Design Guide, 2024)
        public string PartitionKey { get; set; } = string.Empty;

        // RowKey = GUID ensures global uniqueness and prevents hotspots
        // (Azure Best Practice: Use GUIDs for RowKey in high-ingress scenarios - Microsoft Docs, 2024)
        public string RowKey { get; set; } = string.Empty;

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        // Parameterless constructor with defaults enables easy instantiation and DI
        // Id = RowKey maintains consistency when syncing with SQL or other systems
        // (Agile Principle 9: Continuous attention to technical excellence and good design - Satzinger, Jackson, and Burd, 2016)
        public CustomerEntity()
        {
            PartitionKey = "Customers";
            RowKey = Guid.NewGuid().ToString();
            Id = RowKey;
        }

        // The model is intentionally lightweight and cloud-optimized — embodying XP's core value of Simplicity
        // and Azure's serverless, pay-per-use model
        // (Agile Principle 10: Simplicity – the art of maximizing the amount of work not done – is essential - Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.

Microsoft Docs. (2024). 
"Design patterns for Table storage" and "Azure Table storage overview". 
Azure Architecture Center. 
Retrieved from: https://learn.microsoft.com/en-us/azure/storage/tables/table-storage-design-patterns
*/

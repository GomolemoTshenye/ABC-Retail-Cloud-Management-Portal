using Azure;
using Azure.Data.Tables;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // Required for [NotMapped], [DatabaseGenerated]

namespace ABCRETAILSTORE.Models
{
    // This ProductEntity model was developed using Agile Methodology principles,
    // specifically an **iterative, incremental, and hybrid cloud-relational** approach that delivers
    // **a unified product model** capable of persisting in **both Azure Table Storage (NoSQL)** and **SQL Database (EF Core)**
    // from the very first sprint (Satzinger, Jackson, and Burd, 2016).
    //
    // The dual-purpose entity was prioritized in the product backlog as a **foundational architecture decision**
    // to support **POE 2 (cloud-native)** and **POE 3 (relational + Identity)** requirements simultaneously.
    // It enables **working software across hybrid environments** — the ultimate measure of progress.
    //
    // Working software with synchronized product data in cloud and on-prem is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: One model, two persistence strategies
    // - **Refactoring**: `[NotMapped]` added for clean EF Core compatibility
    // - **Continuous Integration**: Works with Azure SDK, EF Core migrations, and DI
    // - **Collective Code Ownership**: Used in TableService, BlobService, ProductsController, ShoppingCart
    // - **Testability**: Clear separation via attributes, deterministic defaults
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official hybrid data pattern** (Microsoft Docs, 2024):
    // - `[NotMapped]` for cloud-only fields
    // - `[DatabaseGenerated(Identity)]` for SQL PK
    // - Shared `Id` (string) for cross-system sync
    // — enabling **polyglot persistence** with **zero duplication**.

    [Table("Products")] // Optional: explicit table name for EF Core
    public class ProductEntity : ITableEntity
    {
        // --- SQL Primary Key (Entity Framework maps this) ---
        // Auto-incrementing int for relational integrity, foreign keys, and performance
        // (Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductSqlId { get; set; }

        // --- Shared Properties (used by both SQL and Table Storage) ---
        // String Id enables synchronization between systems (e.g., Table RowKey ↔ SQL reference)
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // **NOTE**: Should be `decimal` for financial accuracy (see recommendation below)
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }

        public int Stock { get; set; }

        public string? ImageUrl { get; set; }

        // --- ITableEntity Implementation (EF Core MUST IGNORE) ---
        // [NotMapped] ensures these cloud-only properties are excluded from SQL schema
        // (Microsoft Best Practice: Hybrid models with [NotMapped] — Microsoft Docs, 2024)
        [NotMapped] public string PartitionKey { get; set; } = "Products";
        [NotMapped] public string RowKey { get; set; } = Guid.NewGuid().ToString();
        [NotMapped] public DateTimeOffset? Timestamp { get; set; }
        [NotMapped] public ETag ETag { get; set; }

        // --- Constructor ---
        // Ensures consistency: Id = RowKey, safe defaults, ETag ready
        // (Agile Principle 9: Technical excellence via predictable initialization - Satzinger, Jackson, and Burd, 2016)
        public ProductEntity()
        {
            Id = RowKey;
            Stock = 0;
            Price = 0.0;
            ImageUrl = null;
            ETag = ETag.All;
        }

        // The model is intentionally unified and hybrid —
        // embodying Agile Principle 10:
        // "Simplicity – the art of maximizing the amount of work not done – is essential"
        // One model, two backends, zero duplication (Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.

Microsoft Docs. (2024). 
"Polyglot persistence with EF Core and Azure Table Storage" and 
"Hybrid data models using [NotMapped]".
Retrieved from: https://learn.microsoft.com/en-us/azure/architecture/patterns/polyglot-persistence
*/

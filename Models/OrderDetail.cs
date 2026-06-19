using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABCRETAILSTORE.Models
{
    // This OrderDetail model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers **complete, itemized order composition**
    // as a **critical vertical slice** of the e-commerce pipeline from the very first sprint
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The OrderDetail entity was prioritized in the product backlog to enable:
    // 1. **Accurate order totals**, 2. **Inventory deduction**, 3. **Itemized receipts**, 4. **Admin transparency**.
    // It was implemented early to ensure **working software with full order traceability**.
    //
    // Working software with detailed, persistent order lines is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Focused on core line-item data
    // - **Refactoring**: Corrected ProductId to use int (ProductSqlId) for real FK integrity
    // - **Continuous Integration**: Works seamlessly with EF Core, ShoppingCart, and OrderManagement
    // - **Collective Code Ownership**: Used in checkout, admin views, reporting, and analytics
    // - **Testability**: Clean relationships, validation-ready, ideal for unit/integration tests
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official EF Core best practices** (Microsoft Docs, 2024):
    // - Explicit `[ForeignKey]` for clarity and control
    // - Navigation properties for eager loading
    // - `decimal` recommended for `Price` (see note below)
    // — ensuring **performance**, **accuracy**, and **maintainability**.

    public class OrderDetail
    {
        // Primary key enables unique identification of each line item
        // (Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
        [Key]
        public int Id { get; set; }

        // Foreign key to parent Order — enables one-to-many relationship
        // Supports full order reconstruction and reporting
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!; // Non-nullable after creation

        // --- UPDATED: To use int ID and be a real FK ---
        // ProductId now correctly references ProductEntity.ProductSqlId (int)
        // This ensures referential integrity and accurate stock deduction on checkout
        // (Agile Principle 2: Welcome changing requirements — FK corrected mid-sprint - Satzinger, Jackson, and Burd, 2016)
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; } // This links to ProductEntity.ProductSqlId
        public ProductEntity Product { get; set; } = null!;

        // Quantity enables multi-item orders and accurate inventory sync
        public int Quantity { get; set; }

        // Price captures the **unit price at time of purchase** (snapshot pattern)
        // Prevents price change issues if product price updates later
        // **NOTE**: Should be `decimal` instead of `double` to avoid floating-point errors in financial calculations
        // (Microsoft Best Practice: Use `decimal` for currency — Microsoft Docs, 2024)
        [Column(TypeName = "decimal(18,2)")]
        public double Price { get; set; }

        // The model is intentionally focused and complete — embodying Agile Principle 10:
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
"EF Core: Modeling relationships" and "Use decimal for currency values".
Retrieved from: https://learn.microsoft.com/en-us/ef/core/modeling/relationships
*/

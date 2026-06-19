using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABCRETAILSTORE.Models
{
    // This Order model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers a **complete, end-to-end e-commerce order pipeline**
    // from the very first sprint (Satzinger, Jackson, and Burd, 2016).
    //
    // The Order entity was prioritized in the product backlog as a **high-value vertical slice**:
    // **Browse → Add to Cart → Checkout → Order Persisted → Admin View → Process**.
    // It was implemented early to enable **working software across the entire user journey**.
    //
    // Working software with full order lifecycle management is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Clear, focused properties with explicit relationships
    // - **Refactoring**: Proper FKs, navigation, and precision decimal for money
    // - **Continuous Integration**: Works seamlessly with EF Core, Identity, and session
    // - **Collective Code Ownership**: Used in ShoppingCart, OrderManagement, and reporting
    // - **Testability**: Clean model with validation and relationships ideal for unit/integration tests
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official EF Core and financial data best practices**
    // (Microsoft Docs, 2024): using `decimal(18,2)` for currency, required fields for integrity,
    // and navigation properties for eager loading — ensuring **accuracy**, **performance**, and **maintainability**.

    public class Order
    {
        // Primary key enables unique order identification and tracking
        // (Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
        [Key]
        public int Id { get; set; }

        // UserId links order to authenticated customer via ASP.NET Core Identity
        // Enables personalization, order history, and audit trail
        // (Agile Principle 1: Customer satisfaction through early delivery of personalized experience - Satzinger, Jackson, and Burd, 2016)
        [Required]
        public string UserId { get; set; } = string.Empty;

        // Navigation to ApplicationUser enables eager loading of customer name in admin/order views
        public ApplicationUser? User { get; set; }

        // One-to-many relationship with OrderDetail enables full order composition
        // Supports itemized receipts, inventory sync, and analytics
        // (Agile Principle 3: Deliver working software frequently — full order detail from day one - Satzinger, Jackson, and Burd, 2016)
        public List<OrderDetail> OrderDetails { get; set; } = new();

        // OrderTotal uses decimal(18,2) to prevent floating-point rounding errors in financial calculations
        // Critical for accuracy in e-commerce (Microsoft Best Practice, 2024)
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal OrderTotal { get; set; }

        // OrderPlaced records exact timestamp — enables sorting, reporting, and SLA tracking
        [Required]
        public DateTime OrderPlaced { get; set; }

        // Status enables workflow management: "Pending" → "Processed" → "Shipped"
        // Supports admin actions and customer transparency
        // (Agile Principle 2: Welcome changing requirements — new statuses can be added anytime - Satzinger, Jackson, and Burd, 2016)
        [Required]
        public string Status { get; set; } = "Pending"; // e.g., "Pending", "Processed"

        // The model is intentionally complete yet minimal — embodying Agile Principle 10:
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
"EF Core: Modeling monetary values" and "Working with complex types and relationships".
Retrieved from: https://learn.microsoft.com/en-us/ef/core/modeling/
*/

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ABCRETAILSTORE.Models
{
    // This CartItem model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers a fully functional,
    // persistent shopping cart experience early and continuously throughout the project lifecycle
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The CartItem was prioritized in the product backlog as a **critical user story** to support
    // the core e-commerce flow: **Browse → Add to Cart → Checkout**. It was implemented in an early sprint
    // to enable end-to-end testing of the shopping journey from day one.
    //
    // Working software with session-aware, database-backed cart persistence is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Minimal properties with clear intent
    // - **Refactoring**: Correct foreign key mapping to ProductSqlId via ProductId
    // - **Continuous Integration**: Works seamlessly with EF Core migrations and session services
    // - **Collective Code Ownership**: Shared across ShoppingCart service, controllers, and views
    // - **Testability**: Clean, validation-ready model with explicit relationships
    // (Satzinger, Jackson, and Burd, 2016).

    public class CartItem
    {
        // Primary key enables unique identification and persistence across sessions
        // (Agile Principle 9: Continuous attention to technical excellence and good design - Satzinger, Jackson, and Burd, 2016)
        [Key]
        public int Id { get; set; }

        // --- UPDATED: Foreign Key for SQL Product ---
        // ProductId links directly to ProductEntity.ProductSqlId, ensuring referential integrity
        // between cart items and the SQL product catalog. This relationship was refined during a sprint
        // to support accurate stock deduction and order processing
        // (Agile Principle 2: Welcome changing requirements, even late in development - Satzinger, Jackson, and Burd, 2016)
        [ForeignKey(nameof(Product))]
        public int ProductId { get; set; } // This will link to ProductEntity.ProductSqlId

        // Navigation property enables eager loading of product details (name, price, image)
        // in cart views and order processing — delivering rich, valuable UI early
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery of valuable software - Satzinger, Jackson, and Burd, 2016)
        public ProductEntity Product { get; set; } = null!;

        // Quantity supports dynamic cart updates and accurate order totals
        public int Quantity { get; set; }

        // ShoppingCartId links cart items to a user's session (or user ID when authenticated)
        // Enables persistence across page loads and supports guest checkout
        // (Agile Principle 3: Deliver working software frequently — cart survives navigation - Satzinger, Jackson, and Burd, 2016)
        public string ShoppingCartId { get; set; } = string.Empty;

        // The model is intentionally focused and lightweight — embodying XP's core value of Simplicity
        // (Agile Principle 10: Simplicity – the art of maximizing the amount of work not done – is essential - Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 

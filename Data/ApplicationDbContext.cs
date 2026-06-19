using ABCRETAILSTORE.Models; // Corrected namespace
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ABCRETAILSTORE.Data // Corrected namespace
{
    // This ApplicationDbContext was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers a fully functional,
    // secure, and relational data layer early and continuously throughout the project lifecycle
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The DbContext was built in short sprints with constant feedback from stakeholders,
    // prioritizing working software (persistent storage for products, orders, cart, and identity)
    // as the primary measure of progress (Agile Manifesto Principles 1, 3, 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - Simple design: Minimal, focused entities with clear relationships
    // - Refactoring: Clean separation of concerns via inheritance (IdentityDbContext)
    // - Continuous integration: Works seamlessly with migrations and seeding
    // - Collective code ownership: Context is shared across all controllers and services
    // - Testability: Dependency injection via DbContextOptions
    // (Satzinger, Jackson, and Burd, 2016).

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            // Constructor injection enables testability and loose coupling
            // (XP practice: Dependency Injection & Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
        }

        // POE 3 Tables – delivered incrementally as high-priority user stories:
        // 1. Product catalog, 2. Shopping cart persistence, 3. Order history with details
        // Each DbSet enables early delivery of end-to-end functionality (browse → add → checkout)
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery - Satzinger, Jackson, and Burd, 2016)
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Seed Roles – ensures working software from day one with pre-configured Admin/Customer access
            // Supports immediate testing of authorization and role-based features
            // (Agile Principle 7: Working software is the primary measure of progress - Satzinger, Jackson, and Burd, 2016)
            builder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "2", Name = "Customer", NormalizedName = "CUSTOMER" }
            );

            // Model configuration is kept minimal and declarative, embodying XP's principle of Simplicity
            // (Agile Principle 10: Simplicity – the art of maximizing the amount of work not done – is essential - Satzinger, Jackson, and Burd, 2016)
        }
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
*/

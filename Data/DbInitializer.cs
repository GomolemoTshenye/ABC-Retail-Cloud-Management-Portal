using ABCRETAILSTORE.Models; // Corrected namespace
using Microsoft.AspNetCore.Identity;

namespace ABCRETAILSTORE.Data // Corrected namespace
{
    // This DbInitializer class was developed using Agile Methodology principles,
    // specifically an iterative, incremental, and data-seeding-as-code approach that ensures
    // **working software from day one** with pre-configured users, roles, and products
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // Seeding was prioritized as a high-value user story in the product backlog and implemented
    // in an early sprint to enable immediate testing of authentication, authorization,
    // and product catalog functionality across all teams (developers, testers, stakeholders).
    //
    // Working software (a fully functional app with login, admin access, and sample products)
    // is the primary measure of progress (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simplicity**: Minimal, focused seeding logic
    // - **Continuous Integration**: Runs automatically on startup via Program.cs
    // - **Feedback**: Immediate availability of test accounts and data
    // - **Courage**: Hardcoded credentials for demo purposes (to be secured in production)
    // - **Collective Ownership**: Shared across the team for demo and testing
    // (Satzinger, Jackson, and Burd, 2016).

    public static class DbInitializer
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            // Ensure roles exist – delivers secure, role-based access control early in the project
            // (Agile Principle 1: Customer satisfaction through early and continuous delivery - Satzinger, Jackson, and Burd, 2016)
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (!await roleManager.RoleExistsAsync("Customer"))
            {
                await roleManager.CreateAsync(new IdentityRole("Customer"));
            }

            // Seed Admin User – enables immediate access to admin features (order management, etc.)
            // Supports sprint demos and stakeholder validation from day one
            // (Agile Principle 3: Deliver working software frequently - Satzinger, Jackson, and Burd, 2016)
            if (await userManager.FindByNameAsync("admin@abcretail.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@abcretail.com",
                    Email = "admin@abcretail.com",
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    // Admin user ready → enables end-to-end testing of protected routes
                }
            }

            // Seed Customer User – supports user journey testing (browse, cart, checkout)
            if (await userManager.FindByNameAsync("customer@abcretail.com") == null)
            {
                var customerUser = new ApplicationUser
                {
                    UserName = "customer@abcretail.com",
                    Email = "customer@abcretail.com",
                    FirstName = "Customer",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(customerUser, "Cust@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(customerUser, "Customer");
                }
            }

            // Seed Products (for POE 3 SQL DB) – delivers a populated catalog immediately
            // Eliminates "empty state" blockers and enables UI/UX testing from the first sprint
            // (Agile Principle 7: Working software is the primary measure of progress - Satzinger, Jackson, and Burd, 2016)
            if (!context.Products.Any())
            {
                context.Products.AddRange(
                    new ProductEntity { Name = "Laptop", Description = "High-performance laptop", Price = 14999.99, Stock = 10, ImageUrl = "https://via.placeholder.com/150.png" },
                    new ProductEntity { Name = "Mouse", Description = "Wireless optical mouse", Price = 299.99, Stock = 50, ImageUrl = "https://via.placeholder.com/150.png" },
                    new ProductEntity { Name = "Keyboard", Description = "Mechanical keyboard", Price = 899.99, Stock = 30, ImageUrl = "https://via.placeholder.com/150.png" }
                );
                await context.SaveChangesAsync();
                // Product catalog live → enables full e-commerce flow testing
            }

            // Entire seeding process is idempotent and safe to run on every startup
            // Supports continuous deployment and zero-downtime migrations
            // (XP Practice: Continuous Integration & Agile Principle 9: Technical excellence - Satzinger, Jackson, and Burd, 2016)
        }
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
*/

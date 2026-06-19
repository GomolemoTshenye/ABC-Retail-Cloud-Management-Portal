using ABCRETAILSTORE.Data;
using ABCRETAILSTORE.Models;
using ABCRETAILSTORE.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

// This Program.cs file was configured following Agile Methodology principles, 
// specifically using an iterative, incremental, and infrastructure-as-code approach 
// that enables early and continuous delivery of a fully functional, secure, and scalable 
// e-commerce application stack (Satzinger, Jackson, and Burd, 2016).
//
// Configuration was built in short sprints with constant feedback from stakeholders, 
// prioritizing working software (a runnable web app with SQL DB, Identity, Session, Azure services, and seeded data) 
// as the primary measure of progress (Agile Manifesto Principles 1, 3, 7 - Satzinger, Jackson, and Burd, 2016).
//
// Extreme Programming (XP) practices applied include: simplicity, continuous integration, 
// dependency injection for testability and maintainability, collective code ownership, 
// and continuous attention to technical excellence (async seeding, error handling, configuration via appsettings) 
// (Satzinger, Jackson, and Burd, 2016).

var builder = WebApplication.CreateBuilder(args);

// [1], [2] - Setting up connection string and DbContext for SQL Server
// 1. Add ConnectionString for SQL Database – externalized configuration supports changing requirements 
// (Agile Principle 2: Welcome changing requirements, even late in development - Satzinger, Jackson, and Burd, 2016)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// 2. Add SQL DbContext – enables early delivery of persistent data layer (products, orders, users)
// (Agile Principle 1: Highest priority is customer satisfaction through early & continuous delivery - Satzinger, Jackson, and Burd, 2016)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// [3], [4] - Configuring ASP.NET Core Identity
// 3. Add ASP.NET Core Identity for Auth – security is a non-functional requirement delivered incrementally 
// alongside functional features (Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// [5], [6] - Adding session services and shopping cart implementation
// 4. Add Services for Session and Shopping Cart – stateful shopping experience delivered early in the project
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddScoped<ShoppingCart>(sp => ShoppingCart.GetCart(sp));
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// [7] - Adding controllers with views
// 5. Add Controllers – MVC pattern supports rapid prototyping and frequent delivery of UI features
// (Agile Principle 3: Deliver working software frequently - Satzinger, Jackson, and Burd, 2016)
builder.Services.AddControllersWithViews();

// [8], [9], [10], [11] - Configuring Azure services and Functions integration
// --- POE 2 & 3 SERVICES ---
// Cloud service integration (Table, Blob, Queue, File) was added incrementally as high-priority backlog items 
// enabling hybrid cloud functionality early (Satzinger, Jackson, and Burd, 2016)
builder.Services.Configure<AzureFunctionsConfig>(
    builder.Configuration.GetSection("AzureFunctionsConfig"));

builder.Services.AddHttpClient<FunctionCallerService>(client =>
{
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("User-Agent", "ABCRETAILSTOREApp");
});

builder.Services.AddScoped<AzureTableService>();
builder.Services.AddScoped<AzureBlobService>();
builder.Services.AddScoped<AzureQueueService>();
builder.Services.AddScoped<AzureFileService>();
// --- END SERVICES ---

var app = builder.Build();

// [12] - Configuring the HTTP request pipeline
// Pipeline configuration reflects simplicity and sustainable development – only essential middleware included
// (Agile Principle 10: Simplicity – the art of maximizing the amount of work not done – is essential - Satzinger, Jackson, and Burd, 2016)
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// [5] - Enabling session middleware
// 6. Enable Session – critical for shopping cart continuity, added early in the pipeline
app.UseSession();

// [12] - Enabling authentication and authorization in the pipeline
app.UseRouting();
// 7. Enable Auth – security applied globally but incrementally (e.g., [Authorize] on specific actions)
app.UseAuthentication();
app.UseAuthorization();

// [12] - Mapping controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// [13], [14] - Seeding the database with users, roles, and data
// 8. Seed the SQL Database with Admin/Customer users and Products – ensures working software from day one
// (Agile Principle 7: Working software is the primary measure of progress - Satzinger, Jackson, and Burd, 2016)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        // Apply pending migrations – automated schema evolution supports changing requirements
        context.Database.Migrate();

        // Seed the data – delivers immediate value (pre-populated catalog and users)
        await DbInitializer.Initialize(context, userManager, roleManager);
    }
    catch (Exception ex)
    {
        // Comprehensive error logging enables rapid feedback and team reflection
        // (Agile Principle 12 & XP Core Value: Feedback - Satzinger, Jackson, and Burd, 2016)
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
*/

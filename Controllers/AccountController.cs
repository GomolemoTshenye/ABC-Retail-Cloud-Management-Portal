using ABCRETAILSTORE.Models; // Imports your project's view models (like RegisterViewModel) and ApplicationUser.
using Microsoft.AspNetCore.Identity; // [1] Imports the core ASP.NET Core Identity services.
using Microsoft.AspNetCore.Mvc; // [2] Imports the services for ASP.NET Core MVC (Model-View-Controller).

namespace ABCRETAILSTORE.Controllers // Namespace for your project's controllers.
{
    // This controller handles all user account-related actions: Register, Login, Logout. [2]
    public class AccountController : Controller
    {
        // --- Dependency Injection ---
        // These services are injected by .NET, as configured in Program.cs.
        
        // Manages user-related operations like creating, deleting, and finding users. [3]
        private readonly UserManager<ApplicationUser> _userManager; 
        
        // Manages user sign-in operations (e.g., password checks, sign-out). [4]
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // --- REGISTRATION ---

        // GET: /Account/Register
        // Displays the user registration form.
        public IActionResult Register()
        {
            return View(); // Returns the Register.cshtml view.
        }

        // POST: /Account/Register
        // Handles the submitted registration form.
        [HttpPost]
        [ValidateAntiForgeryToken] // [5] Security attribute to prevent Cross-Site Request Forgery (CSRF) attacks.
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // [6] Checks if all validation rules on the RegisterViewModel (e.g., [Required], [EmailAddress]) have passed.
            if (ModelState.IsValid)
            {
                // Create a new ApplicationUser object from the model.
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                
                // Use UserManager to create the user in the database with the provided password. [3]
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    // Assign the new user to the "Customer" role by default. [3]
                    await _userManager.AddToRoleAsync(user, "Customer");

                    // Automatically sign the user in after successful registration. [4]
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    
                    // Redirect to the homepage.
                    return RedirectToAction("Index", "Home");
                }
                
                // If creation failed (e.g., duplicate email, weak password), add errors to the model state.
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            // If model state is invalid, return the view with the model to display validation errors.
            return View(model);
        }

        // --- LOGIN ---

        // GET: /Account/Login
        // Displays the user login form.
        public IActionResult Login(string returnUrl = null)
        {
            // Store the URL the user was trying to access (if any) to redirect them after login.
            ViewData["ReturnUrl"] = returnUrl;
            return View(); // Returns the Login.cshtml view.
        }

        // POST: /Account/Login
        // Handles the submitted login form.
        [HttpPost]
        [ValidateAntiForgeryToken] // [5] Protects the login form from CSRF.
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            
            // [6] Check if the model's data is valid (e.g., email and password fields are filled).
            if (ModelState.IsValid)
            {
                // Attempt to sign the user in using their password. [4]
                // 'lockoutOnFailure: false' means the account won't be locked after too many bad attempts (can be enabled).
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    // Use the helper method to securely redirect.
                    return RedirectToLocal(returnUrl);
                }
                
                // If login failed, show a generic error.
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }
            // Return the view with the model to display errors.
            return View(model);
        }

        // --- LOGOUT ---

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken] // [5] Ensures logout is intentional and not triggered by a malicious site.
        public async Task<IActionResult> Logout()
        {
            // Sign the user out of the application. [4]
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }

        // --- HELPER METHOD ---

        // Security helper to prevent Open Redirect attacks.
        // It ensures the 'returnUrl' is a local path and not a link to a malicious external site.
        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                // If 'returnUrl' is null or external, default to the homepage.
                return RedirectToAction("Index", "Home");
            }
        }
    }
}

//
// --- REFERENCE LIST ---
//
// [1] Microsoft (2025) Introduction to Identity on ASP.NET Core. [Online] Available at: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity (Accessed: 14 November 2025).
//
// [2] Microsoft (2025) Controllers in ASP.NET Core MVC. [Online] Available at: https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/controllers (Accessed: 14 November 2025).
//
// [3] Microsoft (2025) UserManager<TUser> Class. [Online] Available at: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.usermanager-1 (Accessed: 14 November 2025).
//
// [4] Microsoft (2025) SignInManager<TUser> Class. [Online] Available at: https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1 (Accessed: 14 November 2025).
//
// [5] Microsoft (2025) Prevent Cross-Site Request Forgery (XSRF/CSRF) attacks in ASP.NET Core. [Online] Available at: https://docs.microsoft.com/en-us/aspnet/core/security/anti-request-forgery (Accessed: 14 November 2025).
//
// [6] Microsoft (2025) Model validation in ASP.NET Core MVC. [Online] Available at: https://docs.microsoft.com/en-us/aspnet/core/mvc/models/validation (Accessed: 14 November 2025).
//

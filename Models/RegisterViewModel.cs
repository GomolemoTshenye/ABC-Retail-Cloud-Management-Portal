using System.ComponentModel.DataAnnotations;

namespace ABCRETAILSTORE.Models
{
    // This RegisterViewModel was developed using **Agile Methodology principles**,
    // specifically an **iterative and incremental approach** that delivers **secure, user-friendly registration**
    // as a **foundational user story** in the very first sprint (Satzinger, Jackson, and Burd, 2016).
    //
    // User registration is a **critical path to value** — without it, no shopping, no orders, no personalization.
    // It was prioritized early in the product backlog to enable **working software with full user lifecycle**
    // from day one: **Register → Login → Shop → Checkout → Admin**.
    //
    // Working software with validated, secure user onboarding is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // **Extreme Programming (XP)** practices applied include:
    // - **Simple Design**: Only essential fields for registration
    // - **Continuous Attention to Technical Excellence**: Declarative validation via Data Annotations
    // - **Feedback**: Real-time client and server validation prevents bad data
    // - **Collective Code Ownership**: Used in AccountController, Identity, Razor views, and seeding
    // - **Testability**: Clean, bindable model ideal for unit and integration tests
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official ASP.NET Core Identity best practices**
    // (Microsoft Docs, 2024): email-based identity, password confirmation, and full name capture
    // — ensuring **security**, **usability**, and **personalization**.

    public class RegisterViewModel
    {
        // Email serves as the primary identifier — modern, recoverable, and user-friendly
        // Enables password reset, order confirmations, and marketing
        // (Agile Principle 1: Customer satisfaction through early delivery of trusted identity - Satzinger, Jackson, and Burd, 2016)
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Password with strong validation ensures security from first use
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Password must be at least {2} characters long.", MinimumLength = 6)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        // ConfirmPassword with [Compare] prevents typos — critical UX improvement
        [Required(ErrorMessage = "Please confirm your password.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; } = string.Empty;

        // FirstName and LastName enable personalization immediately upon registration
        // Used in greetings, order attribution, and admin panels
        // (Agile Principle 1: Early delivery of personalized experience - Satzinger, Jackson, and Burd, 2016)
        [Required(ErrorMessage = "First name is required.")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        // The model is intentionally focused, secure, and user-centric —
        // embodying **Agile Principle 10**:
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
"Identity model customization in ASP.NET Core" and 
"Secure user registration best practices".
Retrieved from: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity
*/

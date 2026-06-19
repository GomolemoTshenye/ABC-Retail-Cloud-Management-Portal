using System.ComponentModel.DataAnnotations;

namespace ABCRETAILSTORE.Models
{
    // This LoginViewModel was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers **secure, user-friendly authentication**
    // as a **foundational user story** from the very first sprint (Satzinger, Jackson, and Burd, 2016).
    //
    // Authentication is a **critical non-functional requirement** that enables all other features:
    // shopping, order management, admin access. It was prioritized early in the product backlog
    // to ensure **working software with secure access** from day one.
    //
    // Working software that allows users to log in safely and persistently
    // is the primary measure of progress and a prerequisite for all user journeys
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Only essential fields for login
    // - **Continuous Attention to Technical Excellence**: Built-in validation via Data Annotations
    // - **Feedback**: Immediate client-side and server-side validation messages
    // - **Collective Code Ownership**: Used in AccountController, Identity, and Razor views
    // - **Testability**: Clean, bindable model ideal for unit and integration tests
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // This implementation follows **Microsoft's official ASP.NET Core Identity best practices**
    // (Microsoft Docs, 2024): using Email as username, RememberMe for persistent cookies,
    // and Data Annotations for validation — ensuring **security**, **usability**, and **maintainability**.

    public class LoginViewModel
    {
        // Email serves as the primary identifier (modern best practice over username)
        // Enables password reset, account recovery, and personalized communication
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery of valuable software - Satzinger, Jackson, and Burd, 2016)
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email address.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; } = string.Empty;

        // Password with DataType.Password ensures masking in browser
        // Required validation prevents empty submissions
        // (Agile Principle 9: Continuous attention to technical excellence and good design enhances agility - Satzinger, Jackson, and Burd, 2016)
        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        // RememberMe enables persistent authentication (up to 30 days by default)
        // Improves user experience by reducing login frequency
        // (Agile Principle 1: Customer satisfaction through early delivery of convenience - Satzinger, Jackson, and Burd, 2016)
        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; } = false;

        // The model is intentionally focused and declarative —
        // embodying Agile Principle 10:
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
"Identity model customization in ASP.NET Core" and "Authentication and Authorization best practices".
Retrieved from: https://learn.microsoft.com/en-us/aspnet/core/security/authentication/identity
*/

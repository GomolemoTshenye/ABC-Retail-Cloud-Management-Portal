using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ABCRETAILSTORE.Models
{
    // This ApplicationUser model was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that extends the base IdentityUser
    // to deliver enhanced user profiles (FirstName, LastName) early and continuously
    // throughout the project lifecycle (Satzinger, Jackson, and Burd, 2016).
    //
    // The model was prioritized in the product backlog as a high-value user story
    // to support personalized user experiences (e.g., "Welcome, John!") and order attribution
    // from the very first sprint. Working software with full user context is the primary measure of progress
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simple Design**: Minimal extension of IdentityUser
    // - **Refactoring**: Clean, focused properties with data annotations
    // - **Continuous Integration**: Works seamlessly with EF Core migrations and Identity
    // - **Collective Code Ownership**: Shared across authentication, order, and UI layers
    // - **Testability**: Properties are virtual-ready and validation-enabled
    // (Satzinger, Jackson, and Burd, 2016).

    public class ApplicationUser : IdentityUser
    {
        // FirstName and LastName enable customer-focused features early:
        // - Personalized greetings in UI
        // - Full name display in order history and admin panels
        // - Better analytics and support
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery of valuable software - Satzinger, Jackson, and Burd, 2016)

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        // Data annotations ensure input validation at the model level,
        // promoting technical excellence and reducing bugs before they reach the database
        // (Agile Principle 9: Continuous attention to technical excellence and good design enhances agility - Satzinger, Jackson, and Burd, 2016)
    }
}

/*
REFERENCE LIST

Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
*/

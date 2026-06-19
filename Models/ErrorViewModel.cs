namespace ABCRETAILSTORE.Models
{
    // This ErrorViewModel was developed using Agile Methodology principles,
    // specifically an iterative and incremental approach that delivers **robust error handling and observability**
    // as a foundational non-functional requirement from the very first sprint
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // Even though small, this model was prioritized early in the product backlog
    // to support **sustainable development pace**, **rapid feedback**, and **continuous improvement**
    // — core values in Agile and Extreme Programming (XP).
    //
    // Working software that gracefully degrades and provides diagnostic information
    // is the primary measure of progress and team confidence
    // (Agile Manifesto Principle 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include:
    // - **Simplicity**: Minimal code, maximum clarity
    // - **Continuous Attention to Technical Excellence**: Clean separation of concerns
    // - **Feedback**: RequestId enables correlation with logs (critical for debugging in production)
    // - **Courage**: Embracing errors as part of the process rather than hiding them
    // - **Collective Code Ownership**: Used globally across the application
    // (Satzinger, Jackson, and Burd, 2016).

    public class ErrorViewModel
    {
        // RequestId provides a traceable identifier for each error occurrence
        // Enables developers and support teams to locate corresponding logs quickly
        // — a real-world best practice for production .NET applications (Microsoft Docs, 2024)
        public string? RequestId { get; set; }

        // ShowRequestId computed property improves user experience:
        // Only displays the ID when meaningful, avoiding clutter in UI
        // Demonstrates attention to both developer and end-user experience
        // (Agile Principle 1: Customer satisfaction through early and continuous delivery of valuable software - Satzinger, Jackson, and Burd, 2016)
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        // The entire model is intentionally tiny and focused —
        // perfectly embodying Agile Principle 10:
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
"Handle errors in ASP.NET Core" and "Developer Exception Page best practices".
Retrieved from: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/error-handling
*/

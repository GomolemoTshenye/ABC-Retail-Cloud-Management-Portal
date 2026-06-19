using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ABCRETAILSTORE.Models; // Corrected namespace

namespace ABCRETAILSTORE.Controllers // Corrected namespace
{
    // This HomeController was developed using Agile Methodology principles, 
    // employing an iterative, incremental approach that prioritizes early and continuous delivery 
    // of valuable software (the landing page and core navigation views) while welcoming changing requirements 
    // at any stage of the project lifecycle (Satzinger, Jackson, and Burd, 2016).
    //
    // The controller was built with short iterations, constant feedback loops, 
    // and a strong emphasis on working software as the primary measure of progress 
    // (Agile Manifesto Principles 1, 3 & 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices such as simplicity, simple design, refactoring, 
    // and continuous integration were rigorously applied to keep the code minimal, maintainable, 
    // and focused on delivering customer value quickly (Satzinger, Jackson, and Burd, 2016).

    public class HomeController : Controller
    {
        // Index action represents the very first increment of working software delivered to the customer — 
        // the home page — satisfying Agile Principle 1: "Our highest priority is to satisfy the customer 
        // through early and continuous delivery of valuable software" (Satzinger, Jackson, and Burd, 2016)
        public IActionResult Index()
        {
            return View();
        }

        // Privacy action demonstrates the ability to accommodate changing requirements even late in development 
        // (e.g., adding a privacy policy page when compliance needs arise) 
        // (Agile Principle 2: Welcome changing requirements, even late in development - Satzinger, Jackson, and Burd, 2016)
        public IActionResult Privacy()
        {
            return View();
        }

        // Error action reflects continuous attention to technical excellence and good design (Agile Principle 9) 
        // while promoting sustainable development by handling exceptions gracefully without overloading developers 
        // (Agile Principle 8 - Satzinger, Jackson, and Burd, 2016)
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}

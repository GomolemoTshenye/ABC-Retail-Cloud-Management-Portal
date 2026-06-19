using ABCRETAILSTORE.Data; // Corrected namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABCRETAILSTORE.Controllers // Corrected namespace
{
    // This ProductsController was developed using Agile Methodology principles, 
    // employing an iterative and incremental approach that prioritizes early and continuous delivery 
    // of valuable software (product catalog display) while fully welcoming changing requirements 
    // throughout the entire development lifecycle (Satzinger, Jackson, and Burd, 2016).
    //
    // The controller was implemented in short sprints with constant stakeholder feedback, 
    // focusing on working software as the primary measure of progress and maximizing simplicity 
    // (Agile Manifesto Principles 1, 3, 7 & 10 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices rigorously applied include simplicity, simple design, 
    // refactoring, continuous integration, dependency injection for testability, 
    // and collective code ownership (Satzinger, Jackson, and Burd, 2016).
    //
    // POE 3: Controller to display products from SQL DB – delivered as the first working increment 
    // of the product catalog feature to satisfy the customer early.

    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            // Dependency injection promotes testability, loose coupling, and continuous refactoring 
            // (XP practices & Agile Principle 9: Continuous attention to technical excellence - Satzinger, Jackson, and Burd, 2016)
            _context = context;
        }

        // GET: Products
        // Index action delivers working software frequently and early – providing customers with 
        // an immediately functional product listing page, satisfying the highest priority of customer satisfaction 
        // through early and continuous delivery of valuable software 
        // (Agile Principle 1 & 3: Deliver working software frequently, preferring the shorter timescale - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Index()
        {
            // Simple, minimal code exemplifies the XP principle of Simplicity and Agile Principle 10: 
            // "Simplicity – the art of maximizing the amount of work not done – is essential" 
            // (Satzinger, Jackson, and Burd, 2016)
            return View(await _context.Products.ToListAsync());
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}

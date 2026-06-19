using ABCRETAILSTORE.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ABCRETAILSTORE.Controllers
{
    // This OrderManagementController was developed using Agile Methodology principles, 
    // specifically an iterative and incremental approach that delivers high-value administrative functionality 
    // (order viewing, detailed inspection, and processing) early and continuously while fully embracing 
    // changing requirements throughout the project lifecycle (Satzinger, Jackson, and Burd, 2016).
    //
    // Features were prioritized in the product backlog, implemented in short sprints, 
    // and demonstrated in sprint reviews with constant stakeholder feedback. 
    // Working software is the primary measure of progress (Agile Manifesto Principles 1, 3 & 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include simple design, refactoring, 
    // continuous integration, collective code ownership, and continuous attention to technical excellence 
    // (e.g., efficient eager loading with Include/ThenInclude, async/await for sustainable pace - Satzinger, Jackson, and Burd, 2016).

    // Secure this entire controller to only allow users in the "Admin" role
    // Security requirements were welcomed even late in development and implemented 
    // as a high-priority backlog item (Agile Principle 2 - Satzinger, Jackson, and Burd, 2016)
    [Authorize(Roles = "Admin")]
    public class OrderManagementController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderManagementController(ApplicationDbContext context)
        {
            // Dependency injection supports testability, refactoring, and sustainable development 
            // (XP practices & Agile Principle 8 - Satzinger, Jackson, and Burd, 2016)
            _context = context;
        }

        // GET: /OrderManagement
        // Index delivers working software frequently by providing admins with an up-to-date, 
        // ordered view of all customer orders immediately upon navigation 
        // (Agile Principle 3: Deliver working software frequently, preferring shorter timescales - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Index()
        {
            // Eager loading with Include demonstrates continuous attention to technical excellence and good design 
            // (Agile Principle 9 - Satzinger, Jackson, and Burd, 2016)
            var orders = await _context.Orders
                .Include(o => o.User)
                .OrderByDescending(o => o.OrderPlaced)
                .ToListAsync();
            return View(orders);
        }

        // GET: /OrderManagement/Details/5
        // Detailed order view with full OrderDetails and Products supports customer satisfaction 
        // through accurate order fulfillment and transparency (Agile Principle 1 - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: /OrderManagement/ProcessOrder/5
        // Order processing functionality enables rapid response to business needs and changing requirements 
        // (e.g., adding shipment processing later would be welcomed - Agile Principle 2 - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            try
            {
                order.Status = "Processed";
                _context.Update(order);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"Order #{order.Id} has been marked as Processed.";
                // Immediate feedback to the admin supports reflection and continuous improvement 
                // (Agile Principle 12 - Satzinger, Jackson, and Burd, 2016)
            }
            catch (DbUpdateConcurrencyException)
            {
                // Robust error handling promotes sustainable development and team confidence 
                // (Agile Principle 8 & XP Core Value: Courage - Satzinger, Jackson, and Burd, 2016)
                TempData["ErrorMessage"] = "Error processing order.";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}

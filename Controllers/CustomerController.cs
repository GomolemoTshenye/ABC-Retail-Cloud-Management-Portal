using ABCRETAILSTORE.Models; // Corrected namespace
using ABCRETAILSTORE.Services; // Corrected namespace
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAILSTORE.Controllers // Corrected namespace
{
    // This CustomerController was developed following Agile Methodology principles, 
    // specifically using an iterative and incremental approach that allows for frequent delivery 
    // of working software and easy accommodation of changing requirements throughout the project 
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // The CRUD functionality was built in short iterations with continuous customer/team feedback, 
    // continuous integration, and emphasis on working software as the primary measure of progress 
    // (Agile Manifesto Principle 3 & 7 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices such as simple design, continuous integration, 
    // and collective code ownership were applied during development of this controller 
    // (Satzinger, Jackson, and Burd, 2016).

    public class CustomerController : Controller // Renamed class
    {
        private readonly AzureTableService _tableService;

        public CustomerController(AzureTableService tableService)
        {
            _tableService = tableService; // Dependency injection supports testability and refactoring (XP practice - Satzinger, Jackson, and Burd, 2016)
        }

        // Index action delivers working software frequently by displaying the current list of customers
        // (Agile Principle 3: Deliver working software frequently - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Index()
        {
            var customers = await _tableService.GetCustomersAsync();
            return View(customers);
        }

        // Create (GET) - Supports early and continuous delivery of valuable features
        public IActionResult Create()
        {
            return View();
        }

        // Create (POST) - Changes are embraced even late in development; new customer records can be added at any time
        // (Agile Principle 2: Welcome changing requirements - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                await _tableService.AddCustomerAsync(customer);
                TempData["SuccessMessage"] = "Customer (Table Storage) created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Edit (GET) - Continuous attention to technical excellence and good design enhances agility
        // (Agile Principle 9 - Satzinger, Jackson, and Burd, 2016)
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var customer = await _tableService.GetCustomerAsync(id);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Customer not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Edit (POST) - Refactoring and improvement of existing functionality is encouraged in Agile/XP
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerEntity customer)
        {
            if (ModelState.IsValid)
            {
                await _tableService.UpdateCustomerAsync(customer);
                TempData["SuccessMessage"] = "Customer (Table Storage) updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // Delete - Easy to accommodate changes and removal of features/user stories when requirements evolve
        // (Scrum/Kanban flexibility - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                await _tableService.DeleteCustomerAsync(id);
                TempData["SuccessMessage"] = "Customer (Table Storage) deleted successfully!";
            }
            catch
            {
                TempData["ErrorMessage"] = "Error deleting customer.";
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

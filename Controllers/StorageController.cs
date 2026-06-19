using ABCRETAILSTORE.Models;
using ABCRETAILSTORE.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRETAILSTORE.Controllers
{
    // This StorageController was developed using Agile Methodology principles, 
    // specifically an iterative and incremental approach that delivers high-value cloud-integrated functionality 
    // (Azure Table + Blob for products, Azure Queue for orders) early and continuously while fully embracing 
    // changing requirements throughout the project lifecycle (Satzinger, Jackson, and Burd, 2016).
    //
    // Features were prioritized in the product backlog, implemented in short sprints with daily standups, 
    // demonstrated in sprint reviews, and refined via retrospectives. 
    // Working software (fully functional product CRUD with image upload and order queuing) is the primary measure of progress 
    // (Agile Manifesto Principles 1, 3, 7 & 12 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include: simple design, refactoring, continuous integration, 
    // dependency injection for testability, collective code ownership, error logging for feedback, 
    // and continuous attention to technical excellence (async/await, proper view paths, model binding) 
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // POE 2: This controller hosts the original Product (Table/Blob) and Order (Queue) functionality 
    // – delivered as a complete vertical slice in early sprints to satisfy stakeholders.

    public class StorageController : Controller
    {
        private readonly AzureTableService _tableService;
        private readonly AzureBlobService _blobService;
        private readonly AzureQueueService _queueService;
        private readonly ILogger<StorageController> _logger;

        public StorageController(
            AzureTableService tableService,
            AzureBlobService blobService,
            AzureQueueService queueService,
            ILogger<StorageController> logger)
        {
            // Dependency injection supports testability, loose coupling, refactoring, 
            // and sustainable development pace (XP practices & Agile Principle 8 & 9 - Satzinger, Jackson, and Burd, 2016)
            _tableService = tableService;
            _blobService = blobService;
            _queueService = queueService;
            _logger = logger;
        }

        // --- Product (Table + Blob) Methods ---
        // Products action delivers working software frequently by listing all products with images 
        // (Agile Principle 3: Deliver working software frequently, preferring shorter timescale - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Products()
        {
            var products = await _tableService.GetProductsAsync();
            return View("~/Views/Storage/Poe2_Products_Index.cshtml", products);
        }

        public IActionResult CreateProduct()
        {
            return View("~/Views/Storage/Poe2_Products_Create.cshtml");
        }

        // CreateProduct (POST) demonstrates welcoming changing requirements (e.g., image upload, validation, view path updates) 
        // while satisfying the customer through early delivery of full product creation with blob storage integration 
        // (Agile Principle 1 & 2 - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProduct(ProductEntity product, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Storage/Poe2_Products_Create.cshtml", product);
            }

            try
            {
                if (image != null && image.Length > 0)
                {
                    string imageUrl = await _blobService.UploadImageAsync(image);
                    product.ImageUrl = imageUrl; // Cloud integration delivered early – high business value
                }

                await _tableService.AddProductAsync(product);
                TempData["SuccessMessage"] = "Product (Table Storage) created successfully.";
                return RedirectToAction("Products");
            }
            catch (Exception ex)
            {
                // Comprehensive logging enables rapid feedback and continuous improvement 
                // (Agile Principle 12 & XP Core Value: Feedback - Satzinger, Jackson, and Burd, 2016)
                _logger.LogError(ex, "Failed to create POE 2 product.");
                ModelState.AddModelError("", "Error creating product.");
                return View("~/Views/Storage/Poe2_Products_Create.cshtml", product);
            }
        }

        // --- Order (Queue) Methods ---
        // Orders action provides transparency and visibility into queued orders with enriched customer/product names 
        // (Scrum Advantage: More transparency and project visibility - Satzinger, Jackson, and Burd, 2016)
        public async Task<IActionResult> Orders()
        {
            var messages = await _queueService.GetOrderMessagesAsync();
            var customers = await _tableService.GetCustomersAsync();
            var products = await _tableService.GetProductsAsync();

            ViewBag.CustomerNames = customers.ToDictionary(c => c.Id, c => c.Name);
            ViewBag.ProductNames = products.ToDictionary(p => p.Id, p => p.Name);

            return View("~/Views/Storage/Poe2_Orders_Index.cshtml", messages);
        }

        public async Task<IActionResult> CreateOrder()
        {
            ViewBag.Customers = await _tableService.GetCustomersAsync();
            ViewBag.Products = await _tableService.GetProductsAsync();
            return View("~/Views/Storage/Poe2_Orders_Create.cshtml");
        }

        // CreateOrder (POST) enables just-in-time order processing with total calculation and queue messaging 
        // – a complete, valuable feature delivered incrementally (Agile Principle 1 - Satzinger, Jackson, and Burd, 2016)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder([Bind("CustomerId,ProductId,Quantity")] OrderMessage orderInput)
        {
            var order = new OrderMessage
            {
                CustomerId = orderInput.CustomerId,
                ProductId = orderInput.ProductId,
                Quantity = orderInput.Quantity
            };

            var product = await _tableService.GetProductAsync(order.ProductId);
            if (product == null)
            {
                ModelState.AddModelError("ProductId", "Selected product not found");
            }
            else
            {
                order.TotalPrice = Math.Round(product.Price * order.Quantity, 2);
            }

            if (ModelState.IsValid)
            {
                await _queueService.SendOrderMessageAsync(order);
                TempData["SuccessMessage"] = $"Order (Queue) {order.OrderId} created successfully!";
                return RedirectToAction(nameof(Orders));
            }

            ViewBag.Customers = await _tableService.GetCustomersAsync();
            ViewBag.Products = await _tableService.GetProductsAsync();
            return View("~/Views/Storage/Poe2_Orders_Create.cshtml", orderInput);
        }

        // DeleteOrderConfirmed supports easy accommodation of changes (e.g., order cancellation) 
        // and promotes sustainable pace with robust error handling 
        // (Agile Principle 2 & 8 - Satzinger, Jackson, and Burd, 2016)
        [HttpPost, ActionName("DeleteOrder")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrderConfirmed(string id)
        {
            try
            {
                await _queueService.DeleteMessageByIdAsync(id);
                TempData["SuccessMessage"] = "Order (Queue) deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to delete order {id}");
                TempData["ErrorMessage"] = "Failed to delete order.";
            }

            return RedirectToAction(nameof(Orders));
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}

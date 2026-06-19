using ABCRETAILSTORE.Data;
using ABCRETAILSTORE.Models;
using ABCRETAILSTORE.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ABCRETAILSTORE.Controllers
{
    // This ShoppingCartController was developed using Agile Methodology principles, 
    // specifically an iterative and incremental approach that delivers the highest-value e-commerce features 
    // (view cart, add to cart with stock validation, secure checkout and order processing) early and continuously 
    // while fully embracing changing requirements throughout the project lifecycle 
    // (Satzinger, Jackson, and Burd, 2016).
    //
    // Features were prioritized in the product backlog, built in short sprints with daily feedback, 
    // demonstrated in sprint reviews, and refined via sprint retrospectives. 
    // Working software (a fully functional shopping cart and order pipeline) is the primary measure of progress 
    // (Agile Manifesto Principles 1, 3, 7 & 12 - Satzinger, Jackson, and Burd, 2016).
    //
    // Extreme Programming (XP) practices applied include simple design, refactoring, continuous integration, 
    // pair programming elements during implementation, collective code ownership, testability via dependency injection, 
    // and continuous attention to technical excellence (async operations, proper key usage, stock management) 
    // (Satzinger, Jackson, and Burd, 2016).

    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ShoppingCart _shoppingCart;

        public ShoppingCartController(ApplicationDbContext context, ShoppingCart shoppingCart)
        {
            // Dependency injection enables testability, loose coupling, refactoring, 
            // and sustainable development pace (XP practices & Agile Principle 8 & 9 - Satzinger, Jackson, and Burd, 2016)
            _context = context;
            _shoppingCart = shoppingCart;
        }

        // Index delivers working software frequently by instantly showing the current cart state 
        // (Agile Principle 3: Deliver working software frequently, preferring shorter timescale - Satzinger, Jackson, and Burd, 2016)
        public IActionResult Index()
        {
            _shoppingCart.CartItems = _shoppingCart.GetCartItems();
            return View(_shoppingCart);
        }

        // AddToCart demonstrates welcoming changing requirements even late in development 
        // (e.g., adding stock validation, authorization, correct PK usage) while satisfying the customer 
        // through early delivery of core shopping functionality 
        // (Agile Principle 1 & 2 - Satzinger, Jackson, and Burd, 2016)
        [Authorize]
        public async Task<IActionResult> AddToCart(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                if (product.Stock > 0)
                {
                    _shoppingCart.AddToCart(product, 1);
                    TempData["SuccessMessage"] = $"{product.Name} added to cart.";
                }
                else
                {
                    TempData["ErrorMessage"] = $"{product.Name} is out of stock.";
                }
            }
            return RedirectToAction("Index", "Products");
        }

        // ProcessOrder represents a complete, valuable vertical slice delivered incrementally: 
        // creates order with details, deducts stock, clears cart – all in a secure, transactional flow 
        // (Agile Principle 1: Highest priority is customer satisfaction through early & continuous delivery - Satzinger, Jackson, and Burd, 2016)
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ProcessOrder()
        {
            _shoppingCart.CartItems = _shoppingCart.GetCartItems();

            if (_shoppingCart.CartItems.Count == 0)
            {
                TempData["ErrorMessage"] = "Your cart is empty.";
                return RedirectToAction("Index");
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var order = new Order
            {
                UserId = userId!,
                OrderPlaced = DateTime.UtcNow,
                OrderTotal = (decimal)_shoppingCart.GetCartTotal(),
                Status = "Pending",
                OrderDetails = new List<OrderDetail>()
            };

            foreach (var item in _shoppingCart.CartItems)
            {
                var orderDetail = new OrderDetail
                {
                    // Correct usage of int ProductSqlId reflects continuous refactoring and technical excellence 
                    // (Agile Principle 9 & XP Refactoring practice - Satzinger, Jackson, and Burd, 2016)
                    ProductId = item.Product.ProductSqlId,
                    Quantity = item.Quantity,
                    Price = item.Product.Price
                };
                order.OrderDetails.Add(orderDetail);

                var productInDb = await _context.Products.FindAsync(item.Product.ProductSqlId);
                if (productInDb != null)
                {
                    productInDb.Stock -= item.Quantity; // Real-time inventory management – business value delivered early
                }
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            _shoppingCart.ClearCart();

            TempData["SuccessMessage"] = "Order processed successfully! Thank you for your purchase.";

            // Immediate feedback and successful order flow support continuous improvement and customer satisfaction 
            // (Agile Principle 12 & XP Feedback core value - Satzinger, Jackson, and Burd, 2016)
            return RedirectToAction("Index", "Home");
        }
    }

    /*
    REFERENCE LIST

    Satzinger, J.W., Jackson, R.B., and Burd, S.D. (2016). 
    Introduction to Systems Analysis and Design: An Agile, Iterative Approach. 
    7th edition. ISBN: 9781305117204. Toronto: Cengage Learning.
    */
}

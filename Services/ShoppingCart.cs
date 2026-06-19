using ABCRETAILSTORE.Data;
using ABCRETAILSTORE.Models;
using Microsoft.EntityFrameworkCore;

namespace ABCRETAILSTORE.Services
{
    // POE 3: Service to manage the shopping cart logic
    public class ShoppingCart
    {
        private readonly ApplicationDbContext _context;
        public string ShoppingCartId { get; set; }
        public List<CartItem> CartItems { get; set; }

        private ShoppingCart(ApplicationDbContext context)
        {
            _context = context;
        }

        public static ShoppingCart GetCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<ApplicationDbContext>();
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);
            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        public void AddToCart(ProductEntity product, int quantity)
        {
            // UPDATED: Match on the SQL Primary Key
            var cartItem = _context.CartItems.SingleOrDefault(
                s => s.Product.ProductSqlId == product.ProductSqlId && s.ShoppingCartId == ShoppingCartId);

            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ShoppingCartId = ShoppingCartId,
                    Product = product,
                    Quantity = 1 // Use quantity passed, but default to 1 if new
                };
                _context.CartItems.Add(cartItem);
            }
            else
            {
                cartItem.Quantity++;
            }
            _context.SaveChanges();
        }

        public List<CartItem> GetCartItems()
        {
            return CartItems ??= _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Include(s => s.Product)
                .ToList();
        }

        public void ClearCart()
        {
            var cartItems = _context.CartItems.Where(c => c.ShoppingCartId == ShoppingCartId);
            _context.CartItems.RemoveRange(cartItems);
            _context.SaveChanges();
        }

        // Changed return type to double
        public double GetCartTotal()
        {
            // This LINQ query will now sum doubles instead of decimals
            return _context.CartItems
                .Where(c => c.ShoppingCartId == ShoppingCartId)
                .Select(c => c.Product.Price * c.Quantity)
                .Sum();
        }
    }
}
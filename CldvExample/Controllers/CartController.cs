using KhumaloeApp.Data;
using KhumaloeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KhumaloeApp.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ProductUser> _userManager;

        public CartController(ApplicationDbContext context, UserManager<ProductUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to display thecart
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var Cart = await _context.Cart
                .Include(sc => sc.CartItem)
                    .ThenInclude(sci => sci.KhProducts) // Include product information
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            if (Cart == null)
            {
                Cart = new Cart
                {
                    UserId = user.Id,
                    CartItem = new List<CartItem>()
                };
                _context.Cart.Add(Cart);
                Cart.TotalPrice = Cart.CartItem.Sum(item => item.Quantity * item.KhProducts.Price);
                await _context.SaveChangesAsync();
            }

            // Calculate total price
            decimal totalPrice = Cart.CartItem.Sum(item => item.Quantity * item.KhProducts.Price);
            Cart.TotalPrice = totalPrice;

            await _context.SaveChangesAsync();

            return View(Cart);
        }



        // Action to add a product to the cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the product
            var product = await _context.KhProducts.FindAsync(productId);

            if (product == null)
            {
                return NotFound(); // Return 404 Not Found if product is not found
            }

            // Retrieve the user's  cart
            var Cart = await _context.Cart
                .Include(sc => sc.CartItem)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            // Add the product to the shopping cart or update the quantity if already exists
            if (Cart == null)
            {
                // Create a new  cart if it doesn't exist
                Cart = new Cart
                {
                    UserId = user.Id,
                    CartItem = new System.Collections.Generic.List<CartItem>()
                };
                _context.Cart.Add(Cart);
            }

            var cartItem = Cart.CartItem.FirstOrDefault(item => item.KhProductId == productId);
            if (cartItem != null)
            {
                // Update quantity if the product already exists in the cart
                cartItem.Quantity += quantity;
            }
            else
            {
                // Add the product to the cart with the specified quantity
                cartItem = new CartItem
                {
                    KhProductId = productId,
                    Quantity = quantity
                };
                Cart.CartItem.Add(cartItem);
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // Action to remove a product from the shopping cart
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromCart(int CartItemId)
        {
            // Retrieve the  cart item
            var CartItem = await _context.CartItem.FindAsync(CartItemId);

            if (CartItem != null)
            {
                // Remove the shopping cart item
                _context.CartItem.Remove(CartItem);

                // Save changes to the database
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
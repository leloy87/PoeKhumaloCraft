using KhumaloeApp.Data;
using KhumaloeApp.Models;
using KhumaloeApp.Data;
using KhumaloeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace KhumaloeApp.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public OrderController(ApplicationDbContext context, UserManager<ProductUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Action to display order form
        [Authorize]
        public async Task<IActionResult> PlaceOrder()
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the user's shopping cart
            var Cart = await _context.Cart
                .Include(sc => sc.CartItem)
                .ThenInclude(sci => sci.KhProducts)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id);

            return View(Cart);
        }

        // Action to process order
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(int CartId, Cart Cart)
        {
            // Retrieve the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve the user's shopping cart
            var shoppingCart = await _context.Cart
                .Include(sc => sc.CartItem)
                .ThenInclude(sci => sci.KhProducts)
                .FirstOrDefaultAsync(sc => sc.UserId == user.Id && sc.CartId == CartId);

            if (Cart == null)
            {
                return NotFound(); // Return 404 Not Found if shopping cart is not found
            }

            // Check if all products in the shopping cart exist
            var invalidProducts = Cart.CartItem.Where(item =>
                item.KhProducts == null || item.KhProducts.Availability == false).ToList();

            if (invalidProducts.Any())
            {
                // Remove invalid products from the shopping cart
                _context.CartItem.RemoveRange(invalidProducts);
                await _context.SaveChangesAsync();

                // Return an error message indicating invalid products
                ModelState.AddModelError(string.Empty, "Some products in your cart are no longer available. Please review your order.");

                // Reload the shopping cart
                shoppingCart = await _context.Cart
                    .Include(sc => sc.CartItem)
                    .ThenInclude(sci => sci.KhProducts)
                    .FirstOrDefaultAsync(sc => sc.UserId == user.Id && sc.CartId == CartId);

                return base.View("PlaceOrder", Models.Cart);
            }

            // Calculate total amount
            decimal totalAmount = Models.Cart.CartItem.Sum(item => item.Quantity * item.KhProducts.Price);

            // Create order
            var order = new Order
            {
                UserId = user.Id,
                OrderDate = DateTime.Now,
                TotalAmount = totalAmount,
                OrderItems = Models.Cart.CartItem.Select(item => new OrderItem
                {
                    ProductId = item.KhProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.KhProducts.Price
                }).ToList()
            };

            // Add order to the database
            _context.Order.Add(order);

            // Remove the items from the shopping cart
            _context.CartItem.RemoveRange(Cart.CartItem);

            // Save changes to the database
            await _context.SaveChangesAsync();

            return RedirectToAction("ViewOrders");
        }


        // Action to view details of a specific order
        [Authorize]
        public async Task<IActionResult> OrderDetails(int id)
        {
            // Get the order with the specified id
            var order = await _context.Order
                .Include(o => o.OrderItem) // Include related order items
                .ThenInclude(oi => oi.KhProducts) // Include related products
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound(); // Return 404 Not Found if order is not found
            }

            return View(order);
        }

        // Action to view previous orders
        [Authorize]
        public async Task<IActionResult> ViewOrders()
        {
            // Get the current user
            var user = await _userManager.GetUserAsync(User);

            // Retrieve orders for the current user
            var orders = await _context.Order
                .Include(o => o.OrderItem) // Include related order items
                .ThenInclude(oi => oi.KhProducts) // Include related products
                .Where(o => o.UserId == user.Id)
                .ToListAsync();

            return View(orders);
        }
    }
}
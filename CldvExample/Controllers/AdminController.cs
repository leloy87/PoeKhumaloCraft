using KhumaloeApp.Data;
using KhumaloeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace KhumaloeApp.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }

        // AdminHome action
        public IActionResult AdminHome()
        {
            return View();
        }

        // Action to view all products
        public IActionResult ViewProducts()
        {
            var products = _context.KhProducts.ToList();
            return View(products);
        }

        // Example of an admin-only action to insert a new product
        [HttpGet]
        public IActionResult InsertProduct()
        {
            return View(new KhProducts()); // Return the view with a new instance of Products model
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult InsertProduct(KhProducts product)
        {
            if (ModelState.IsValid)
            {
                _context.KhProducts.Add(product);
                _context.SaveChanges();
                return RedirectToAction("AdminHome");
            }
            return View(product); // Return to the view with validation errors
        }
        // Action to delete a product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.KhProducts.Find(id);
            if (product == null)
            {
                return NotFound(); // Return 404 if product is not found
            }
            _context.KhProducts.Remove(product);
            _context.SaveChanges();
            return RedirectToAction(nameof(ViewProducts));
        }
    }
}

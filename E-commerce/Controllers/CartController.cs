using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using System.Collections.Generic;
using System.Linq;
using E_commerce.data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CartController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddToCart(int productId)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == productId);
            if (product != null)
            {
                List<Product> cart = HttpContext.Session.Get<List<Product>>("Cart") ?? new List<Product>();
                cart.Add(product);
                HttpContext.Session.Set("Cart", cart);
            }

            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int productId)
        {
            List<Product> cart = HttpContext.Session.Get<List<Product>>("Cart") ?? new List<Product>();
            var productToRemove = cart.FirstOrDefault(p => p.Id == productId);
            if (productToRemove != null)
            {
                cart.Remove(productToRemove);
                HttpContext.Session.Set("Cart", cart);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<Product>>("Cart") ?? new List<Product>();
            return View(cart);
        }

       [HttpPost]
       public IActionResult SubmitOrder()
       {
            List<Product> cart = HttpContext.Session.Get<List<Product>>("Cart") ?? new List<Product>();
            if (cart.Any())
            {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the User ID from the claims
            if (userId == null)
            {
                return RedirectToAction("Login", "Account"); // Redirect to login if user is not authenticated
            }

            var order = new Order
            {
                UserId = int.Parse(userId),
                OrderDate = DateTime.Now,
                TotalAmount = cart.Sum(p => p.Price) + 5.00m // Add delivery fee
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var product in cart)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = 1, // Assuming quantity is 1 for each product
                    UnitPrice = product.Price
                };
                _context.OrderItems.Add(orderItem);
            }

            _context.SaveChanges();

            // Clear the cart
            HttpContext.Session.Remove("Cart");
        }

        return RedirectToAction("Index");
    }

    }
}

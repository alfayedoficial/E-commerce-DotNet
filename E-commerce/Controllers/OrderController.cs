using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using System.Security.Claims;
using E_commerce.data;

namespace E_Commerce.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("MyOrders")]
        public IActionResult MyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var orders = _context.Orders
                .Where(o => o.UserId == int.Parse(userId))
                .OrderByDescending(o => o.OrderDate) // Order by OrderDate descending from new to old
                .Select(o => new OrderViewModel
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    TotalAmount = o.TotalAmount,
                    Items = o.OrderItems.Select(oi => new OrderItemViewModel
                    {
                        ProductName = oi.Product.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice
                    }).ToList()
                })
                .ToList();

            return View("~/Views/MyOrders/Index.cshtml", orders); 
        }
    }
}

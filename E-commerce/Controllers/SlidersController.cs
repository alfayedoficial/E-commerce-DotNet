using Microsoft.AspNetCore.Mvc;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;
using E_commerce.data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_Commerce.Controllers
{
    public class SlidersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SlidersController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var sliders = _context.Sliders.Include(s => s.Product).ToList();
            return View(sliders);
        }

        public IActionResult Create()
        {
            ViewBag.Products = new SelectList(_context.Products, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(int productId)
        {
            var slider = new Slider { ProductId = productId };
            _context.Sliders.Add(slider);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteSlider(int id)
        {
            var slider = _context.Sliders.Find(id);
            if (slider != null)
            {
                _context.Sliders.Remove(slider);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

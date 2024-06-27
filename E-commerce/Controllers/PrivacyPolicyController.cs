
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    public class PrivacyPolicyController : Controller
    {
       

        public IActionResult Index()
        {
            var currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            ViewBag.CurrentTime = currentTime;
            return View();
        }


    }
}

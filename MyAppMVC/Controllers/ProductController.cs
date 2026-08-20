using Microsoft.AspNetCore.Mvc;

namespace MyAppMVC.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}

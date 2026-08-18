using Microsoft.AspNetCore.Mvc;

namespace MyAppMVC.Controllers
{
    public class DemoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Home()
        {
            return Home();
        }
        public IActionResult Introduct()
        {
            return Introduct();
        }
        public IActionResult Product() { 
            return Product();
        }
        public IActionResult Contact()
        {
            return Contact();
        }
    }
}

using HoangBaTrung_241224491_2026.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HoangBaTrung_241224491_2026.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlnhaSachContext _db;

        public HomeController(QlnhaSachContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var ds = _db.Saches.Where(s => s.Gia != null && s.Gia >= 100).ToList();
            ViewBag.TieuDe = "Hoàng Bá Trung";
            return View(ds);
        }

        public IActionResult LocTheoTheLoai(int id)
        {
            var tl = _db.TheLoais.Find(id);
            var ds = _db.Saches.Where(s => s.MaTL == id).ToList();
            ViewBag.TieuDe = tl?.TenTL;
            return PartialView("HoangBaTrung_MainContent", ds);
        }
        
        [HttpPost]
        public IActionResult Create(Sach s)
        {
            ModelState.Remove("MaTLNavigation");
            if (ModelState.IsValid)
            {
                _db.Saches.Add(s);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DsTheLoai = new SelectList(_db.TheLoais, "MaTL", "TenTL", s.MaTL);
            return View(s);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

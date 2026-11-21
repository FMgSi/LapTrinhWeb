using HoangBaTrung_241224491_2023.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HoangBaTrung_241224491_2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlhangHoaContext _db;

        public HomeController(QlhangHoaContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            var ds = _db.HangHoas.Where(h => h.Gia != null && h.Gia >= 100).ToList();
            ViewBag.TieuDe = "Hoàng Bá Trung";
            return View(ds);
        }

        public IActionResult LocTheoLoai(int id)
        {
            var loai = _db.LoaiHangs.Find(id);
            var ds = _db.HangHoas.Where(h => h.MaLoai == id).ToList();
            ViewBag.TieuDe = loai?.TenLoai;
            return PartialView("HoangBaTrung_MainContent", ds);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DsLoaiHang = new SelectList(_db.LoaiHangs.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HangHoa hh)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.HangHoas.Add(hh);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiHang = new SelectList(_db.LoaiHangs.ToList(), "MaLoai", "TenLoai", hh.MaLoai);
            return View(hh);
        }

        [ResponseCache(Duration = 0 , Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}

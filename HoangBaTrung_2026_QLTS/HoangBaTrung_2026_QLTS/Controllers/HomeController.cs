using HoangBaTrung_2026_QLTS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HoangBaTrung_2026_QLTS.Controllers
{
    public class HomeController : Controller
    {
        private readonly QltaiSanContext _db;

        public HomeController(QltaiSanContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var ds = _db.TaiSans.Where(t => t.NguyenGia != null && t.NguyenGia >= 20).ToList();
            ViewBag.TieuDe = "Hoàng Bá Trung";
            return View(ds);
        }

        public IActionResult LocTheoLoai(int id)
        {
            var loai = _db.LoaiTaiSans.Find(id);
            var ds = _db.TaiSans.Where(t => t.MaLoai == id).ToList();
            ViewBag.TieuDe = loai?.TenLoai;
            return PartialView("HoangBaTrung_MainContent", ds);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaiSan ts)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.TaiSans.Add(ts);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ts = _db.TaiSans.Find(id);
            if (ts == null)
            {
                return NotFound();
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaiSan ts)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.TaiSans.Update(ts);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
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

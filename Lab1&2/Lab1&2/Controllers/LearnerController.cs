using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        // Số lượng phần tử hiển thị trên mỗi trang (đặt là 3 theo bài Lab)
        private readonly int pageSize = 3;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // Action Index: Tải trang chính lần đầu tiên
        public IActionResult Index(int? mid)
        {
            var learners = db.Learners.Include(m => m.Major).AsQueryable();

            if (mid != null && mid > 0)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.mid = mid;
            }

            int totalRecords = learners.Count();
            int pageNum = (int)Math.Ceiling(totalRecords / (float)pageSize);
            if (pageNum == 0) pageNum = 1;

            ViewBag.pageNum = pageNum;
            ViewBag.pageIndex = 1;
            ViewBag.totalRecords = totalRecords;

            var result = learners.Take(pageSize).ToList();
            return View(result);
        }

        // Action phục vụ AJAX: Lọc theo ngành + Tìm kiếm từ khóa + Phân trang kết hợp
        public IActionResult LearnerFilter(int? mid, string? keyword, int? pageIndex)
        {
            var learners = db.Learners.Include(m => m.Major).AsQueryable();

            // 1. Lọc theo chuyên ngành
            if (mid != null && mid > 0)
            {
                learners = learners.Where(l => l.MajorID == mid);
                ViewBag.mid = mid;
            }

            // 2. Tìm kiếm theo từ khóa (hỗ trợ cả Tên và Họ)
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                learners = learners.Where(l => l.FirstMidName.ToLower().Contains(keyword.ToLower()) 
                                            || l.LastName.ToLower().Contains(keyword.ToLower()));
                ViewBag.keyword = keyword;
            }

            // 3. Tính toán tổng số lượng bản ghi và số trang
            int totalRecords = learners.Count();
            int pageNum = (int)Math.Ceiling(totalRecords / (float)pageSize);
            if (pageNum == 0) pageNum = 1;

            // Xác định trang hiện tại, điều chỉnh nếu vượt quá giới hạn
            int page = pageIndex == null || pageIndex <= 0 ? 1 : pageIndex.Value;
            if (page > pageNum) page = pageNum;

            ViewBag.pageNum = pageNum;
            ViewBag.pageIndex = page;
            ViewBag.totalRecords = totalRecords;

            // 4. Cắt trang bằng Skip và Take
            var result = learners.Skip(pageSize * (page - 1))
                                 .Take(pageSize)
                                 .ToList();

            return PartialView("LearnerTable", result);
        }

        // ==========================================
        // CÁC CHỨC NĂNG CRUD CƠ BẢN
        // ==========================================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors.OrderBy(m => m.MajorName), "MajorID", "MajorName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (ModelState.IsValid)
            {
                db.Add(learner);
                await db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors.OrderBy(m => m.MajorName), "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Learners == null) return NotFound();

            var learner = await db.Learners.FindAsync(id);
            if (learner == null) return NotFound();

            ViewBag.MajorID = new SelectList(db.Majors.OrderBy(m => m.MajorName), "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LearnerID,FirstMidName,LastName,MajorID,EnrollmentDate")] Learner learner)
        {
            if (id != learner.LearnerID) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(learner);
                    await db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!db.Learners.Any(e => e.LearnerID == learner.LearnerID))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors.OrderBy(m => m.MajorName), "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Learners == null) return NotFound();

            var learner = await db.Learners
                .Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefaultAsync(m => m.LearnerID == id);

            if (learner == null) return NotFound();

            if (learner.Enrollments != null && learner.Enrollments.Count > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }

            return View(learner);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (db.Learners == null) return Problem("Entity set 'Learners' is null.");

            var learner = await db.Learners.FindAsync(id);
            if (learner != null)
            {
                db.Learners.Remove(learner);
                await db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

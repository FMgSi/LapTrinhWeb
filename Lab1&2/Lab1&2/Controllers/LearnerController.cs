using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.Controllers
{
    public class LearnerController : Controller
    {
        private readonly SchoolContext db;

        public LearnerController(SchoolContext context)
        {
            db = context;
        }

        // 1. Index (List)
        public async Task<IActionResult> Index()
        {
            var learners = db.Learners.Include(m => m.Major);
            return View(await learners.ToListAsync());
        }

        // 2. Create (GET)
        public IActionResult Create()
        {
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName");
            return View();
        }

        // 2. Create (POST)
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
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // 3. Edit (GET)
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || db.Learners == null) return NotFound();

            var learner = await db.Learners.FindAsync(id);
            if (learner == null) return NotFound();

            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // 3. Edit (POST)
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
                    if (!db.Learners.Any(e => e.LearnerID == learner.LearnerID)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.MajorID = new SelectList(db.Majors, "MajorID", "MajorName", learner.MajorID);
            return View(learner);
        }

        // 4. Delete (GET)
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || db.Learners == null) return NotFound();

            var learner = await db.Learners
                .Include(l => l.Major)
                .Include(e => e.Enrollments)
                .FirstOrDefaultAsync(m => m.LearnerID == id);

            if (learner == null) return NotFound();

            if (learner.Enrollments.Count > 0)
            {
                return Content("This learner has some enrollments, can't delete!");
            }

            return View(learner);
        }

        // 4. Delete (POST)
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

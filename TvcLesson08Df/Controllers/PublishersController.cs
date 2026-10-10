using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TvcLesson08Df.Models;

namespace TvcLesson08Df.Controllers;

public class PublishersController : Controller
{
    private readonly BookStoreContext _context;

    public PublishersController(BookStoreContext context)
    {
        _context = context;
    }

    // GET: Publishers
    public async Task<IActionResult> Index()
    {
        var publishers = await _context.Publishers
            .Include(p => p.Books)
            .ToListAsync();
        return View(publishers);
    }

    // GET: Publishers/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers
            .Include(p => p.Books)
            .FirstOrDefaultAsync(m => m.PublisherId == id);
        if (publisher == null)
        {
            return NotFound();
        }

        return View(publisher);
    }

    // GET: Publishers/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Publishers/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("PublisherId,PublisherName,Address,Phone")] Publisher publisher)
    {
        if (ModelState.IsValid)
        {
            _context.Add(publisher);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm nhà xuất bản thành công!";
            return RedirectToAction(nameof(Index));
        }
        return View(publisher);
    }

    // GET: Publishers/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher == null)
        {
            return NotFound();
        }
        return View(publisher);
    }

    // POST: Publishers/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("PublisherId,PublisherName,Address,Phone")] Publisher publisher)
    {
        if (id != publisher.PublisherId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(publisher);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật nhà xuất bản thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublisherExists(publisher.PublisherId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(publisher);
    }

    // GET: Publishers/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var publisher = await _context.Publishers
            .Include(p => p.Books)
            .FirstOrDefaultAsync(m => m.PublisherId == id);
        if (publisher == null)
        {
            return NotFound();
        }

        return View(publisher);
    }

    // POST: Publishers/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var publisher = await _context.Publishers.FindAsync(id);
        if (publisher != null)
        {
            _context.Publishers.Remove(publisher);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa nhà xuất bản thành công!";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool PublisherExists(int id)
    {
        return _context.Publishers.Any(e => e.PublisherId == id);
    }
}

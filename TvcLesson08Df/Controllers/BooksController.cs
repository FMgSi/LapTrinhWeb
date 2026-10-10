using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TvcLesson08Df.Models;

namespace TvcLesson08Df.Controllers;

public class BooksController : Controller
{
    private readonly BookStoreContext _context;

    public BooksController(BookStoreContext context)
    {
        _context = context;
    }

    // GET: Books
    public async Task<IActionResult> Index(string? searchString, int? categoryId, int? publisherId)
    {
        var booksQuery = _context.Books
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchString))
        {
            booksQuery = booksQuery.Where(b => b.Title.Contains(searchString) || (b.Author != null && b.Author.Contains(searchString)));
        }

        if (categoryId.HasValue && categoryId > 0)
        {
            booksQuery = booksQuery.Where(b => b.CategoryId == categoryId.Value);
        }

        if (publisherId.HasValue && publisherId > 0)
        {
            booksQuery = booksQuery.Where(b => b.PublisherId == publisherId.Value);
        }

        ViewData["Categories"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", categoryId);
        ViewData["Publishers"] = new SelectList(await _context.Publishers.ToListAsync(), "PublisherId", "PublisherName", publisherId);
        ViewData["CurrentSearch"] = searchString;

        return View(await booksQuery.ToListAsync());
    }

    // GET: Books/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // GET: Books/Create
    public async Task<IActionResult> Create()
    {
        ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName");
        ViewData["PublisherId"] = new SelectList(await _context.Publishers.ToListAsync(), "PublisherId", "PublisherName");
        return View();
    }

    // POST: Books/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("BookId,Title,Author,Price,Quantity,CategoryId,PublisherId,ImageUrl,Description")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Thêm sách mới thành công!";
            return RedirectToAction(nameof(Index));
        }
        ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", book.CategoryId);
        ViewData["PublisherId"] = new SelectList(await _context.Publishers.ToListAsync(), "PublisherId", "PublisherName", book.PublisherId);
        return View(book);
    }

    // GET: Books/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", book.CategoryId);
        ViewData["PublisherId"] = new SelectList(await _context.Publishers.ToListAsync(), "PublisherId", "PublisherName", book.PublisherId);
        return View(book);
    }

    // POST: Books/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,Author,Price,Quantity,CategoryId,PublisherId,ImageUrl,Description")] Book book)
    {
        if (id != book.BookId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Cập nhật thông tin sách thành công!";
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.BookId))
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
        ViewData["CategoryId"] = new SelectList(await _context.Categories.ToListAsync(), "CategoryId", "CategoryName", book.CategoryId);
        ViewData["PublisherId"] = new SelectList(await _context.Publishers.ToListAsync(), "PublisherId", "PublisherName", book.PublisherId);
        return View(book);
    }

    // GET: Books/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var book = await _context.Books
            .Include(b => b.Category)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(m => m.BookId == id);
        if (book == null)
        {
            return NotFound();
        }

        return View(book);
    }

    // POST: Books/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Đã xóa sách thành công!";
        }

        return RedirectToAction(nameof(Index));
    }

    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.BookId == id);
    }
}

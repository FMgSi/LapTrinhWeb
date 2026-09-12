using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;

namespace WebApplication3.Controllers
{
    public class BookController : Controller
    {
        protected Book book = new Book();

        public IActionResult Index()
        {
            ViewBag.authors = book.Authors;
            ViewBag.genres = book.Geners;
            var books = book.GetBookList();
            return View(books);
        }

        public IActionResult Create()
        {
            ViewBag.authors = book.Authors;
            ViewBag.genres = book.Geners;
            Book model = new Book();
            return View(model);
        }

        public IActionResult Edit(int id)
        {
            ViewBag.authors = book.Authors;
            ViewBag.genres = book.Geners;

            Book model = book.GetBookById(id);
            return View(model);


        }

        public IActionResult PopularBook()
        {
            var books = book.GetBookList();
            return PartialView(books);
        }
    }
}

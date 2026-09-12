using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;

namespace WebApplication3.ViewComponents
{
    public class BookViewComponent : ViewComponent
    {
        protected Book book = new Book();

        // Phương thức Invoke sẽ tự động chạy khi component được gọi trên View
        public IViewComponentResult Invoke()
        {
            var books = book.GetBookList();
            return View(books); // Sẽ tìm file Default.cshtml tương ứng
        }
    }
}
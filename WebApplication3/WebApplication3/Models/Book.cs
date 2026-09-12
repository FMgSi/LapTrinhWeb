using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication3.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenerId { get; set; }
        public string Image {  get; set; }
        public float Price { get; set; }
        public int TotalPage { get; set; }
        public string Sumary { get; set; }

        public List<Book> GetBookList()
        {
            List<Book> books = new List<Book>() {
                new Book(){
                    Id = 1,
                    Title = "Chi pheo",
                    AuthorId = 1,
                    GenerId =1,
                    Image = "images/b1.jpg",
                    Price = 500000,
                    Sumary = "",
                    TotalPage = 250
                },
                new Book()
                {
                    Id = 2,
                    Title = "De men phieu luu ki",
                    AuthorId = 2,
                    GenerId =2,
                    Image = "images/b2.jpg",
                    Price = 500000,
                    Sumary = "",
                    TotalPage = 250
                },
                new Book()
                {
                    Id = 3,
                    Title = "Trang ti",
                    AuthorId = 3,
                    GenerId =3,
                    Image = "images/b3.jpg",
                    Price = 500000,
                    Sumary = "",
                    TotalPage = 250
                },
                new Book()
                {
                    Id = 4,
                    Title = "Chi pheo",
                    AuthorId = 1,
                    GenerId =1,
                    Image = "images/b4.jpg",
                    Price = 500000,
                    Sumary = "",
                    TotalPage = 250
                }
            };

            return books;
            
        }

        public Book GetBookById(int id)
        {
            Book book = this.GetBookList().FirstOrDefault(b => b.Id == id);
            return book;
            
        }

        public List<SelectListItem> Authors { get; } = new List<SelectListItem>
        {
            new SelectListItem {Value="1", Text="Nam Cao"},
            new SelectListItem {Value="2", Text="Ngo Tat To"},
            new SelectListItem {Value="3", Text="Adamkhoom"},
            new SelectListItem {Value="4", Text="Vu Trong Phung"}

        };

        public List<SelectListItem> Geners { get; } = new List<SelectListItem>
        {
            new SelectListItem {Value="1", Text="Truyen tranh"},
            new SelectListItem {Value="2", Text="Van hoc duong dai"},
            new SelectListItem {Value="3", Text="Phat hoc pho thong"},
            new SelectListItem {Value="4", Text="Truyen cuoi"}
        };


    }
}

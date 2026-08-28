using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;
using System.Net;
using System.Reflection;
using System.Xml.Linq;
namespace WebApplication1.Controllers
{
    public class ProductController : Controller
    {
        private readonly List<Category> _categories = new List<Category>()
        {
            new Category { Id = 1, Name = "Quần Áo" },
            new Category { Id = 2, Name = "Túi xách" },
            new Category { Id = 3, Name = "Đồng hồ" },
            new Category { Id = 4, Name = "Tivi" },
            new Category { Id = 5, Name = "Tủ lạnh" }
        };

        private readonly List<Product> _products = new List<Product>()
        {
            new Product {
                Id = 1,
                Name = "Bộ đồ bơi cho trẻ em nam",
                Image = "/images/avatar/01.jpg",
                Price = 50000,
                SalePrice = 35000,
                CategoryId = 1,
                Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit. Ipsa eligendi, voluptatem perspiciatis qui delectus ab unde iure doloribus natus expedita, laborum blanditiis quaerat repellendus necessitatibus nam quo earum ex suscipit.",
                Status = true,
                CreatedAt = new DateTime(2021, 7, 15)
            },
            new Product {
                Id = 2,
                Name = "Bộ đồ bơi cho trẻ em nữ",
                Image = "/images/avatar/02.jpg",
                Price = 50000,
                SalePrice = 35000,
                CategoryId = 1,
                Description = "Lorem ipsum dolor sit amet consectetur adipisicing elit.",
                Status = true,
                CreatedAt = new DateTime(2021, 7, 15)
            },
            new Product {
                Id = 3,
                Name = "Túi thời trang mẫu mới 2021",
                Image = "/images/avatar/03.jpg",
                Price = 50000,
                SalePrice = 35000,
                CategoryId = 2,
                Description = "Thiết kế thời trang, chất liệu da cao cấp phù hợp xu hướng mới.",
                Status = true,
                CreatedAt = new DateTime(2021, 7, 15)
            }
        };

        public IActionResult Index(int? categoryId)
        {
            var products = _products;
            if (categoryId.HasValue)
            {
                products = _products.Where(p => p.CategoryId == categoryId.Value).ToList();
            }

            ViewBag.Categories = _categories;
            ViewBag.Products = products;
            return View();
        }

        public IActionResult Details(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Product = product;
            return View();
        }
    }
}
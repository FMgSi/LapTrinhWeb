using Microsoft.AspNetCore.Mvc;
using MvcModelDemo.Models;

namespace MvcModelDemo.Controllers;

public class UserController : Controller
{
    // 1. Truyền 1 đối tượng qua ViewBag
    public IActionResult SingleUserViewBag()
    {
        var user = new User
        {
            Id = 1,
            name = "John Smith",
            address = "Park Street",
            email = "john@mvcexample.com"
        };

        ViewBag.user = user;
        return View();
    }

    public IActionResult Index()
    {
        var users = new List<User>
        {
            new User { Id = 1, name = "Mark Smith", address = "Park Street", email = "Mark@mvcexample.com" },
            new User { Id = 2, name = "John Parker", address = "New Park", email = "John@mvcexample.com" },
            new User { Id = 3, name = "Steave Edward", address = "Melbourn Street", email = "steave@mvcexample.com" }
        };

        return View(users); // Truyền trực tiếp danh sách vào View
    }
}
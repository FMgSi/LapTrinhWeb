using Microsoft.AspNetCore.Mvc;
using MvcModelDemo.Models;

namespace MvcModelDemo.Controllers;

public class AccountController : Controller
{
    // GET: Account/Login (Hiển thị Form)
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    // Cách 1: Binding qua Primitive Values (Tham số cơ bản)
    [HttpPost]
    public IActionResult LoginPrimitive(string userName, string password)
    {
        if (userName == "Peter" && password == "pass@123")
        {
            return Content("Welcome " + userName);
        }

        ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
        return View("Login");
    }

    // Cách 2: Binding trực tiếp vào Complex Object (Login model)
    [HttpPost]
    public IActionResult Login(Login login)
    {
        if (login.userName == "Peter" && login.password == "pass@123")
        {
            return Content("Welcome " + login.userName);
        }

        ViewBag.Error = "Sai tài khoản hoặc mật khẩu!";
        return View(login);
    }
}
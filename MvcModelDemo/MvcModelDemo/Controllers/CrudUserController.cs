using Microsoft.AspNetCore.Mvc;
using MvcModelDemo.Models;

namespace MvcModelDemo.Controllers;

public class CrudUserController : Controller
{
    // Giả lập database in-memory
    private static List<User> _users = new()
    {
        new User { Id = 1, name = "Mark Smith", address = "Park Street", email = "Mark@mvcexample.com" },
        new User { Id = 2, name = "John Parker", address = "New Park", email = "John@mvcexample.com" }
    };

    public IActionResult Index() => View(_users);

    public IActionResult Details(long id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    public IActionResult Create(User user)
    {
        user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
        _users.Add(user);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(long id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost]
    public IActionResult Edit(User updatedUser)
    {
        var user = _users.FirstOrDefault(u => u.Id == updatedUser.Id);
        if (user != null)
        {
            user.name = updatedUser.name;
            user.address = updatedUser.address;
            user.email = updatedUser.email;
        }
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(long id)
    {
        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user == null) return NotFound();
        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(long id)
    {
        _users.RemoveAll(u => u.Id == id);
        return RedirectToAction(nameof(Index));
    }
}
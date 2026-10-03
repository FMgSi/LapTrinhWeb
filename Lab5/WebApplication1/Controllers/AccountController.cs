using Microsoft.AspNetCore.Mvc;
using NetCoreMVCLAB5.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NetCoreMVCLAB5.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            List<Account> accounts = new List<Account>();
            return View(accounts);
        }

        // GET: Account/Create
        public ActionResult Create()
        {
            Account model = new Account();
            return View(model);
        }

        // POST: Account/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Account model)
        {
            if (ModelState.IsValid)
            {
                // Thêm logic lưu tài khoản vào cơ sở dữ liệu nếu cần
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // Action kiểm tra tính hợp lệ số điện thoại (Remote Validation - Bước 8-10)
        [AcceptVerbs("GET", "POST")]
        public IActionResult VerifyPhone(string phone)
        {
            string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            bool isPhone = !string.IsNullOrEmpty(phone) && Regex.IsMatch(phone, pattern);

            if (!isPhone)
            {
                return Json($"Số điện thoại {phone} không đúng định dạng. VD: 0986421227 hoặc 098.421.1127");
            }
            return Json(true);
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using TvcMember.Models;

namespace TvcMember.Controllers
{
    public class TvcMemberController : Controller
    {
        private static List<Member> members = new List<Member>();

        public IActionResult Index()
        {
            return View(members);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Member member)
        {
            // Kiểm tra tính hợp lệ qua Data Annotations [00:40:47]
            if (!ModelState.IsValid)
            {
                // Dữ liệu sai -> trả lại View kèm dữ liệu đã nhập để hiện lỗi validation
                return View(member);
            }

            // Gán Id tự tăng đơn giản nếu hợp lệ [00:44:47]
            member.Id = members.Count > 0 ? members.Max(m => m.Id) + 1 : 1;
            members.Add(member);

            return RedirectToAction(nameof(Index));
        }
    }
}
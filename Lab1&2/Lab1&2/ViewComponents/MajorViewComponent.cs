using Microsoft.AspNetCore.Mvc;
using MyWebApp.Data;
using MyWebApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyWebApp.ViewComponents
{
    public class MajorViewComponent : ViewComponent
    {
        private readonly SchoolContext db;

        public MajorViewComponent(SchoolContext _context)
        {
            db = _context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Lấy danh sách ngành học sắp xếp theo tên
            var majors = db.Majors.OrderBy(m => m.MajorName).ToList();
            return View("RenderMajor", majors);
        }
    }
}


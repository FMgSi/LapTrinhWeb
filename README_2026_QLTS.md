# Hướng Dẫn Chi Tiết Làm Đề Thi Mẫu 2026 — Quản Lý Tài Sản (`QLTaiSan` / AssetHub)

Hướng dẫn từng bước làm bộ đề **`DeThi_Mau_2026SV_QLTS`** (gồm **cả Đề 1 — Thêm mới `Create`** và **Đề 2 — Sửa thông tin `Edit`**) trên **Visual Studio (VS tím) + ASP.NET Core 10 MVC**, bám sát phong cách sạch chuẩn của bài `HoangBaTrung_241224491_2026`.

> **Lưu ý về tên project / namespace:** Khi làm bài thi thật, đặt tên đúng quy định `HoTen_MaSV` (ví dụ `HoangBaTrung_241224491`). Khi luyện tập chung trong repo này, có thể đặt là `HoangBaTrung_241224491_QLTS` (và đổi namespace trong code tương ứng).

---

## Tóm tắt đề bài (Đề 1 & Đề 2)

| Câu | Điểm | Nội dung |
|---|---|---|
| **Câu 1** | 2đ | Tạo dự án ASP.NET Core MVC (`.NET 10`), dùng `index.html` xây dựng `HoangBaTrung_Layout.cshtml`, tách `HoangBaTrung_Header`, `HoangBaTrung_MainContent`, `HoangBaTrung_Footer`, copy `css`, `images`, `js` vào `wwwroot`. |
| **Câu 2** | 2đ | Dùng EF Core (Database First) sinh Model từ DB `QLTaiSan` (`LoaiTaiSan`, `TaiSan`). Đổ `LoaiTaiSan` lên thanh điều hướng, đổi `Trang chủ` thành `"Hoàng Bá Trung"`, bỏ 4 menu tĩnh cũ (`Giới thiệu`, `Kiểm kê`, `Bảo trì`, `Liên hệ`). |
| **Câu 3** | 1đ | Load danh sách `TaiSan` có **`NguyenGia >= 20`** (triệu đồng, bỏ bản ghi `NguyenGia = NULL`), tiêu đề nội dung là `"Hoàng Bá Trung"`. |
| **Câu 4** | 2đ | Bấm từng loại tài sản trên menu → lọc `TaiSan` theo `MaLoai` bằng **AJAX** (không tải lại trang), tiêu đề đổi thành `TenLoai`. |
| **Câu 5 (Đề 1)** | 3đ | **Thêm Tài sản mới (`Create`)**: dropdown `MaLoai` (`TenLoai`), nhập `TenTS`, `NguyenGia`, `Anh`, nút `Create`. |
| **Câu 5 (Đề 2)** | 3đ | **Sửa thông tin tài sản (`Edit`)**: tại mỗi thẻ tài sản có nút `Sửa` → mở form chứa sẵn dữ liệu hiện tại, nút `Save` → cập nhật bản ghi cũ (**không tạo thêm bản ghi mới**). |
| **Câu 5.2 (Cả 2 đề)** | 1đ | **Data Annotations**: `TenTS` không được để trống; `NguyenGia` là số từ **1 đến 2000**; `Anh` có đuôi **`.jpg \| .jpeg \| .png \| .webp`**. |

---

## Bước 0: Tạo cơ sở dữ liệu `QLTaiSan` trong SQL Server

1. Mở **SSMS**, kết nối tới SQL Server (`localhost,1433`, user `sa` hoặc Windows Auth tùy máy).
2. Mở file `C:\Users\qualcomm\Downloads\DeThi_Mau_2026SV_QLTS\Database_script.sql`.
3. Bấm **Execute (F5)**.
   - Script `QLTaiSan` đã viết chuẩn `CREATE DATABASE [QLTaiSan]` (không còn lỗi đường dẫn ổ `D:\` như đề 2023), tự xóa DB cũ nếu trùng và nạp sẵn 5 `LoaiTaiSan` + 16 `TaiSan` (trong đó bản ghi số 16 có `NguyenGia = NULL`).
4. **Kiểm tra nhanh:** Refresh mục `Databases` xem đã xuất hiện **`QLTaiSan`** chưa trước khi sang Bước 2 (tránh lỗi `Error 4060: Cannot open database`).

---

## Bước 1: Tạo Project mới & Ghép tài nguyên vào `wwwroot` *(Câu 1)*

### 1.1. Tạo Project trên Visual Studio
1. **Create a new project** → **ASP.NET Core Web App (Model-View-Controller)** → **Next**.
2. Đặt tên: `HoangBaTrung_241224491_QLTS` *(đi thi thật đặt `HoangBaTrung_241224491`)*.
3. Framework: **`.NET 10.0`** → **Create**.

### 1.2. Ghép tài nguyên *(Câu 1.2)*
Từ thư mục `C:\Users\qualcomm\Downloads\DeThi_Mau_2026SV_QLTS\Template`, copy **3 thư mục** vào `wwwroot/` của project:
- `css` (`layout.css`, `style.css`)
- `images` (`logo.png`, `banner.jpg`, `Img_1.jpg` ... `Img_8.jpg`)
- `js` (`jquery.min.js`, `main.js`)

---

## Bước 2: Cài EF Core & Scaffold Model *(Câu 2.1)*

### 2.1. Cài 3 gói NuGet bản `10.0.x`
Mở **Manage NuGet Packages...** → tab **Browse**, cài 3 gói:
1. `Microsoft.EntityFrameworkCore.SqlServer`
2. `Microsoft.EntityFrameworkCore.Tools`
3. `Microsoft.EntityFrameworkCore.Design`

### 2.2. Chạy lệnh Scaffold trong Package Manager Console
> **Mẹo cực quan trọng:** Thêm cờ **`-UseDatabaseNames`** để EF Core giữ nguyên tên cột **`MaTS`**, **`TenTS`** khớp 100% với đề bài (nếu không có cờ này, EF Core sẽ đổi thành `MaTs`, `TenTs` rất dễ gõ nhầm).

```powershell
Scaffold-DbContext "Server=localhost,1433;Database=QLTaiSan;User Id=sa;Password=<MAT_KHAU_SA>;MultipleActiveResultSets=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -UseDatabaseNames -Force
```

*(Hoặc nếu dùng SQLEXPRESS Windows Auth: `"Server=localhost\SQLEXPRESS;Database=QLTaiSan;Trusted_Connection=True;TrustServerCertificate=True;"`)*

Sau khi chạy xong, kiểm tra trong `Models/` sẽ có:
- `QltaiSanContext.cs` *(chú ý đúng chữ hoa/thường tên class EF sinh ra!)*
- `TaiSan.cs`
- `LoaiTaiSan.cs`

### 2.3. Cấu hình ConnectionString & đăng ký DI
1. Mở **`appsettings.json`**, thêm `ConnectionStrings` *(kiểm tra kỹ dấu `=` trong `Database=QLTaiSan` và chữ `l` trong `MultipleActiveResultSets`)*:
   ```json
   {
     "Logging": {
       "LogLevel": {
         "Default": "Information",
         "Microsoft.AspNetCore": "Warning"
       }
     },
     "AllowedHosts": "*",
     "ConnectionStrings": {
       "QLTaiSanContext": "Server=localhost,1433;Database=QLTaiSan;User Id=sa;Password=<MAT_KHAU_SA>;MultipleActiveResultSets=True;TrustServerCertificate=True;"
     }
   }
   ```

2. Mở **`Models/QltaiSanContext.cs`**, **comment bỏ hàm `OnConfiguring(...)`** để hết cảnh báo build và không hardcode mật khẩu DB.

3. Mở **`Program.cs`**, đăng ký `AddDbContext` ngay sau `AddControllersWithViews()`:
   ```csharp
   using HoangBaTrung_241224491_QLTS.Models;
   using Microsoft.EntityFrameworkCore;

   var builder = WebApplication.CreateBuilder(args);

   builder.Services.AddControllersWithViews();

   builder.Services.AddDbContext<QltaiSanContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("QLTaiSanContext")));
   ```

---

## Bước 3: Thêm Validation cho Model `TaiSan` *(Câu 5.2 — dùng chung cả Đề 1 & Đề 2)*

Mở **`Models/TaiSan.cs`**, thêm `using System.ComponentModel.DataAnnotations;` và gắn Data Annotations theo đúng yêu cầu đề (**1 đến 2000**, đuôi **`.jpg | .jpeg | .png | .webp`**):

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HoangBaTrung_241224491_QLTS.Models;

public partial class TaiSan
{
    public int MaTS { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại tài sản")]
    public int MaLoai { get; set; }

    [Required(ErrorMessage = "Tên tài sản không được để trống")]
    public string TenTS { get; set; } = null!;

    [Required(ErrorMessage = "Nguyên giá không được để trống")]
    [Range(1, 2000, ErrorMessage = "Nguyên giá phải là số từ 1 đến 2000")]
    public decimal? NguyenGia { get; set; }

    [Required(ErrorMessage = "Tên file ảnh không được để trống")]
    [RegularExpression(@"^.+\.(jpg|jpeg|png|webp|JPG|JPEG|PNG|WEBP)$",
        ErrorMessage = "Ảnh phải có đuôi .jpg, .jpeg, .png hoặc .webp")]
    public string? Anh { get; set; }

    public virtual LoaiTaiSan MaLoaiNavigation { get; set; } = null!;
}
```

---

## Bước 4: Tách Layout & Partial Views từ `index.html` *(Câu 1.1 & Câu 2.2)*

Tạo 4 file trong **`Views/Shared/`** (chuột phải `Views/Shared` → **Add → View → Razor View - Empty**).

### 4.1. `Views/Shared/HoangBaTrung_Header.cshtml` *(Câu 2.2)*
```cshtml
@inject QltaiSanContext db

<!-- Begin Header -->
<header class="header">
    <div class="topbar">
        <div class="container">
            <span>Phòng Hành chính - Quản trị: <b>(024) 3999 8888</b> &nbsp;|&nbsp; Hỗ trợ: taisan@assethub.vn</span>
            <span>Xin chào, Quản trị viên &nbsp;|&nbsp; Đăng xuất</span>
        </div>
    </div>
    <div class="header-main">
        <div class="container">
            <a asp-controller="Home" asp-action="Index">
                <img src="~/images/logo.png" alt="AssetHub" />
            </a>
            <div class="cart-box">Tổng tài sản: 16 - Nguyên giá: 3.125 triệu</div>
        </div>
    </div>
    <nav class="navbar">
        <div class="container">
            <ul class="menu">
                <li class="active">
                    <a asp-controller="Home" asp-action="Index">Hoàng Bá Trung</a>
                </li>
                @foreach (var loai in db.LoaiTaiSans.ToList())
                {
                    <li>
                        <a href="#" class="js-loai" data-id="@loai.MaLoai">@loai.TenLoai</a>
                    </li>
                }
                @* Dòng dưới dành cho Đề 1 (Thêm mới). Nếu chỉ làm Đề 2 có thể bỏ hoặc giữ để test cả 2 *@
                <li>
                    <a asp-controller="Home" asp-action="Create">Thêm tài sản</a>
                </li>
            </ul>
            <form class="search-box">
                <input type="text" placeholder="Tìm tài sản..." />
                <button type="button">Tìm</button>
            </form>
        </div>
    </nav>
</header>
<!-- End Header -->
```

### 4.2. `Views/Shared/HoangBaTrung_MainContent.cshtml` *(Câu 3, Câu 4 & nút Sửa cho Đề 2)*
> **Chú ý class CSS của AssetHub:** Dùng `.asset-card` và `.asset-foot` (khác với `.book-card` / `.book-foot` ở đề BookHub).
> - Nếu làm **Đề 1**: nút trong `.asset-foot` để `Chi tiết`.
> - Nếu làm **Đề 2**: nút trong `.asset-foot` là thẻ `<a asp-controller="Home" asp-action="Edit" asp-route-id="@ts.MaTS" class="btn-buy">Sửa</a>`.

```cshtml
@model IEnumerable<TaiSan>

<div class="section-head">
    <h3>@ViewBag.TieuDe</h3>
    <a href="#">Xem tất cả &rsaquo;</a>
</div>
<div class="grid">
    @foreach (var ts in Model)
    {
        <div class="asset-card">
            <a href="#"><img src="~/images/@ts.Anh" alt="@ts.TenTS" /></a>
            <h2>@ts.TenTS</h2>
            <div class="asset-foot">
                <span class="price">@(ts.NguyenGia.HasValue ? ts.NguyenGia.Value.ToString("0") + " tr" : "Chưa định giá")</span>
                @* Đề 2 dùng link Sửa này; nếu chỉ làm Đề 1 có thể đổi thành <button class="btn-buy">Chi tiết</button> *@
                <a asp-controller="Home" asp-action="Edit" asp-route-id="@ts.MaTS" class="btn-buy">Sửa</a>
            </div>
        </div>
    }
</div>
```

### 4.3. `Views/Shared/HoangBaTrung_Footer.cshtml`
```cshtml
<!-- Begin Footer -->
<footer class="footer">
    <div class="container">
        <div class="grid">
            <div>
                <h4>Về AssetHub</h4>
                <ul>
                    <li><a href="#">Giới thiệu</a></li>
                    <li><a href="#">Quy chế quản lý</a></li>
                    <li><a href="#">Sơ đồ đơn vị</a></li>
                </ul>
            </div>
            <div>
                <h4>Hỗ trợ người dùng</h4>
                <ul>
                    <li><a href="#">Hướng dẫn kiểm kê</a></li>
                    <li><a href="#">Quy trình bảo trì</a></li>
                    <li><a href="#">Bảo mật dữ liệu</a></li>
                </ul>
            </div>
            <div>
                <h4>Tài khoản</h4>
                <ul>
                    <li><a href="#">Đăng nhập</a></li>
                    <li><a href="#">Hồ sơ cá nhân</a></li>
                    <li><a href="#">Tài sản được giao</a></li>
                </ul>
            </div>
            <div>
                <h4>Liên hệ</h4>
                <ul>
                    <li>Hotline: (024) 3999 8888</li>
                    <li>Email: taisan@assethub.vn</li>
                </ul>
            </div>
        </div>
        <div class="copyright">&copy; 2026 AssetHub. Template phục vụ đề thi mẫu.</div>
    </div>
</footer>
<!-- End Footer -->
```

### 4.4. `Views/Shared/HoangBaTrung_Layout.cshtml` *(Kèm AJAX — Câu 4)*
```cshtml
<!DOCTYPE html>
<html lang="vi">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <title>@(ViewData["Title"] ?? "Trang chủ") - AssetHub</title>
    <link href="~/css/layout.css" rel="stylesheet" type="text/css" />
    <link href="~/css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <partial name="HoangBaTrung_Header" />

    <main class="main">
        <div class="container">
            <div class="banner"><img src="~/images/banner.jpg" alt="Banner" /></div>
            <div id="noidung">
                @RenderBody()
            </div>
        </div>
    </main>

    <partial name="HoangBaTrung_Footer" />

    <div class="to-top">&uarr;</div>
    <script src="~/js/jquery.min.js"></script>
    <script src="~/js/main.js"></script>
    <script>
        $(document).on('click', '.js-loai', function (e) {
            e.preventDefault();
            var id = $(this).data('id');
            $('#noidung').load('@Url.Action("LocTheoLoai", "Home")' + '?id=' + id);
            $('.menu li').removeClass('active');
            $(this).parent().addClass('active');
        });
    </script>
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```

### 4.5. Cập nhật `Views/_ViewStart.cshtml`
```cshtml
@{
    Layout = "HoangBaTrung_Layout";
}
```

---

## Bước 5: Code `Controllers/HomeController.cs` *(Đủ cả Đề 1 `Create` & Đề 2 `Edit`)*

```csharp
using HoangBaTrung_241224491_QLTS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HoangBaTrung_241224491_QLTS.Controllers
{
    public class HomeController : Controller
    {
        private readonly QltaiSanContext _db;

        public HomeController(QltaiSanContext db)
        {
            _db = db;
        }

        // Câu 3: Hiển thị TaiSan có NguyenGia >= 20 (bỏ NULL), tiêu đề là họ tên SV
        public IActionResult Index()
        {
            var ds = _db.TaiSans.Where(t => t.NguyenGia != null && t.NguyenGia >= 20).ToList();
            ViewBag.TieuDe = "Hoàng Bá Trung";
            return View(ds);
        }

        // Câu 4: Lọc TaiSan theo MaLoai bằng AJAX
        public IActionResult LocTheoLoai(int id)
        {
            var loai = _db.LoaiTaiSans.Find(id);
            var ds = _db.TaiSans.Where(t => t.MaLoai == id).ToList();
            ViewBag.TieuDe = loai?.TenLoai;
            return PartialView("HoangBaTrung_MainContent", ds);
        }

        // ================= ĐỀ 1: THÊM TÀI SẢN MỚI =================
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(TaiSan ts)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.TaiSans.Add(ts);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
        }

        // ================= ĐỀ 2: SỬA THÔNG TIN TÀI SẢN =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var ts = _db.TaiSans.Find(id);
            if (ts == null)
            {
                return NotFound();
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(TaiSan ts)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.TaiSans.Update(ts);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiTaiSan = new SelectList(_db.LoaiTaiSans.ToList(), "MaLoai", "TenLoai", ts.MaLoai);
            return View(ts);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
```

---

## Bước 6: Tạo các View trong `Views/Home/` & CSS Form

### 6.1. `Views/Home/Index.cshtml`
```cshtml
@model IEnumerable<TaiSan>
@{
    ViewData["Title"] = "Trang chủ";
    Layout = "HoangBaTrung_Layout";
}

<partial name="HoangBaTrung_MainContent" model="Model" />
```

### 6.2. `Views/Home/Create.cshtml` *(Dành cho Đề 1)*
```cshtml
@model TaiSan
@{
    ViewData["Title"] = "Thêm tài sản mới";
    Layout = "HoangBaTrung_Layout";
}

<div class="section-head">
    <h3>HOÀNG BÁ TRUNG - THÊM TÀI SẢN MỚI</h3>
</div>

<div class="form-box">
    <form asp-action="Create" method="post">
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>

        <div class="form-group">
            <label asp-for="MaLoai">Loại tài sản</label>
            <select asp-for="MaLoai" asp-items="ViewBag.DsLoaiTaiSan" class="form-control"></select>
            <span asp-validation-for="MaLoai" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="TenTS">Tên tài sản</label>
            <input asp-for="TenTS" type="text" class="form-control" />
            <span asp-validation-for="TenTS" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="NguyenGia">Nguyên giá (triệu)</label>
            <input asp-for="NguyenGia" type="text" class="form-control" />
            <span asp-validation-for="NguyenGia" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="Anh">Tên file ảnh</label>
            <input asp-for="Anh" type="text" class="form-control" />
            <span asp-validation-for="Anh" class="text-danger"></span>
        </div>

        <button type="submit" class="btn-buy">Create</button>
    </form>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

### 6.3. `Views/Home/Edit.cshtml` *(Dành cho Đề 2)*
> **BẮT BUỘC PHẢI CÓ `<input type="hidden" asp-for="MaTS" />`!** Nếu thiếu dòng này, `MaTS` gửi lên sẽ là `0` → EF Core sẽ tạo nhầm bản ghi mới hoặc lỗi khóa chính thay vì sửa bản ghi hiện tại.

```cshtml
@model TaiSan
@{
    ViewData["Title"] = "Sửa thông tin tài sản";
    Layout = "HoangBaTrung_Layout";
}

<div class="section-head">
    <h3>HOÀNG BÁ TRUNG - SỬA THÔNG TIN TÀI SẢN</h3>
</div>

<div class="form-box">
    <form asp-action="Edit" method="post">
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>

        @* QUAN TRỌNG: giữ khóa chính MaTS để EF Core Update đúng bản ghi *@
        <input type="hidden" asp-for="MaTS" />

        <div class="form-group">
            <label asp-for="MaLoai">Loại tài sản</label>
            <select asp-for="MaLoai" asp-items="ViewBag.DsLoaiTaiSan" class="form-control"></select>
            <span asp-validation-for="MaLoai" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="TenTS">Tên tài sản</label>
            <input asp-for="TenTS" type="text" class="form-control" />
            <span asp-validation-for="TenTS" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="NguyenGia">Nguyên giá (triệu)</label>
            <input asp-for="NguyenGia" type="text" class="form-control" />
            <span asp-validation-for="NguyenGia" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="Anh">Tên file ảnh</label>
            <input asp-for="Anh" type="text" class="form-control" />
            <span asp-validation-for="Anh" class="text-danger"></span>
        </div>

        <button type="submit" class="btn-buy">Save</button>
    </form>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

### 6.4. Bổ sung CSS Form vào cuối `wwwroot/css/style.css`
```css
.form-box {
    background: #fff;
    padding: 20px;
    border-radius: 8px;
    max-width: 480px;
    box-shadow: 0 1px 4px rgba(0, 0, 0, .1);
}

.form-group {
    margin-bottom: 14px;
}

.form-group label {
    display: block;
    font-size: 14px;
    margin-bottom: 4px;
    font-weight: 600;
}

.form-control {
    width: 100%;
    padding: 8px 10px;
    border: 1px solid #c5d3d8;
    border-radius: 4px;
}

.text-danger {
    color: #d93025;
    font-size: 13px;
    display: block;
    margin-top: 4px;
}
```

---

## Những lỗi kinh điển & lưu ý bắt buộc phải nhớ khi làm bài thi

1. **Chuỗi kết nối (`appsettings.json` & PMC):**
   - Nhớ chạy `Database_script.sql` trên đúng server (`localhost,1433`) trước khi Scaffold (nếu chưa có DB sẽ dính `Error 4060`).
   - Soát kỹ lỗi gõ nhầm: `Database=QLTaiSan` (đủ dấu `=`) và `MultipleActiveResultSets=True` (đủ chữ `l`).
   - Sau khi Scaffold, **comment bỏ `OnConfiguring`** trong `QltaiSanContext.cs` để hết warning và dùng DI chuẩn.
2. **Tên cột khi Scaffold:**
   - Thêm `-UseDatabaseNames` khi chạy `Scaffold-DbContext` để giữ đúng tên `MaTS`, `TenTS` (tránh EF tự đổi thành `MaTs`, `TenTs`).
   - Nhớ kiểm tra tên class Context EF sinh ra là `QltaiSanContext` (chữ `t` thường!).
3. **Bẫy `Nullable` + `ModelState.IsValid`:**
   - Project `.NET 10` bật sẵn `<Nullable>enable</Nullable>` → Navigation property `MaLoaiNavigation` luôn bị đánh dấu lỗi bắt buộc khi submit form.
   - **Bắt buộc** có dòng `ModelState.Remove("MaLoaiNavigation");` trước `if (ModelState.IsValid)` ở **cả `[HttpPost] Create` lẫn `[HttpPost] Edit`**.
4. **Lỗi gõ nhầm trong View & Controller (rất hay gặp khi code nhanh 60 phút):**
   - Tên `ViewBag.DsLoaiTaiSan` phải **khớp hoa/thường 100%** giữa `[HttpGet]`, `[HttpPost]` và View (`asp-items="ViewBag.DsLoaiTaiSan"`).
   - Chỉ `MaLoai` dùng thẻ `<select>`; còn `TenTS`, `NguyenGia`, `Anh` phải là **`<input ... />`** (đừng copy-paste nhầm thành `<select>`!).
   - Tên section ở cuối `Create.cshtml` / `Edit.cshtml` là **`@section Scripts`** (có chữ `s`), khớp với `@await RenderSectionAsync("Scripts", required: false)` trong Layout (và **không** đặt dấu `;` sau `@await RenderSectionAsync(...)`).
   - Phải nhúng `<script src="~/js/jquery.min.js"></script>` **trước** đoạn code AJAX và `@await RenderSectionAsync("Scripts", ...)` để cả AJAX lẫn client-side validation hoạt động.
   - Trong Razor expression `@(...)` nhớ đóng đủ ngoặc tròn `)`.
5. **Bẫy riêng của Đề 2 (`Edit`):**
   - **Phải có `<input type="hidden" asp-for="MaTS" />`** trong `Edit.cshtml`.
   - Trong `[HttpPost] Edit`, gọi **`_db.TaiSans.Update(ts)`** (tuyệt đối không gọi nhầm `Add(ts)`).
   - Khi lọc AJAX theo loại xong, thẻ tài sản vẫn dùng chung `HoangBaTrung_MainContent.cshtml` nên nút `Sửa` vẫn hoạt động bình thường sau khi lọc!
6. **Lưu ý ngưỡng giá khi test & demo:**
   - Trang chủ (`Index`) lọc **`NguyenGia >= 20`**, còn Validation cho phép **`1` đến `2000`**.
   - Nếu bạn thêm mới hoặc sửa một tài sản về mức giá từ **`1` đến `19`**, dữ liệu vẫn lưu thành công trong DB nhưng quay về trang chủ sẽ **không hiển thị** (chỉ thấy khi bấm vào đúng loại tài sản trên menu). Khi demo quay video, hãy test bằng giá `>= 20` (ví dụ `150` hoặc `350`) để thấy kết quả xuất hiện ngay trên trang chủ!

---

## Checklist kiểm thử nhanh trước khi nộp bài

- [ ] **Câu 1:** Chạy **F5**, mở **F12 → Network** không có lỗi 404 ở `layout.css`, `style.css`, `images/*`, `jquery.min.js`, `main.js`.
- [ ] **Câu 2:** Thanh menu hiện đủ: `HOÀNG BÁ TRUNG` + 5 loại tài sản từ DB (`THIẾT BỊ CNTT`, `THIẾT BỊ VĂN PHÒNG`, `NỘI THẤT`, `PHƯƠNG TIỆN VẬN TẢI`, `MÁY MÓC SẢN XUẤT`).
- [ ] **Câu 3:** Trang chủ có tiêu đề `HOÀNG BÁ TRUNG`, chỉ hiện tài sản có `NguyenGia >= 20` (bản ghi `Màn hình LG 27 inch` giá 6, `Tài sản chưa định giá` `NULL`… không hiện).
- [ ] **Câu 4:** Bấm từng mục trên thanh menu → cập nhật `#noidung` bằng **AJAX** không reload trang, tiêu đề đổi theo tên loại, tab đang chọn sáng nền xanh (`.active`).
- [ ] **Câu 5 (Đề 1 — Create):**
  - Thử nhập `NguyenGia = 0` hoặc `2500` → báo lỗi `1 đến 2000`.
  - Thử nhập `Anh = may_in.gif` → báo lỗi chỉ nhận `.jpg, .jpeg, .png, .webp`.
  - Nhập hợp lệ (VD: `Thiết bị CNTT`, `Máy in HP LaserJet`, `35`, `Img_5.jpg`) → bấm `Create` → quay về trang chủ thấy tài sản mới.
- [ ] **Câu 5 (Đề 2 — Edit):**
  - Bấm `Sửa` ở một tài sản (VD: `Laptop Dell Latitude 5440`) → form đổ sẵn đúng loại, tên, giá `28`, ảnh `Img_1.jpg`.
  - Thử sửa sai (VD: `NguyenGia = 3000` hoặc `Anh = abc.gif`) → báo lỗi ngay trên form, không cập nhật DB.
  - Sửa hợp lệ (VD: đổi tên hoặc giá thành `32`) → bấm `Save` → quay về trang chủ thấy bản ghi cũ đổi thông tin, **không bị nhân đôi bản ghi**.

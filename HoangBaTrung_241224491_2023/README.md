# Hướng Dẫn Chi Tiết Làm `DeThi_Mau_2023` (Visual Studio + ASP.NET Core 10 MVC)

Hướng dẫn từng bước hoàn thiện **Đề thi mẫu 2023 — Quản lý Hàng Hóa (`QLHangHoa`)** trên **Visual Studio** với **ASP.NET Core 10 MVC**, áp dụng kiến trúc gọn sạch theo mẫu bài `HoangBaTrung_241224491_2026`.

> **Lưu ý về tên project / namespace:** Hướng dẫn dùng tên project **`HoangBaTrung_241224491_2023`** khớp với thư mục hiện tại. Khi làm bài thi chính thức, đặt tên theo đúng yêu cầu đề là `HoTen_MaSV` (ví dụ `HoangBaTrung_241224491`) và sửa namespace tương ứng.

---

## Mục lục yêu cầu đề bài (`DeThi_Mau_2023.pdf`)

- **Câu 1 (2 điểm):** Tạo dự án ASP.NET Core MVC, dùng `index.html` dựng `HoangBaTrung_Layout.cshtml`, tách hợp lý `HoangBaTrung_Header`, `HoangBaTrung_MainContent`, `HoangBaTrung_Footer` và ghép tài nguyên (`css`, `images`, `js`, `font`) vào `wwwroot`.
- **Câu 2 (2 điểm):** Sinh Model bằng Entity Framework Core từ CSDL `QLHangHoa`, đổ bảng `LoaiHang` lên thanh điều hướng và đổi mục `Home` thành họ tên sinh viên.
- **Câu 3 (1 điểm):** Load danh sách `HangHoa` có `Gia >= 100` lên trang chủ, tiêu đề thay bằng họ tên sinh viên.
- **Câu 4 (2 điểm):** Kích vào từng mục trên thanh điều hướng → lọc danh sách `HangHoa` theo loại và hiển thị trong main content bằng **AJAX**.
- **Câu 5 (3 điểm):** Chức năng **Thêm Hàng Hóa mới** kèm **DataAnnotations Validation**:
  - `Gia`: là số trong khoảng **100 đến 5000**.
  - `Anh`: tên file có đuôi **`.jpg | .png | .gif | .tiff`**.

---

## Bước 0: Tạo cơ sở dữ liệu `QLHangHoa` trong SQL Server

1. Mở **SQL Server Management Studio (SSMS)** và kết nối tới SQL Server.
2. Mở file `Database_script.sql` trong thư mục `DeThi_Mau_2023`.
3. **Lưu ý lỗi thường gặp:** Ở dòng 4–9, script mặc định lưu file MDF/LDF tại ổ `D:\` (`FILENAME = N'D:\QLHangHoa.mdf'`). Nếu máy không có ổ `D:\` hoặc SQL Server không có quyền ghi vào `D:\`, sửa đoạn dòng 4–9 thành:
   ```sql
   CREATE DATABASE [QLHangHoa]
   GO
   ```
4. Bấm **Execute (F5)** để tạo CSDL `QLHangHoa` (gồm 3 bảng: `HangHoa`, `LoaiHang`, `tblAccount`).

---

## Bước 1: Tạo Project trên Visual Studio (.NET 10) & Ghép tài nguyên *(Câu 1)*

### 1.1. Tạo Project
1. Mở **Visual Studio** → **Create a new project**.
2. Chọn **ASP.NET Core Web App (Model-View-Controller)** → **Next**.
3. Cấu hình:
   - **Project name:** `HoangBaTrung_241224491_2023` *(khi thi đặt `HoTen_MaSV`)*
   - **Framework:** `.NET 10.0`
4. Bấm **Create**.

### 1.2. Ghép tài nguyên vào `wwwroot` *(Câu 1.2)*
Từ thư mục `DeThi_Mau_2023\Template`, copy **cả 4 thư mục** sau vào thư mục `wwwroot` của project:
- `css` (`style.css`, `global.css`, `slider.css`, `easy-responsive-tabs.css`)
- `images` (toàn bộ ảnh sản phẩm `Img_1.jpg` ... `Img_6.jpg`, `logo.png`, ...)
- `js`
- `font` *(bắt buộc copy vì `style.css` có `@font-face` trỏ tới `../font/Amble-Regular-webfont.ttf`)*

---

## Bước 2: Cài Entity Framework Core & Scaffold Model *(Câu 2.1)*

### 2.1. Cài 3 gói NuGet (bản `10.0.x`)
Vào **Tools → NuGet Package Manager → Manage NuGet Packages for Solution** → tab **Browse**, cài đặt:
1. `Microsoft.EntityFrameworkCore.SqlServer`
2. `Microsoft.EntityFrameworkCore.Tools`
3. `Microsoft.EntityFrameworkCore.Design`

### 2.2. Chạy lệnh Scaffold sinh Model
Mở **Tools → NuGet Package Manager → Package Manager Console**, chạy một trong hai lệnh sau tùy cách đăng nhập SQL Server của máy bạn:

- **Dùng tài khoản SQL (`sa`):**
  ```powershell
  Scaffold-DbContext "Server=localhost,1433;Database=QLHangHoa;User Id=sa;Password=<MAT_KHAU_SA>;MultipleActiveResultSets=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
  ```
- **Hoặc dùng Windows Authentication (`SQLEXPRESS`):**
  ```powershell
  Scaffold-DbContext "Server=localhost\SQLEXPRESS;Database=QLHangHoa;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force
  ```

Các file được sinh trong `Models/`:
- `QlhangHoaContext.cs`
- `HangHoa.cs`
- `LoaiHang.cs`
- `TblAccount.cs`

### 2.3. Cấu hình ConnectionString & đăng ký DI
1. Mở **`appsettings.json`**, thêm mục `ConnectionStrings`:
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
       "QLHangHoaContext": "Server=localhost,1433;Database=QLHangHoa;User Id=sa;Password=<MAT_KHAU_SA>;MultipleActiveResultSets=True;TrustServerCertificate=True;"
     }
   }
   ```
   *(Nếu dùng Windows Auth, đổi chuỗi kết nối thành `Server=localhost\\SQLEXPRESS;Database=QLHangHoa;Trusted_Connection=True;TrustServerCertificate=True;`.)*

2. Mở **`Models/QlhangHoaContext.cs`**, comment bỏ hàm `OnConfiguring(...)` để sử dụng cấu hình DI từ `Program.cs`.

3. Mở **`Program.cs`**, thêm `using` và đăng ký `AddDbContext` ngay sau `AddControllersWithViews()`:
   ```csharp
   using HoangBaTrung_241224491_2023.Models;
   using Microsoft.EntityFrameworkCore;

   var builder = WebApplication.CreateBuilder(args);

   builder.Services.AddControllersWithViews();

   builder.Services.AddDbContext<QlhangHoaContext>(options =>
       options.UseSqlServer(builder.Configuration.GetConnectionString("QLHangHoaContext")));
   ```

---

## Bước 3: Thêm Validation cho Model `HangHoa` *(Câu 5.2)*

Mở **`Models/HangHoa.cs`**, bổ sung `System.ComponentModel.DataAnnotations` và các quy tắc kiểm tra theo đúng yêu cầu đề:

```csharp
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HoangBaTrung_241224491_2023.Models;

public partial class HangHoa
{
    public int MaHang { get; set; }

    [Required(ErrorMessage = "Vui lòng chọn loại hàng")]
    public int MaLoai { get; set; }

    [Required(ErrorMessage = "Tên hàng không được để trống")]
    public string TenHang { get; set; } = null!;

    [Required(ErrorMessage = "Giá không được để trống")]
    [Range(100, 5000, ErrorMessage = "Giá phải là số từ 100 đến 5000")]
    public decimal? Gia { get; set; }

    [Required(ErrorMessage = "Tên file ảnh không được để trống")]
    [RegularExpression(@"^.+\.(jpg|png|gif|tiff|JPG|PNG|GIF|TIFF)$",
        ErrorMessage = "Ảnh phải có đuôi .jpg, .png, .gif hoặc .tiff")]
    public string? Anh { get; set; }

    public virtual LoaiHang MaLoaiNavigation { get; set; } = null!;
}
```

---

## Bước 4: Tách Layout & Partial Views từ `index.html` *(Câu 1.1 & Câu 2.2)*

Tạo 4 file Razor View trong **`Views/Shared/`** (chuột phải `Views/Shared` → **Add → View → Razor View - Empty**).

### 4.1. `Views/Shared/HoangBaTrung_Header.cshtml` *(Câu 2.2)*
```cshtml
@inject QlhangHoaContext db

<!-- Begin Header -->
<div class="header">
    <div class="header_top">
        <div class="logo">
            <a asp-controller="Home" asp-action="Index">
                <img src="~/images/logo.png" alt="Logo" />
            </a>
        </div>
        <div class="cart">
            <p>
                Welcome to our Online Store! <span>Cart:</span>
                <div id="dd" class="wrapper-dropdown-2">
                    0 item(s) - $0.00
                    <ul class="dropdown">
                        <li>you have no items in your Shopping cart</li>
                    </ul>
                </div>
            </p>
        </div>
        <div class="clear"></div>
    </div>
    <div class="header_bottom">
        <div class="menu">
            <ul>
                <li class="active">
                    <a asp-controller="Home" asp-action="Index">Hoàng Bá Trung</a>
                </li>
                @foreach (var loai in db.LoaiHangs.ToList())
                {
                    <li>
                        <a href="#" class="js-loaihang" data-id="@loai.MaLoai">@loai.TenLoai</a>
                    </li>
                }
                <li>
                    <a asp-controller="Home" asp-action="Create">Thêm hàng hóa</a>
                </li>
                <div class="clear"></div>
            </ul>
        </div>
        <div class="search_box">
            <form>
                <input type="text" value="Search" onfocus="this.value = '';" onblur="if (this.value == '') {this.value = 'Search';}">
                <input type="button" value="">
            </form>
        </div>
        <div class="clear"></div>
    </div>
</div>
<!-- End Header -->
```

### 4.2. `Views/Shared/HoangBaTrung_MainContent.cshtml` *(Câu 3 & Câu 4)*
```cshtml
@model IEnumerable<HangHoa>

<div class="content_top">
    <div class="heading">
        <h3>@ViewBag.TieuDe</h3>
    </div>
    <div class="see">
        <p><a href="#">See all Products</a></p>
    </div>
    <div class="clear"></div>
</div>
<div class="section group">
    @foreach (var h in Model)
    {
        <div class="grid_1_of_4 images_1_of_4">
            <a href="#"><img src="~/images/@h.Anh" alt="@h.TenHang" /></a>
            <h2>@h.TenHang</h2>
            <div class="price-details">
                <div class="price-number">
                    <p><span class="rupees">@(h.Gia.HasValue ? h.Gia.Value.ToString("0") : "Chưa có giá")</span></p>
                </div>
                <div class="add-cart">
                    <h4><a href="#">Add to Cart</a></h4>
                </div>
                <div class="clear"></div>
            </div>
        </div>
    }
</div>
```

### 4.3. `Views/Shared/HoangBaTrung_Footer.cshtml`
```cshtml
<!-- Begin Footer -->
<div class="footer">
    <div class="wrap">
        <div class="section group">
            <div class="col_1_of_4 span_1_of_4">
                <h4>Information</h4>
                <ul>
                    <li><a href="#">About Us</a></li>
                    <li><a href="#">Customer Service</a></li>
                    <li><a href="#">Advanced Search</a></li>
                </ul>
            </div>
            <div class="col_1_of_4 span_1_of_4">
                <h4>Why buy from us</h4>
                <ul>
                    <li><a href="#">About Us</a></li>
                    <li><a href="#">Customer Service</a></li>
                    <li><a href="#">Privacy Policy</a></li>
                </ul>
            </div>
            <div class="col_1_of_4 span_1_of_4">
                <h4>My account</h4>
                <ul>
                    <li><a href="#">Sign In</a></li>
                    <li><a href="#">View Cart</a></li>
                    <li><a href="#">My Wishlist</a></li>
                </ul>
            </div>
            <div class="col_1_of_4 span_1_of_4">
                <h4>Contact</h4>
                <ul>
                    <li><span>+91-123-456789</span></li>
                    <li><span>+00-123-000000</span></li>
                </ul>
            </div>
        </div>
    </div>
</div>
<!-- End Footer -->
```

### 4.4. `Views/Shared/HoangBaTrung_Layout.cshtml` *(Kèm AJAX — Câu 4)*

> **Lưu ý quan trọng:** Không dùng `~/js/jquery-1.7.2.min.js` cũ trong template vì không tương thích với `_ValidationScriptsPartial`. Hãy dùng **`~/lib/jquery/dist/jquery.min.js`** có sẵn của ASP.NET Core 10.

```cshtml
<!DOCTYPE HTML>
<html lang="vi">
<head>
    <title>@(ViewData["Title"] ?? "Trang chủ") - Hoàng Bá Trung</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1">
    <link href="~/css/style.css" rel="stylesheet" type="text/css" media="all" />
</head>
<body>
    <!-- Begin Wrap -->
    <div class="wrap">
        <partial name="HoangBaTrung_Header" />

        <!-- Begin Main -->
        <div class="main">
            <div class="content" id="noidung">
                @RenderBody()
            </div>
        </div>
        <!-- End Main -->
    </div>
    <!-- End Wrap -->

    <partial name="HoangBaTrung_Footer" />

    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script>
        $(document).on('click', '.js-loaihang', function (e) {
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

## Bước 5: Code `Controllers/HomeController.cs` *(Câu 3, Câu 4, Câu 5.1)*

```csharp
using HoangBaTrung_241224491_2023.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace HoangBaTrung_241224491_2023.Controllers
{
    public class HomeController : Controller
    {
        private readonly QlhangHoaContext _db;

        public HomeController(QlhangHoaContext db)
        {
            _db = db;
        }

        // Câu 3: Hiển thị hàng hóa có Gia >= 100, tiêu đề là họ tên SV
        public IActionResult Index()
        {
            var ds = _db.HangHoas.Where(h => h.Gia != null && h.Gia >= 100).ToList();
            ViewBag.TieuDe = "Hoàng Bá Trung";
            return View(ds);
        }

        // Câu 4: Lọc hàng hóa theo loại bằng AJAX
        public IActionResult LocTheoLoai(int id)
        {
            var loai = _db.LoaiHangs.Find(id);
            var ds = _db.HangHoas.Where(h => h.MaLoai == id).ToList();
            ViewBag.TieuDe = loai?.TenLoai;
            return PartialView("HoangBaTrung_MainContent", ds);
        }

        // Câu 5.1: Hiển thị form thêm hàng hóa mới
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.DsLoaiHang = new SelectList(_db.LoaiHangs.ToList(), "MaLoai", "TenLoai");
            return View();
        }

        // Câu 5.1 + 5.2: Lưu hàng hóa mới & kiểm tra hợp lệ
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(HangHoa hh)
        {
            ModelState.Remove("MaLoaiNavigation");
            if (ModelState.IsValid)
            {
                _db.HangHoas.Add(hh);
                _db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DsLoaiHang = new SelectList(_db.LoaiHangs.ToList(), "MaLoai", "TenLoai", hh.MaLoai);
            return View(hh);
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

## Bước 6: Tạo các View trong `Views/Home/`

### 6.1. `Views/Home/Index.cshtml`
```cshtml
@model IEnumerable<HangHoa>
@{
    ViewData["Title"] = "Trang chủ";
    Layout = "HoangBaTrung_Layout";
}

<partial name="HoangBaTrung_MainContent" model="Model" />
```

### 6.2. `Views/Home/Create.cshtml`
```cshtml
@model HangHoa
@{
    ViewData["Title"] = "Thêm Hàng Hóa Mới";
    Layout = "HoangBaTrung_Layout";
}

<div class="content_top">
    <div class="heading">
        <h3>Hoàng Bá Trung - Thêm Hàng Hóa Mới</h3>
    </div>
    <div class="clear"></div>
</div>

<div class="form-box">
    <form asp-action="Create" method="post">
        <div asp-validation-summary="ModelOnly" class="text-danger"></div>

        <div class="form-group">
            <label asp-for="MaLoai">MaLoai</label>
            <select asp-for="MaLoai" asp-items="ViewBag.DsLoaiHang" class="form-control"></select>
            <span asp-validation-for="MaLoai" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="TenHang">TenHang</label>
            <input asp-for="TenHang" type="text" class="form-control" />
            <span asp-validation-for="TenHang" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="Gia">Gia</label>
            <input asp-for="Gia" type="text" class="form-control" />
            <span asp-validation-for="Gia" class="text-danger"></span>
        </div>

        <div class="form-group">
            <label asp-for="Anh">Anh</label>
            <input asp-for="Anh" type="text" class="form-control" />
            <span asp-validation-for="Anh" class="text-danger"></span>
        </div>

        <button type="submit" class="btn-submit">Create</button>
    </form>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

### 6.3. Bổ sung CSS Form vào cuối `wwwroot/css/style.css`
```css
.form-box {
    max-width: 420px;
    padding: 20px 0;
}

.form-group {
    margin-bottom: 14px;
}

.form-group label {
    display: block;
    font-size: 14px;
    margin-bottom: 4px;
    font-weight: bold;
}

.form-control {
    width: 100%;
    padding: 7px 10px;
    border: 1px solid #ccc;
    box-sizing: border-box;
}

.text-danger {
    color: #d93025;
    font-size: 13px;
    display: block;
    margin-top: 4px;
}

.btn-submit {
    background: #CD1F25;
    color: #fff;
    border: none;
    padding: 8px 18px;
    cursor: pointer;
    font-weight: bold;
}

.btn-submit:hover {
    background: #292929;
}
```

---

## Bước 7: Checklist kiểm thử trước khi nộp bài

- [ ] **Câu 1:** Chạy **F5**, mở DevTools (**F12 → Network**) kiểm tra không có lỗi 404 ở `style.css`, `images/*`, `font/*`, `jquery.min.js`.
- [ ] **Câu 2:** Thanh menu hiển thị đúng: `HOÀNG BÁ TRUNG`, `ĐIỆN THOẠI`, `MÁY TÍNH`, `TI VI`, `TỦ LẠNH`, `PHỤ KIỆN`, `THÊM HÀNG HÓA`.
- [ ] **Câu 3:** Trang chủ có tiêu đề `HOÀNG BÁ TRUNG`, chỉ hiển thị các hàng hóa có `Gia >= 100` (bản ghi `Honda` có `Gia = NULL` không hiện ở trang chủ).
- [ ] **Câu 4:** Bấm vào từng loại hàng trên menu → nội dung `#noidung` cập nhật ngay bằng **AJAX** không reload trang, thanh menu đổi trạng thái `.active`.
- [ ] **Câu 5:** Vào `THÊM HÀNG HÓA`:
  - Thử nhập `Gia = 50` hoặc `Gia = 6000` → hiện thông báo lỗi `100 đến 5000`.
  - Thử nhập `Anh = test.webp` hoặc `test.bmp` → hiện thông báo lỗi đuôi ảnh `.jpg | .png | .gif | .tiff`.
  - Nhập dữ liệu hợp lệ (ví dụ `Điện thoại`, `iPhone 16`, `2500`, `Img_2.jpg`) → bấm **Create** → chuyển về trang chủ và thấy hàng hóa mới hiển thị.

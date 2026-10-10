using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TvcLesson08Df.Models;

var builder = WebApplication.CreateBuilder(args);

// Lấy chuỗi kết nối từ appsettings.json
var connectionString = builder.Configuration.GetConnectionString("BookStoreConnection")
    ?? "Server=(localdb)\\MSSQLLocalDB;Database=BookStore;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

// Tự động khởi động và phân giải Named Pipe cho (localdb) trên môi trường Windows ARM64 (tránh lỗi DLL x64)
if (connectionString.Contains("(localdb)", StringComparison.OrdinalIgnoreCase))
{
    try
    {
        // 1. Khởi động instance nếu đang ở trạng thái Stopped
        using (var startProc = Process.Start(new ProcessStartInfo("sqllocaldb", "start MSSQLLocalDB")
        {
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }))
        {
            startProc?.WaitForExit(3000);
        }

        // 2. Lấy thông tin pipe name đang chạy
        using (var infoProc = Process.Start(new ProcessStartInfo("sqllocaldb", "info MSSQLLocalDB")
        {
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        }))
        {
            if (infoProc != null)
            {
                var output = infoProc.StandardOutput.ReadToEnd();
                infoProc.WaitForExit(3000);
                var match = Regex.Match(output, @"np:\\\\\.\\pipe\\[^\r\n]+");
                if (match.Success)
                {
                    var pipe = match.Value.Trim();
                    connectionString = Regex.Replace(
                        connectionString,
                        @"(Server|Data Source)\s*=\s*\(localdb\)\\[^;]+",
                        $"Server={pipe}",
                        RegexOptions.IgnoreCase);
                }
            }
        }
    }
    catch
    {
        // Giữ nguyên chuỗi kết nối mặc định nếu không chạy được sqllocaldb
    }
}

// Đăng ký BookStoreContext vào DI container
builder.Services.AddDbContext<BookStoreContext>(options =>
    options.UseSqlServer(connectionString));

// Đăng ký dịch vụ MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

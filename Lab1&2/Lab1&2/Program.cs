using Microsoft.EntityFrameworkCore;
using MyWebApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Cấu hình Database: Hỗ trợ linh hoạt cả SQL Server và InMemory Database
var useInMemory = builder.Configuration.GetValue<bool>("UseInMemoryDatabase", false);
var connectionString = builder.Configuration.GetConnectionString("SchoolContext");

if (useInMemory || string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseInMemoryDatabase("SchoolLabDb"));
}
else
{
    builder.Services.AddDbContext<SchoolContext>(options =>
        options.UseSqlServer(connectionString));
}

var app = builder.Build();

// Tự động khởi tạo DB và nạp Seed Data mẫu
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        DbInitializer.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Lỗi khi khởi tạo CSDL.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Learner}/{action=Index}/{id?}");

app.Run();

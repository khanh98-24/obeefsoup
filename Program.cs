using OBeefSoup.Services;
using OBeefSoup.Data;
using Microsoft.EntityFrameworkCore;
  
var builder = WebApplication.CreateBuilder(args);

// =======================
// ADD SERVICES
// =======================

// MVC
builder.Services.AddControllersWithViews();

// Database (PostgreSQL Railway)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

// Session (shopping cart)
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// HttpContext access
builder.Services.AddHttpContextAccessor();

// Custom services
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// =======================
// MIDDLEWARE PIPELINE
// =======================

// Production error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ⚠ Railway đã có HTTPS → tránh lỗi redirect loop
// app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();     // phải trước MapControllerRoute
app.UseAuthorization();

// =======================
// ROUTING
// =======================

// Areas (Admin)
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

// Default route
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// =======================
// RUN APP (Railway PORT)
// =======================

var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";

app.Run($"http://0.0.0.0:{port}");
using OBeefSoup.Services;
using OBeefSoup.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// =======================
// ADD SERVICES
// =======================

// MVC
builder.Services.AddControllersWithViews();

// ✅ Connection string (Azure sẽ đọc từ Environment Variables)
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? Environment.GetEnvironmentVariable("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Session
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddHttpContextAccessor();

// Services
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<MenuService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();

// =======================
// MIDDLEWARE
// =======================

// Production error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Azure dùng HTTPS sẵn → luôn bật
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

// =======================
// ROUTES
// =======================

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// =======================
// RUN
// =======================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}
app.Run();
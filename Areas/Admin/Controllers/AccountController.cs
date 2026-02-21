using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Filters;
using OBeefSoup.Models;
using OBeefSoup.Services;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        // GET: /Admin/Account/Login → redirect về trang login chung
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            // Nếu đã đăng nhập rồi thì redirect Dashboard
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(AuthService.SESSION_USER_ID)))
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            return RedirectToAction("Login", "CustomerAccount", new { area = "", returnUrl });
        }

        // POST: /Admin/Account/Login → redirect về trang login chung
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string? email, string? username, string password, string? returnUrl = null)
        {
            return RedirectToAction("Login", "CustomerAccount", new { area = "" });
        }

        // GET: /Admin/Account/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account", new { area = "Admin" });
        }

        // GET: /Admin/Account/Users  (chỉ Admin)
        [AdminOnlyFilter]
        public async Task<IActionResult> Users()
        {
            var users = await _authService.GetAllUsersAsync();
            return View(users);
        }

        // GET: /Admin/Account/Register  (chỉ Admin)
        [AdminOnlyFilter]
        public IActionResult Register()
        {
            return View();
        }

        // POST: /Admin/Account/Register  (chỉ Admin)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnlyFilter]
        public async Task<IActionResult> Register(string username, string email, string fullName, string password, string confirmPassword, string role)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            if (await _authService.EmailExistsAsync(email))
            {
                ViewBag.Error = "Email đã tồn tại.";
                return View();
            }

            if (role != "Admin" && role != "Manager")
                role = "Manager";

            await _authService.CreateUserAsync(username, email, password, role, fullName);

            TempData["Success"] = $"Tạo tài khoản '{username}' thành công!";
            return RedirectToAction("Users");
        }

        // POST: /Admin/Account/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnlyFilter]
        public async Task<IActionResult> Delete(int id)
        {
            // Không cho phép xóa chính mình
            var currentId = HttpContext.Session.GetString(AuthService.SESSION_USER_ID);
            if (currentId == id.ToString())
            {
                TempData["Error"] = "Không thể xóa tài khoản đang đăng nhập.";
                return RedirectToAction("Users");
            }

            await _authService.DeleteUserAsync(id);
            TempData["Success"] = "Đã xóa tài khoản.";
            return RedirectToAction("Users");
        }

        // POST: /Admin/Account/ToggleStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AdminOnlyFilter]
        public async Task<IActionResult> ToggleStatus(int id)
        {
            var currentId = HttpContext.Session.GetString(AuthService.SESSION_USER_ID);
            if (currentId == id.ToString())
            {
                TempData["Error"] = "Không thể vô hiệu hóa tài khoản đang đăng nhập.";
                return RedirectToAction("Users");
            }

            await _authService.ToggleUserStatusAsync(id);
            TempData["Success"] = "Đã cập nhật trạng thái tài khoản.";
            return RedirectToAction("Users");
        }

        // GET: /Admin/Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // GET: /Admin/Account/ResetSeed  (TẠM - reset & debug)
        public async Task<IActionResult> ResetSeed()
        {
            var context = HttpContext.RequestServices.GetRequiredService<OBeefSoup.Data.ApplicationDbContext>();
            
            var newAdminHash = AuthService.HashPassword("Admin@123");
            var newManagerHash = AuthService.HashPassword("Manager@123");
            
            // Update trực tiếp bằng raw SQL
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE AdminUsers SET PasswordHash = {0}, Email = {1} WHERE Username = {2}",
                newAdminHash, "admin@obeefsoup.vn", "admin");
            await context.Database.ExecuteSqlRawAsync(
                "UPDATE AdminUsers SET PasswordHash = {0}, Email = {1} WHERE Username = {2}",
                newManagerHash, "manager@obeefsoup.vn", "manager");
            
            // Debug: đọc lại từ DB
            var users = await context.AdminUsers.ToListAsync();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== RESET DONE ===");
            sb.AppendLine();
            foreach (var u in users)
            {
                sb.AppendLine($"ID: {u.Id} | Username: {u.Username} | Email: {u.Email}");
                sb.AppendLine($"  DB Hash: {u.PasswordHash}");
                sb.AppendLine($"  IsActive: {u.IsActive} | Role: {u.Role}");
                sb.AppendLine();
            }
            sb.AppendLine($"Runtime Hash('Admin@123') = {newAdminHash}");
            sb.AppendLine($"Runtime Hash('Manager@123') = {newManagerHash}");
            sb.AppendLine();
            sb.AppendLine("LOGIN: admin@obeefsoup.vn / Admin@123");
            sb.AppendLine("LOGIN: manager@obeefsoup.vn / Manager@123");
            
            return Content(sb.ToString());
        }
    }
}

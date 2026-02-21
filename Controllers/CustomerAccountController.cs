using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Services;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Controllers
{
    public class CustomerAccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        // Session keys cho khách hàng
        private const string SESS_CUSTOMER_ID   = "CustomerId";
        private const string SESS_CUSTOMER_NAME = "CustomerName";
        private const string SESS_CUSTOMER_EMAIL = "CustomerEmail";

        public CustomerAccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /CustomerAccount/Register
        [HttpGet]
        public IActionResult Register()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SESS_CUSTOMER_ID)))
                return RedirectToAction("Index", "Home");
            return View();
        }

        // POST: /CustomerAccount/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(string fullName, string email, string phone,
                                                   string address, string password, string confirmPassword)
        {
            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
                string.IsNullOrWhiteSpace(phone) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng điền đầy đủ thông tin bắt buộc.";
                return View();
            }

            if (password != confirmPassword)
            {
                ViewBag.Error = "Mật khẩu xác nhận không khớp.";
                return View();
            }

            if (password.Length < 6)
            {
                ViewBag.Error = "Mật khẩu phải có ít nhất 6 ký tự.";
                return View();
            }

            // Kiểm tra email đã tồn tại chưa
            var exists = await _context.Customers.AnyAsync(c => c.Email == email && c.PasswordHash != null);
            if (exists)
            {
                ViewBag.Error = "Email này đã được đăng ký. Vui lòng đăng nhập hoặc dùng email khác.";
                return View();
            }

            var customer = new Customer
            {
                FullName    = fullName,
                Email       = email,
                Phone       = phone,
                Address     = address ?? "",
                PasswordHash = AuthService.HashPassword(password),
                CreatedDate = DateTime.Now
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Tự động đăng nhập sau khi đăng ký
            HttpContext.Session.SetString(SESS_CUSTOMER_ID,    customer.CustomerId.ToString());
            HttpContext.Session.SetString(SESS_CUSTOMER_NAME,  customer.FullName);
            HttpContext.Session.SetString(SESS_CUSTOMER_EMAIL, customer.Email);

            TempData["Welcome"] = $"Chào mừng {customer.FullName} đến với O'BeefSoup!";
            return RedirectToAction("Index", "Home");
        }

        // GET: /CustomerAccount/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString(SESS_CUSTOMER_ID)))
                return RedirectToAction("Index", "Home");
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // POST: /CustomerAccount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password, string? returnUrl = null)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "Vui lòng nhập email và mật khẩu.";
                return View();
            }

            var hash = AuthService.HashPassword(password);

            // 1) Kiểm tra AdminUsers trước
            var adminUser = await _context.AdminUsers
                .FirstOrDefaultAsync(u => (u.Email == email || u.Username == email) && u.IsActive);

            if (adminUser != null && AuthService.VerifyPassword(password, adminUser.PasswordHash))
            {
                // Set session Admin
                HttpContext.Session.SetString(AuthService.SESSION_USER_ID, adminUser.Id.ToString());
                HttpContext.Session.SetString(AuthService.SESSION_USER_ROLE, adminUser.Role);
                HttpContext.Session.SetString(AuthService.SESSION_USER_NAME, adminUser.Username);
                HttpContext.Session.SetString(AuthService.SESSION_FULL_NAME, adminUser.FullName);
                // Cũng set session customer để navbar hiện tên
                HttpContext.Session.SetString(SESS_CUSTOMER_ID, "admin_" + adminUser.Id);
                HttpContext.Session.SetString(SESS_CUSTOMER_NAME, adminUser.FullName);
                HttpContext.Session.SetString(SESS_CUSTOMER_EMAIL, adminUser.Email);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            // 2) Kiểm tra Customers
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email && c.PasswordHash == hash);

            if (customer != null)
            {
                HttpContext.Session.SetString(SESS_CUSTOMER_ID, customer.CustomerId.ToString());
                HttpContext.Session.SetString(SESS_CUSTOMER_NAME, customer.FullName);
                HttpContext.Session.SetString(SESS_CUSTOMER_EMAIL, customer.Email);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Email hoặc mật khẩu không đúng.";
            return View();
        }

        // GET: /CustomerAccount/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using OBeefSoup.Models;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Controllers
{
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(string comment, int rating)
        {
            var customerIdStr = HttpContext.Session.GetString("CustomerId");
            if (string.IsNullOrEmpty(customerIdStr))
            {
                return Json(new { success = false, message = "Bạn cần đăng nhập để gửi đánh giá." });
            }

            var customerName = HttpContext.Session.GetString("CustomerName") ?? "Khách hàng";

            // Nếu admin đang giả lập user
            int customerId;
            if (customerIdStr.StartsWith("admin_"))
            {
                // Admin ID (có thể xử lý riêng hoặc gán một ID mặc định cho admin reviews)
                customerId = 1; // Mặc định cho admin
            }
            else
            {
                customerId = int.Parse(customerIdStr);
            }

            var testimonial = new Testimonial
            {
                CustomerId = customerId,
                CustomerName = customerName,
                Comment = comment,
                Rating = rating,
                Date = DateTime.Now,
                IsApproved = false // Chờ duyệt
            };

            _context.Testimonials.Add(testimonial);
            await _context.SaveChangesAsync();

            return Json(new { success = true, message = "Cảm ơn bạn! Đánh giá của bạn đã được gửi và đang chờ quản trị viên phê duyệt." });
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using OBeefSoup.Models;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Services;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TestimonialsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestimonialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Kiểm tra quyền Admin đơn giản (giả sử đã có hệ thống auth)
        private bool IsAdmin()
        {
            return HttpContext.Session.GetString(AuthService.SESSION_USER_ROLE) == "Admin" || 
                   HttpContext.Session.GetString(AuthService.SESSION_USER_ROLE) == "Manager";
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin()) return RedirectToAction("Login", "Account");

            var testimonials = await _context.Testimonials
                .OrderByDescending(t => t.Date)
                .ToListAsync();

            return View(testimonials);
        }

        [HttpPost]
        public async Task<IActionResult> ToggleApproval(int id)
        {
            if (!IsAdmin()) return Unauthorized();

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                testimonial.IsApproved = !testimonial.IsApproved;
                await _context.SaveChangesAsync();
                return Json(new { success = true, isApproved = testimonial.IsApproved });
            }
            return Json(new { success = false });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin()) return Unauthorized();

            var testimonial = await _context.Testimonials.FindAsync(id);
            if (testimonial != null)
            {
                _context.Testimonials.Remove(testimonial);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}

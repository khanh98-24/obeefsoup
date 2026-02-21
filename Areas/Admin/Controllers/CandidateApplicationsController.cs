using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class CandidateApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CandidateApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/CandidateApplications
        public async Task<IActionResult> Index(int? jobPostingId)
        {
            var query = _context.CandidateApplications
                .Include(c => c.JobPosting)
                .AsQueryable();

            if (jobPostingId.HasValue)
            {
                query = query.Where(c => c.JobPostingId == jobPostingId.Value);
                ViewBag.JobTitle = (await _context.JobPostings.FindAsync(jobPostingId))?.Title;
            }

            var applications = await query
                .OrderByDescending(c => c.AppliedDate)
                .ToListAsync();

            return View(applications);
        }

        // GET: Admin/CandidateApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var application = await _context.CandidateApplications
                .Include(c => c.JobPosting)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (application == null) return NotFound();

            return View(application);
        }

        // POST: Admin/CandidateApplications/UpdateStatus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, ApplicationStatus status)
        {
            var application = await _context.CandidateApplications.FindAsync(id);
            if (application == null) return NotFound();

            application.Status = status;
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn ứng tuyển thành công!";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // POST: Admin/CandidateApplications/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.CandidateApplications.FindAsync(id);
            if (application != null)
            {
                _context.CandidateApplications.Remove(application);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa đơn ứng tuyển thành công!";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

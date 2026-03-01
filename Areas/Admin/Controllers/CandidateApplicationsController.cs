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
            if (application == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                    return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển!" });
                return NotFound();
            }

            application.Status = status;
            await _context.SaveChangesAsync();

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                var statusText = status switch
                {
                    ApplicationStatus.Pending      => "Chờ duyệt",
                    ApplicationStatus.Reviewed     => "Đã xem hồ sơ",
                    ApplicationStatus.Interviewing => "Đang phỏng vấn",
                    ApplicationStatus.Hired        => "Đã tuyển",
                    ApplicationStatus.Rejected     => "Từ chối",
                    _                              => status.ToString()
                };
                var badgeClass = status switch
                {
                    ApplicationStatus.Pending      => "bg-warning-subtle text-warning border-warning-subtle",
                    ApplicationStatus.Reviewed     => "bg-info-subtle text-info border-info-subtle",
                    ApplicationStatus.Interviewing => "bg-primary-subtle text-primary border-primary-subtle",
                    ApplicationStatus.Hired        => "bg-success-subtle text-success border-success-subtle",
                    ApplicationStatus.Rejected     => "bg-danger-subtle text-danger border-danger-subtle",
                    _                              => "bg-light text-muted"
                };
                return Json(new { success = true, message = "Cập nhật trạng thái thành công!", statusText, badgeClass });
            }

            TempData["SuccessMessage"] = "Cập nhật trạng thái đơn ứng tuyển thành công!";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        // AJAX: Delete candidate application and return JSON
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAjax(int id)
        {
            var application = await _context.CandidateApplications.FindAsync(id);
            if (application == null) return Json(new { success = false, message = "Không tìm thấy đơn ứng tuyển!" });

            _context.CandidateApplications.Remove(application);
            await _context.SaveChangesAsync();
            return Json(new { success = true, message = "Xóa đơn ứng tuyển thành công!" });
        }

        // AJAX: Get partial view for candidate applications table
        [HttpGet]
        public async Task<IActionResult> IndexPartial(int? jobPostingId)
        {
            var query = _context.CandidateApplications.Include(c => c.JobPosting).AsQueryable();
            if (jobPostingId.HasValue)
            {
                query = query.Where(c => c.JobPostingId == jobPostingId.Value);
                ViewBag.JobTitle = (await _context.JobPostings.FindAsync(jobPostingId))?.Title;
            }
            var apps = await query.OrderByDescending(c => c.AppliedDate).ToListAsync();
            return PartialView("_CandidateApplicationsTablePartial", apps);
        }
    }
}

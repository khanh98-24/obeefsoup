using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;

namespace OBeefSoup.Controllers
{
    public class RecruitmentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RecruitmentController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var jobPostings = await _context.JobPostings
                .Include(j => j.StoreLocation)
                .Where(j => j.IsActive && (j.Deadline == null || j.Deadline >= DateTime.Now))
                .OrderByDescending(j => j.CreatedDate)
                .ToListAsync();

            return View(jobPostings);
        }

        // GET: Recruitment/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var job = await _context.JobPostings
                .Include(j => j.StoreLocation)
                .FirstOrDefaultAsync(j => j.Id == id);
                
            if (job == null || !job.IsActive) return NotFound();

            return View(job);
        }

        // POST: Recruitment/Apply
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Apply(CandidateApplication application)
        {
            if (ModelState.IsValid)
            {
                application.AppliedDate = DateTime.Now;
                application.Status = ApplicationStatus.Pending;
                
                _context.CandidateApplications.Add(application);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Cảm ơn bạn đã quan tâm! Hồ sơ của bạn đã được gửi thành công. Chúng tôi sẽ liên hệ với bạn sớm nhất có thể.";
                return RedirectToAction(nameof(Details), new { id = application.JobPostingId });
            }

            // If we got here, something failed, redisplay form
            var job = await _context.JobPostings.FindAsync(application.JobPostingId);
            return View("Details", job);
        }
    }
}

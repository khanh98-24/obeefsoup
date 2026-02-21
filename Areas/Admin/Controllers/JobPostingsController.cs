using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class JobPostingsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public JobPostingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/JobPostings
        public async Task<IActionResult> Index()
        {
            var jobPostings = await _context.JobPostings
                .Include(j => j.StoreLocation)
                .Include(j => j.Applications)
                .OrderByDescending(j => j.CreatedDate)
                .ToListAsync();
            return View(jobPostings);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.StoreLocationId = await _context.StoreLocations
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Requirements,Benefits,SalaryRange,Deadline,IsActive,StoreLocationId")] JobPosting jobPosting)
        {
            if (ModelState.IsValid)
            {
                jobPosting.CreatedDate = DateTime.Now;
                _context.Add(jobPosting);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm vị trí tuyển dụng thành công!";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StoreLocationId = await _context.StoreLocations
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();
            return View(jobPosting);
        }

        // GET: Admin/JobPostings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting == null) return NotFound();

            ViewBag.StoreLocationId = await _context.StoreLocations
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();
            return View(jobPosting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Requirements,Benefits,SalaryRange,Deadline,IsActive,CreatedDate,StoreLocationId")] JobPosting jobPosting)
        {
            if (id != jobPosting.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobPosting);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật vị trí tuyển dụng thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobPostingExists(jobPosting.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.StoreLocationId = await _context.StoreLocations
                .Select(s => new { s.Id, s.Name })
                .ToListAsync();
            return View(jobPosting);
        }

        // POST: Admin/JobPostings/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var jobPosting = await _context.JobPostings.FindAsync(id);
            if (jobPosting != null)
            {
                // Optionally check if there are applications before deleting, or use cascade/set null
                _context.JobPostings.Remove(jobPosting);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa vị trí tuyển dụng thành công!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool JobPostingExists(int id)
        {
            return _context.JobPostings.Any(e => e.Id == id);
        }
    }
}

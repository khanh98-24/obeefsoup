using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class StoreLocationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StoreLocationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/StoreLocations
        public async Task<IActionResult> Index()
        {
            var stores = await _context.StoreLocations
                .OrderBy(s => s.Id)
                .ToListAsync();
            return View(stores);
        }

        // GET: Admin/StoreLocations/Create
        public IActionResult Create()
        {
            return View(new StoreLocation { IsActive = true, OpeningHours = "7:00 - 22:00 (Hàng ngày)" });
        }

        // POST: Admin/StoreLocations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StoreLocation model)
        {
            model.MapUrl = ExtractMapUrl(model.MapUrl);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.StoreLocations.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm cơ sở thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Admin/StoreLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var store = await _context.StoreLocations.FindAsync(id);
            if (store == null) return NotFound();
            return View(store);
        }

        // POST: Admin/StoreLocations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, StoreLocation model)
        {
            if (id != model.Id) return NotFound();
            model.MapUrl = ExtractMapUrl(model.MapUrl);
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.StoreLocations.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // POST: Admin/StoreLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var store = await _context.StoreLocations.FindAsync(id);
                if (store != null)
                {
                    _context.StoreLocations.Remove(store);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa cơ sở thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/StoreLocations/ToggleActive/5
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var store = await _context.StoreLocations.FindAsync(id);
            if (store != null)
            {
                store.IsActive = !store.IsActive;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        /// Extracts the embed URL from a full iframe tag if pasted, or returns as-is if already a URL
        /// </summary>
        private static string ExtractMapUrl(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return string.Empty;
            input = input.Trim();
            // If it's a full <iframe> tag, extract the src value
            if (input.StartsWith("<iframe", StringComparison.OrdinalIgnoreCase))
            {
                var srcMatch = System.Text.RegularExpressions.Regex.Match(
                    input, @"src=""([^""]+)""", 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (srcMatch.Success)
                    return srcMatch.Groups[1].Value;
            }
            return input;
        }
    }
}

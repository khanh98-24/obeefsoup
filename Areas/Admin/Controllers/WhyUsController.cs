using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Filters;
using OBeefSoup.Services;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class WhyUsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public WhyUsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/WhyUs
        public async Task<IActionResult> Index()
        {
            var items = await _context.WhyUsItems
                .OrderBy(w => w.DisplayOrder)
                .ToListAsync();
            return View(items);
        }

        // GET: Admin/WhyUs/Create
        public IActionResult Create()
        {
            return View(new WhyUsItem { DisplayOrder = 1, IsActive = true, Size = "small" });
        }

        // POST: Admin/WhyUs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WhyUsItem model, string? CroppedImage)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (!string.IsNullOrEmpty(CroppedImage))
                    {
                        model.BackgroundImageUrl = await _imageService.SaveImageFromBase64Async(CroppedImage, "whyus");
                    }

                    model.CreatedDate = DateTime.Now;
                    _context.WhyUsItems.Add(model);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Thêm mục thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // GET: Admin/WhyUs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var item = await _context.WhyUsItems.FindAsync(id);
            if (item == null) return NotFound();
            return View(item);
        }

        // POST: Admin/WhyUs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WhyUsItem model, string? CroppedImage)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var item = await _context.WhyUsItems.FindAsync(id);
                    if (item == null) return NotFound();

                    if (!string.IsNullOrEmpty(CroppedImage))
                    {
                        item.BackgroundImageUrl = await _imageService.SaveImageFromBase64Async(CroppedImage, "whyus");
                    }

                    item.Title = model.Title;
                    item.Description = model.Description;
                    item.IconClass = model.IconClass;
                    item.Size = model.Size;
                    item.DisplayOrder = model.DisplayOrder;
                    item.IsActive = model.IsActive;
                    item.UpdatedDate = DateTime.Now;

                    _context.Update(item);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.WhyUsItems.Any(e => e.Id == id)) return NotFound();
                    else throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        // POST: Admin/WhyUs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var item = await _context.WhyUsItems.FindAsync(id);
                if (item != null)
                {
                    if (!string.IsNullOrEmpty(item.BackgroundImageUrl))
                    {
                        _imageService.DeleteImage(item.BackgroundImageUrl);
                    }
                    _context.WhyUsItems.Remove(item);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Xóa mục thành công!";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa: " + ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/WhyUs/ToggleActive/5
        [HttpPost]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var item = await _context.WhyUsItems.FindAsync(id);
            if (item != null)
            {
                item.IsActive = !item.IsActive;
                item.UpdatedDate = DateTime.Now;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

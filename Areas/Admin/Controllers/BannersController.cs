using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;
using OBeefSoup.Services;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class BannersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BannersController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/Banners
        public async Task<IActionResult> Index()
        {
            var banners = await _context.Banners
                .OrderBy(b => b.DisplayOrder)
                .ThenByDescending(b => b.CreatedDate)
                .ToListAsync();

            return View(banners);
        }

        // GET: Admin/Banners/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Banners/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BannerViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var banner = new Banner
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Link = model.Link,
                        DisplayOrder = model.DisplayOrder,
                        IsActive = model.IsActive,
                        CreatedDate = DateTime.Now
                    };

                    // Handle image upload
                    if (model.ImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.ImageFile))
                        {
                            banner.ImageUrl = await _imageService.SaveImageAsync(model.ImageFile, "banners");
                        }
                        else
                        {
                            ModelState.AddModelError("ImageFile", "File ảnh không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF (tối đa 5MB)");
                            return View(model);
                        }
                    }
                    else
                    {
                        banner.ImageUrl = "https://placehold.co/1200x400/8B0000/FFF?text=No+Banner+Image";
                    }

                    _context.Banners.Add(banner);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm banner thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm banner: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Admin/Banners/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var banner = await _context.Banners.FindAsync(id);
            if (banner == null)
                return NotFound();

            var model = new BannerViewModel
            {
                Id = banner.Id,
                Title = banner.Title,
                Description = banner.Description,
                Link = banner.Link,
                DisplayOrder = banner.DisplayOrder,
                IsActive = banner.IsActive,
                ImageUrl = banner.ImageUrl,
                CreatedDate = banner.CreatedDate,
                UpdatedDate = banner.UpdatedDate
            };

            return View(model);
        }

        // POST: Admin/Banners/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BannerViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var banner = await _context.Banners.FindAsync(id);
                    if (banner == null)
                        return NotFound();

                    // Update properties
                    banner.Title = model.Title;
                    banner.Description = model.Description;
                    banner.Link = model.Link;
                    banner.DisplayOrder = model.DisplayOrder;
                    banner.IsActive = model.IsActive;
                    banner.UpdatedDate = DateTime.Now;

                    // Handle image upload
                    if (!string.IsNullOrEmpty(model.CroppedImageData))
                    {
                        // Ảnh đã crop từ client
                        if (!string.IsNullOrEmpty(banner.ImageUrl))
                            _imageService.DeleteImage(banner.ImageUrl);

                        banner.ImageUrl = await _imageService.SaveImageFromBase64Async(model.CroppedImageData, "banners");
                    }
                    else if (model.ImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.ImageFile))
                        {
                            if (!string.IsNullOrEmpty(banner.ImageUrl))
                                _imageService.DeleteImage(banner.ImageUrl);

                            banner.ImageUrl = await _imageService.SaveImageAsync(model.ImageFile, "banners");
                        }
                        else
                        {
                            ModelState.AddModelError("ImageFile", "File ảnh không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF (tối đa 5MB)");
                            return View(model);
                        }
                    }

                    _context.Update(banner);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật banner thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BannerExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật banner: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Admin/Banners/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var banner = await _context.Banners.FirstOrDefaultAsync(m => m.Id == id);

            if (banner == null)
                return NotFound();

            return View(banner);
        }

        // POST: Admin/Banners/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var banner = await _context.Banners.FindAsync(id);
                if (banner == null)
                    return NotFound();

                // Delete image from server
                if (!string.IsNullOrEmpty(banner.ImageUrl))
                {
                    _imageService.DeleteImage(banner.ImageUrl);
                }

                _context.Banners.Remove(banner);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa banner thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa banner: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool BannerExists(int id)
        {
            return _context.Banners.Any(e => e.Id == id);
        }
    }
}

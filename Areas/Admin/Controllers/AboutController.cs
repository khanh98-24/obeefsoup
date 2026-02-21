using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Filters;
using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;
using OBeefSoup.Services;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class AboutController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public AboutController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/About
        public async Task<IActionResult> Index()
        {
            var settings = await _context.SiteSettings
                .Where(s => s.Key.StartsWith("About"))
                .ToDictionaryAsync(s => s.Key, s => s.Value);

            var features = await _context.AboutFeatures
                .OrderBy(f => f.DisplayOrder)
                .ToListAsync();

            ViewBag.Features = features;

            var model = new AboutSectionViewModel
            {
                Title1 = settings.GetValueOrDefault("AboutTitle1", ""),
                Title2 = settings.GetValueOrDefault("AboutTitle2", "O' BeefSoup"),
                Subtitle = settings.GetValueOrDefault("AboutSubtitle", "CÂU CHUYỆN THƯƠNG HIỆU"),
                Description = settings.GetValueOrDefault("AboutDescription", ""),
                ImageUrl = settings.GetValueOrDefault("AboutImageUrl", "")
            };

            return View(model);
        }

        // POST: Admin/About/UpdateSettings
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSettings(AboutSectionViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var settings = await _context.SiteSettings
                        .Where(s => s.Key.StartsWith("About"))
                        .ToListAsync();

                    UpdateSetting(settings, "AboutTitle1", model.Title1);
                    UpdateSetting(settings, "AboutTitle2", model.Title2);
                    UpdateSetting(settings, "AboutSubtitle", model.Subtitle);
                    UpdateSetting(settings, "AboutDescription", model.Description);

                    // Handle image upload
                    if (!string.IsNullOrEmpty(model.CroppedImageData))
                    {
                        var oldUrl = settings.FirstOrDefault(s => s.Key == "AboutImageUrl")?.Value;
                        if (!string.IsNullOrEmpty(oldUrl) && !oldUrl.Contains("placehold.co") && !oldUrl.Contains("gioithieu.jpg"))
                            _imageService.DeleteImage(oldUrl);

                        var newUrl = await _imageService.SaveImageFromBase64Async(model.CroppedImageData, "about");
                        UpdateSetting(settings, "AboutImageUrl", newUrl);
                    }
                    else if (model.ImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.ImageFile))
                        {
                            var oldUrl = settings.FirstOrDefault(s => s.Key == "AboutImageUrl")?.Value;
                            if (!string.IsNullOrEmpty(oldUrl) && !oldUrl.Contains("placehold.co") && !oldUrl.Contains("gioithieu.jpg"))
                                _imageService.DeleteImage(oldUrl);

                            var newUrl = await _imageService.SaveImageAsync(model.ImageFile, "about");
                            UpdateSetting(settings, "AboutImageUrl", newUrl);
                        }
                    }

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thông tin Giới thiệu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật: " + ex.Message);
                }
            }

            ViewBag.Features = await _context.AboutFeatures.OrderBy(f => f.DisplayOrder).ToListAsync();
            return View("Index", model);
        }

        private void UpdateSetting(List<SiteSetting> settings, string key, string? value)
        {
            var setting = settings.FirstOrDefault(s => s.Key == key);
            if (setting != null)
            {
                setting.Value = value ?? "";
                setting.UpdatedDate = DateTime.Now;
            }
            else
            {
                _context.SiteSettings.Add(new SiteSetting
                {
                    Key = key,
                    Value = value ?? "",
                    IsActive = true,
                    CreatedDate = DateTime.Now
                });
            }
        }

        // Feature CRUD Actions

        // GET: Admin/About/CreateFeature
        public IActionResult CreateFeature()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateFeature(AboutFeatureViewModel model)
        {
            if (ModelState.IsValid)
            {
                var feature = new AboutFeature
                {
                    Title = model.Title,
                    IconClass = model.IconClass,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    CreatedDate = DateTime.Now
                };

                if (model.IconImageFile != null)
                {
                    if (_imageService.IsValidImage(model.IconImageFile))
                    {
                        feature.IconImageUrl = await _imageService.SaveImageAsync(model.IconImageFile, "icons");
                        feature.IconClass = null; // Prioritize image if uploaded
                    }
                }

                _context.AboutFeatures.Add(feature);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm thành phần thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Admin/About/EditFeature/5
        public async Task<IActionResult> EditFeature(int? id)
        {
            if (id == null) return NotFound();

            var feature = await _context.AboutFeatures.FindAsync(id);
            if (feature == null) return NotFound();

            var model = new AboutFeatureViewModel
            {
                Id = feature.Id,
                Title = feature.Title,
                IconClass = feature.IconClass,
                IconImageUrl = feature.IconImageUrl,
                DisplayOrder = feature.DisplayOrder,
                IsActive = feature.IsActive
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFeature(int id, AboutFeatureViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var feature = await _context.AboutFeatures.FindAsync(id);
                    if (feature == null) return NotFound();

                    feature.Title = model.Title;
                    feature.IconClass = model.IconClass;
                    feature.DisplayOrder = model.DisplayOrder;
                    feature.IsActive = model.IsActive;
                    feature.UpdatedDate = DateTime.Now;

                    if (model.IconImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.IconImageFile))
                        {
                            if (!string.IsNullOrEmpty(feature.IconImageUrl))
                                _imageService.DeleteImage(feature.IconImageUrl);

                            feature.IconImageUrl = await _imageService.SaveImageAsync(model.IconImageFile, "icons");
                            feature.IconClass = null;
                        }
                    }
                    else if (!string.IsNullOrEmpty(model.IconClass))
                    {
                        // If user switched back to icon class, delete image
                        if (!string.IsNullOrEmpty(feature.IconImageUrl))
                        {
                            _imageService.DeleteImage(feature.IconImageUrl);
                            feature.IconImageUrl = null;
                        }
                    }

                    _context.Update(feature);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Cập nhật thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi: " + ex.Message);
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFeature(int id)
        {
            var feature = await _context.AboutFeatures.FindAsync(id);
            if (feature != null)
            {
                _context.AboutFeatures.Remove(feature);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Đã xóa thành phần.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

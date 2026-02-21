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
    public class SettingsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public SettingsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/Settings
        public async Task<IActionResult> Index()
        {
            var settings = await _context.SiteSettings.ToListAsync();
            return View(settings);
        }

        // GET: Admin/Settings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var setting = await _context.SiteSettings.FindAsync(id);
            if (setting == null)
            {
                return NotFound();
            }

            // Load current opacity if editing image setting
            if (setting.Key.Contains("Image"))
            {
                var opacitySetting = await _context.SiteSettings
                    .FirstOrDefaultAsync(s => s.Key == "MenuBackgroundOpacity");
                ViewBag.CurrentOpacity = opacitySetting?.Value ?? "0.15";
            }

            return View(setting);
        }

        // POST: Admin/Settings/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> Edit(int id, SiteSetting setting, IFormFile? imageFile, string? croppedImageData, string? imageOpacity)
        {
            Console.WriteLine($"[Settings] Updating setting Id: {id}, Key: {setting.Key}");
            
            if (id != setting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update key and other info
                    var dbSetting = await _context.SiteSettings.FindAsync(id);
                    if (dbSetting == null) return NotFound();

                    dbSetting.Description = setting.Description;
                    dbSetting.IsActive = setting.IsActive;
                    dbSetting.UpdatedDate = DateTime.Now;

                    // Handle image upload
                    string? newImageUrl = null;

                    // 1. Cropped image (base64) takes precedence
                    if (!string.IsNullOrEmpty(croppedImageData) && croppedImageData.StartsWith("data:image"))
                    {
                        try 
                        {
                            newImageUrl = await _imageService.SaveImageFromBase64Async(croppedImageData, "settings");
                        }
                        catch (Exception ex)
                        {
                            ModelState.AddModelError("", "Lỗi lưu ảnh đã cắt: " + ex.Message);
                            return View(setting);
                        }
                    }
                    // 2. Fallback to regular file upload (if no crop data)
                    else if (imageFile != null && imageFile.Length > 0)
                    {
                        if (_imageService.IsValidImage(imageFile))
                        {
                            newImageUrl = await _imageService.SaveImageAsync(imageFile, "settings");
                        }
                        else
                        {
                            ModelState.AddModelError("imageFile", "File ảnh không hợp lệ (JPG, PNG, GIF, WEBP, tối đa 5MB).");
                            return View(setting);
                        }
                    }

                    if (!string.IsNullOrEmpty(newImageUrl))
                    {
                        // Delete old image if it exists and changed
                        if (!string.IsNullOrEmpty(dbSetting.Value) && dbSetting.Value != newImageUrl)
                        {
                            _imageService.DeleteImage(dbSetting.Value);
                        }
                        dbSetting.Value = newImageUrl;
                    }
                    else if (!setting.Key.Contains("Image"))
                    {
                        // For non-image settings, update value from form
                        dbSetting.Value = setting.Value;
                    }

                    // Save opacity setting if provided (runs even if image didn't change)
                    if (!string.IsNullOrEmpty(imageOpacity) && dbSetting.Key.Contains("Image"))
                    {
                        var opacitySetting = await _context.SiteSettings
                            .FirstOrDefaultAsync(s => s.Key == "MenuBackgroundOpacity");
                        if (opacitySetting != null)
                        {
                            opacitySetting.Value = imageOpacity;
                            opacitySetting.UpdatedDate = DateTime.Now;
                            _context.Update(opacitySetting);
                        }
                        else
                        {
                            _context.SiteSettings.Add(new SiteSetting
                            {
                                Key = "MenuBackgroundOpacity",
                                Value = imageOpacity,
                                Description = "Độ mờ ảnh nền (0.0 - 1.0)",
                                IsActive = true,
                                CreatedDate = DateTime.Now
                            });
                        }
                    }

                    _context.Update(dbSetting);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật cài đặt thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi hệ thống khi lưu: " + ex.Message);
                }
            }
            
            // Re-load data needed for the view on failure
            if (setting.Key.Contains("Image"))
            {
                ViewBag.CurrentOpacity = imageOpacity ?? "0.15";
            }

            return View(setting);
        }

        private bool SettingExists(int id)
        {
            return _context.SiteSettings.Any(e => e.Id == id);
        }
    }
}

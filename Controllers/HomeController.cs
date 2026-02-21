using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Models.ViewModels;
using OBeefSoup.Data;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // Load site settings
            var siteSettings = await _context.SiteSettings
                .Where(s => s.IsActive)
                .ToListAsync();

            var menuBgImage = siteSettings.FirstOrDefault(s => s.Key == "MenuBackgroundImage")?.Value;
            var menuBgOpacityStr = siteSettings.FirstOrDefault(s => s.Key == "MenuBackgroundOpacity")?.Value ?? "0.15";
            double.TryParse(menuBgOpacityStr, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var menuBgOpacity);

            var viewModel = new HomeViewModel
            {
                Banners = await _context.Banners
                    .Where(b => b.IsActive)
                    .OrderBy(b => b.DisplayOrder)
                    .ToListAsync(),
                FeaturedProducts = await _context.Products
                    .Where(p => p.IsFeatured && p.IsActive)
                    .Include(p => p.Category)
                    .ToListAsync(),
                Stores = await _context.StoreLocations
                    .Where(s => s.IsActive)
                    .OrderBy(s => s.Id)
                    .ToListAsync(),
                MenuBackgroundImage = menuBgImage,
                MenuBackgroundOpacity = menuBgOpacity,
                Testimonials = await _context.Testimonials
                    .Where(t => t.IsApproved)
                    .OrderByDescending(t => t.Date)
                    .Take(6)
                    .ToListAsync(),
                RecentBlogPosts = await _context.BlogPosts
                    .Where(b => b.IsActive)
                    .OrderByDescending(b => b.DisplayOrder)
                    .ThenByDescending(b => b.CreatedDate)
                    .Take(3)
                    .ToListAsync(),
                WhyUsItems = await _context.WhyUsItems
                    .Where(w => w.IsActive)
                    .OrderBy(w => w.DisplayOrder)
                    .ToListAsync(),
                // About Section
                AboutTitle1 = siteSettings.FirstOrDefault(s => s.Key == "AboutTitle1")?.Value ?? "",
                AboutTitle2 = siteSettings.FirstOrDefault(s => s.Key == "AboutTitle2")?.Value ?? "O' BeefSoup",
                AboutSubtitle = siteSettings.FirstOrDefault(s => s.Key == "AboutSubtitle")?.Value ?? "CÂU CHUYỆN THƯƠNG HIỆU",
                AboutDescription = siteSettings.FirstOrDefault(s => s.Key == "AboutDescription")?.Value ?? "",
                AboutImageUrl = siteSettings.FirstOrDefault(s => s.Key == "AboutImageUrl")?.Value ?? "/images/gioithieu.jpg",
                AboutFeatures = await _context.AboutFeatures
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.DisplayOrder)
                    .ToListAsync()
            };

            return View(viewModel);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}

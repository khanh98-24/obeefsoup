using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Controllers
{
    public class MenuController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenuController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Menu
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .OrderBy(p => p.Category.Name)
                .ThenBy(p => p.Name)
                .ToListAsync();

            var categories = await _context.Categories.ToListAsync();

            // Load background settings — đồng bộ với trang chủ
            var bgImageSetting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "MenuBackgroundImage");
            var bgOpacitySetting = await _context.SiteSettings.FirstOrDefaultAsync(s => s.Key == "MenuBackgroundOpacity");
            ViewBag.BgImage = bgImageSetting?.Value ?? "";
            ViewBag.BgOpacity = bgOpacitySetting?.Value ?? "0.05";

            ViewBag.Categories = categories;
            return View(products);
        }

        // GET: /Menu/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Lấy món liên quan: ưu tiên cùng danh mục, bổ sung danh mục khác nếu chưa đủ 4
            var related = await _context.Products
                .Where(p => p.CategoryId == product.CategoryId && p.Id != id && p.IsActive)
                .Take(4)
                .ToListAsync();

            if (related.Count < 4)
            {
                var needed = 4 - related.Count;
                var relatedIds = related.Select(p => p.Id).ToList();
                var extra = await _context.Products
                    .Where(p => p.Id != id && p.IsActive && !relatedIds.Contains(p.Id))
                    .OrderBy(p => Guid.NewGuid()) // random
                    .Take(needed)
                    .ToListAsync();
                related.AddRange(extra);
            }

            ViewBag.RelatedProducts = related;

            return View(product);
        }
    }
}

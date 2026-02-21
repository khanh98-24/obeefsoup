using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;

namespace OBeefSoup.Controllers
{
    public class BlogController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BlogController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Blog
        public async Task<IActionResult> Index()
        {
            var blogPosts = await _context.BlogPosts
                .Where(b => b.IsActive)
                .OrderByDescending(b => b.DisplayOrder)
                .ThenByDescending(b => b.CreatedDate)
                .ToListAsync();
            return View(blogPosts);
        }

        // GET: /Blog/Details/slug
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug)) return NotFound();

            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsActive);

            if (blogPost == null) return NotFound();

            // Lấy thêm các bài viết liên quan (loại trừ bài hiện tại)
            ViewBag.RelatedPosts = await _context.BlogPosts
                .Where(b => b.IsActive && b.Id != blogPost.Id)
                .OrderByDescending(b => b.CreatedDate)
                .Take(3)
                .ToListAsync();

            return View(blogPost);
        }
    }
}

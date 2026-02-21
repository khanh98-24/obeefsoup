using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Services;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class BlogPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BlogPostsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/BlogPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.BlogPosts.OrderByDescending(b => b.CreatedDate).ToListAsync());
        }

        // GET: Admin/BlogPosts/Create
        public IActionResult Create()
        {
            return View(new BlogPost());
        }

        // POST: Admin/BlogPosts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogPost blogPost, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                // Tự động tạo Slug nếu trống
                if (string.IsNullOrEmpty(blogPost.Slug))
                {
                    blogPost.Slug = GenerateSlug(blogPost.Title);
                }

                if (imageFile != null)
                {
                    blogPost.ImageUrl = await _imageService.SaveImageAsync(imageFile, "blog");
                }

                blogPost.CreatedDate = DateTime.Now;
                _context.Add(blogPost);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã thêm bài viết mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        // GET: Admin/BlogPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost == null) return NotFound();

            return View(blogPost);
        }

        // POST: Admin/BlogPosts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BlogPost blogPost, IFormFile? imageFile)
        {
            if (id != blogPost.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingPost = await _context.BlogPosts.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                    if (existingPost == null) return NotFound();

                    if (imageFile != null)
                    {
                        // Xóa ảnh cũ nếu có
                        if (!string.IsNullOrEmpty(existingPost.ImageUrl))
                        {
                            _imageService.DeleteImage(existingPost.ImageUrl);
                        }
                        blogPost.ImageUrl = await _imageService.SaveImageAsync(imageFile, "blog");
                    }
                    else
                    {
                        blogPost.ImageUrl = existingPost.ImageUrl;
                    }

                    if (string.IsNullOrEmpty(blogPost.Slug))
                    {
                        blogPost.Slug = GenerateSlug(blogPost.Title);
                    }

                    blogPost.UpdatedDate = DateTime.Now;
                    blogPost.CreatedDate = existingPost.CreatedDate;

                    _context.Update(blogPost);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Đã cập nhật bài viết thành công!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogPostExists(blogPost.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(blogPost);
        }

        // GET: Admin/BlogPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var blogPost = await _context.BlogPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogPost == null) return NotFound();

            return View(blogPost);
        }

        // POST: Admin/BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blogPost = await _context.BlogPosts.FindAsync(id);
            if (blogPost != null)
            {
                if (!string.IsNullOrEmpty(blogPost.ImageUrl))
                {
                    _imageService.DeleteImage(blogPost.ImageUrl);
                }
                _context.BlogPosts.Remove(blogPost);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Đã xóa bài viết.";
            }
            
            return RedirectToAction(nameof(Index));
        }

        private bool BlogPostExists(int id)
        {
            return _context.BlogPosts.Any(e => e.Id == id);
        }

        private string GenerateSlug(string title)
        {
            string slug = title.ToLower().Trim();
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\s-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"\s+", "-").Trim('-');
            return slug + "-" + DateTime.Now.Ticks.ToString().Substring(10);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;
using OBeefSoup.Services;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public ProductsController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: Admin/Products
        public async Task<IActionResult> Index(string? searchTerm)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                query = query.Where(p => p.Name.Contains(searchTerm) || 
                                       (p.Description != null && p.Description.Contains(searchTerm)));
                ViewBag.SearchTerm = searchTerm;
            }

            var products = await query
                .OrderBy(p => p.CategoryId)
                .ThenBy(p => p.Name)
                .ToListAsync();

            return View(products);
        }

        // GET: Admin/Products/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(
                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                "Id",
                "Name"
            );
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = new Product
                    {
                        Name = model.Name,
                        Description = model.Description,
                        Price = model.Price,
                        CategoryId = model.CategoryId,
                        Stock = model.Stock,
                        IsActive = model.IsActive,
                        IsFeatured = model.IsFeatured,
                        CreatedDate = DateTime.Now
                    };

                    // Handle image upload
                    if (model.ImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.ImageFile))
                        {
                            product.ImageUrl = await _imageService.SaveImageAsync(model.ImageFile);
                        }
                        else
                        {
                            ModelState.AddModelError("ImageFile", "File ảnh không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF (tối đa 5MB)");
                            ViewBag.Categories = new SelectList(
                                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                                "Id",
                                "Name",
                                model.CategoryId
                            );
                            return View(model);
                        }
                    }
                    else
                    {
                        product.ImageUrl = "https://placehold.co/400x300/8B0000/FFF?text=No+Image";
                    }

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Thêm sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi thêm sản phẩm: " + ex.Message);
                }
            }

            ViewBag.Categories = new SelectList(
                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                "Id",
                "Name",
                model.CategoryId
            );
            return View(model);
        }

        // GET: Admin/Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound();

            var model = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Stock = product.Stock,
                IsActive = product.IsActive,
                IsFeatured = product.IsFeatured,
                ImageUrl = product.ImageUrl,
                CreatedDate = product.CreatedDate,
                UpdatedDate = product.UpdatedDate
            };

            ViewBag.Categories = new SelectList(
                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                "Id",
                "Name",
                product.CategoryId
            );

            return View(model);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel model)
        {
            if (id != model.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var product = await _context.Products.FindAsync(id);
                    if (product == null)
                        return NotFound();

                    // Update properties
                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.Stock = model.Stock;
                    product.IsActive = model.IsActive;
                    product.IsFeatured = model.IsFeatured;
                    product.UpdatedDate = DateTime.Now;

                    // Handle image upload
                    if (model.ImageFile != null)
                    {
                        if (_imageService.IsValidImage(model.ImageFile))
                        {
                            // Delete old image
                            if (!string.IsNullOrEmpty(product.ImageUrl))
                            {
                                _imageService.DeleteImage(product.ImageUrl);
                            }

                            // Save new image
                            product.ImageUrl = await _imageService.SaveImageAsync(model.ImageFile);
                        }
                        else
                        {
                            ModelState.AddModelError("ImageFile", "File ảnh không hợp lệ. Chỉ chấp nhận JPG, PNG, GIF (tối đa 5MB)");
                            ViewBag.Categories = new SelectList(
                                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                                "Id",
                                "Name",
                                model.CategoryId
                            );
                            return View(model);
                        }
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật sản phẩm thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(model.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Lỗi khi cập nhật sản phẩm: " + ex.Message);
                }
            }

            ViewBag.Categories = new SelectList(
                await _context.Categories.Where(c => c.IsActive).ToListAsync(),
                "Id",
                "Name",
                model.CategoryId
            );
            return View(model);
        }

        // GET: Admin/Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null)
                    return NotFound();

                // Delete image from server
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    _imageService.DeleteImage(product.ImageUrl);
                }

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Xóa sản phẩm thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa sản phẩm: " + ex.Message;
            }

            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}

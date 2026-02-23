using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class MenusController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MenusController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Menus
        public async Task<IActionResult> Index()
        {
            await EnsureDefaultMenus();
            var menuItems = await _context.MenuItems
                .Include(m => m.Parent)
                .OrderBy(m => m.ParentId)
                .ThenBy(m => m.DisplayOrder)
                .ToListAsync();

            return View(menuItems);
        }

        private async Task EnsureDefaultMenus()
        {
            var defaultMenus = new List<MenuItem>
            {
                new MenuItem { Title = "Tin tức", Url = "/Blog", DisplayOrder = 6, IsActive = true },
                new MenuItem { Title = "Tuyển dụng", Url = "/Recruitment", DisplayOrder = 7, IsActive = true }
            };

            var existingUrls = await _context.MenuItems.Select(m => m.Url).ToListAsync();
            bool changed = false;

            foreach (var dm in defaultMenus)
            {
                if (!existingUrls.Contains(dm.Url))
                {
                    _context.MenuItems.Add(dm);
                    changed = true;
                }
            }

            if (changed)
            {
                await _context.SaveChangesAsync();
            }
        }

        // GET: Admin/Menus/Create
        public async Task<IActionResult> Create()
        {
            ViewBag.ParentMenus = new SelectList(
                await _context.MenuItems.Where(m => m.ParentId == null).ToListAsync(),
                "Id",
                "Title"
            );
            return View();
        }

        // POST: Admin/Menus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MenuItemViewModel model)
        {
            if (ModelState.IsValid)
            {
                var menuItem = new MenuItem
                {
                    Title = model.Title,
                    Url = model.Url,
                    ParentId = model.ParentId,
                    DisplayOrder = model.DisplayOrder,
                    IsActive = model.IsActive,
                    Icon = model.Icon
                };

                _context.MenuItems.Add(menuItem);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = "Thêm mục menu thành công!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.ParentMenus = new SelectList(
                await _context.MenuItems.Where(m => m.ParentId == null).ToListAsync(),
                "Id",
                "Title",
                model.ParentId
            );
            return View(model);
        }

        // GET: Admin/Menus/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null) return NotFound();

            var model = new MenuItemViewModel
            {
                Id = menuItem.Id,
                Title = menuItem.Title,
                Url = menuItem.Url,
                ParentId = menuItem.ParentId,
                DisplayOrder = menuItem.DisplayOrder,
                IsActive = menuItem.IsActive,
                Icon = menuItem.Icon
            };

            ViewBag.ParentMenus = new SelectList(
                await _context.MenuItems.Where(m => m.ParentId == null && m.Id != id).ToListAsync(),
                "Id",
                "Title",
                menuItem.ParentId
            );

            return View(model);
        }

        // POST: Admin/Menus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItemViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var menuItem = await _context.MenuItems.FindAsync(id);
                    if (menuItem == null) return NotFound();

                    menuItem.Title = model.Title;
                    menuItem.Url = model.Url;
                    menuItem.ParentId = model.ParentId;
                    menuItem.DisplayOrder = model.DisplayOrder;
                    menuItem.IsActive = model.IsActive;
                    menuItem.Icon = model.Icon;

                    _context.Update(menuItem);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = "Cập nhật menu thành công!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuItemExists(model.Id)) return NotFound();
                    else throw;
                }
            }

            ViewBag.ParentMenus = new SelectList(
                await _context.MenuItems.Where(m => m.ParentId == null && m.Id != id).ToListAsync(),
                "Id",
                "Title",
                model.ParentId
            );
            return View(model);
        }

        // GET: Admin/Menus/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var menuItem = await _context.MenuItems
                .Include(m => m.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem == null) return NotFound();

            return View(menuItem);
        }

        // POST: Admin/Menus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var menuItem = await _context.MenuItems
                .Include(m => m.SubMenus)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (menuItem != null)
            {
                // Nếu có menu con, gỡ bỏ ParentId của chúng hoặc xóa chúng (tùy thiết kế)
                // Ở đây chọn gỡ bỏ ParentId để không mất menu con
                foreach(var sub in menuItem.SubMenus)
                {
                    sub.ParentId = null;
                }
                
                _context.MenuItems.Remove(menuItem);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Xóa menu thành công!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool MenuItemExists(int id)
        {
            return _context.MenuItems.Any(e => e.Id == id);
        }
    }
}

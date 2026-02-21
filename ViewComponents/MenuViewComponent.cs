using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;

namespace OBeefSoup.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context;

        public MenuViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var menuItems = await _context.MenuItems
                .Include(m => m.SubMenus)
                .Where(m => m.IsActive && m.ParentId == null)
                .OrderBy(m => m.DisplayOrder)
                .ToListAsync();

            return View(menuItems);
        }
    }
}

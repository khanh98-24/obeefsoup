using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    [AdminOnlyFilter]
    public class DatabaseController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.ConnectionString = "Server=(localdb)\\mssqllocaldb;Database=OBeefSoupDb";
            return View();
        }
    }
}

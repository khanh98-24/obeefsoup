using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Services;
using OBeefSoup.Models;
using OBeefSoup.Filters;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            if (order == null) return NotFound();
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus status)
        {
            var success = await _orderService.UpdateOrderStatusAsync(orderId, status);
            
            if (success)
                TempData["Success"] = "Đã cập nhật trạng thái đơn hàng thành công!";
            else
                TempData["Error"] = "Không tìm thấy đơn hàng!";

            return RedirectToAction("Details", new { id = orderId });
        }
    }
}

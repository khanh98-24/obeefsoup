using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Services;
using OBeefSoup.Models;
using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly CartService _cartService;
        private readonly OrderService _orderService;

        public CheckoutController(CartService cartService, OrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }

        // GET: /Checkout
        public async Task<IActionResult> Index()
        {
            var cart = _cartService.GetCart();
            
            if (cart.Count == 0)
            {
                TempData["Message"] = "Giỏ hàng trống. Vui lòng thêm món ăn trước khi thanh toán.";
                return RedirectToAction("Index", "Home");
            }

            var model = new CheckoutViewModel();

            // Kiểm tra nếu khách hàng đã đăng nhập
            var customerIdStr = HttpContext.Session.GetString("CustomerId");
            if (!string.IsNullOrEmpty(customerIdStr))
            {
                // Nếu là Admin đăng nhập dưới quyền khách (có prefix admin_)
                if (customerIdStr.StartsWith("admin_"))
                {
                    model.FullName = HttpContext.Session.GetString("AdminFullName") ?? "";
                    model.Email = HttpContext.Session.GetString("CustomerEmail") ?? "";
                }
                else if (int.TryParse(customerIdStr, out int customerId))
                {
                    // Lấy thông tin từ database
                    var context = HttpContext.RequestServices.GetRequiredService<OBeefSoup.Data.ApplicationDbContext>();
                    var customer = await context.Customers.FindAsync(customerId);
                    if (customer != null)
                    {
                        model.FullName = customer.FullName;
                        model.Email = customer.Email;
                        model.Phone = customer.Phone;
                        model.DeliveryAddress = customer.Address;
                        model.Notes = customer.Notes;
                    }
                }
            }

            ViewBag.CartTotal = _cartService.GetCartTotal();
            ViewBag.Cart = cart;
            
            return View(model);
        }

        // POST: /Checkout/PlaceOrder
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutViewModel model)
        {
            var cart = _cartService.GetCart();
            
            if (!ModelState.IsValid)
            {
                ViewBag.CartTotal = _cartService.GetCartTotal();
                ViewBag.Cart = cart;
                return View("Index", model);
            }
            
            if (cart.Count == 0)
            {
                TempData["Error"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Tạo đơn hàng
                var order = await _orderService.CreateOrderFromCartAsync(
                    cart,
                    model.FullName,
                    model.Email,
                    model.Phone,
                    model.DeliveryAddress,
                    model.Notes
                );

                // Xóa giỏ hàng sau khi đặt hàng thành công
                _cartService.ClearCart();

                // Chuyển đến trang xác nhận
                return RedirectToAction("Confirmation", new { orderId = order.OrderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Đã xảy ra lỗi khi đặt hàng: {ex.Message}";
                ViewBag.CartTotal = _cartService.GetCartTotal();
                ViewBag.Cart = cart;
                return View("Index", model);
            }
        }

        // GET: /Checkout/Confirmation/{orderId}
        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }

    /// <summary>
    /// ViewModel cho trang checkout
    /// </summary>
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        [Display(Name = "Họ và tên")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        [Display(Name = "Số điện thoại")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Vui lòng nhập địa chỉ giao hàng")]
        [Display(Name = "Địa chỉ giao hàng")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Display(Name = "Ghi chú")]
        public string? Notes { get; set; } = string.Empty;
    }
}

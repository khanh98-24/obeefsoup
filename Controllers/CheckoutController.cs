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
        private readonly VnPayService _vnPayService;
        private readonly MoMoService _moMoService;
        private readonly IConfiguration _configuration;

        public CheckoutController(CartService cartService, OrderService orderService, VnPayService vnPayService, MoMoService moMoService, IConfiguration configuration)
        {
            _cartService = cartService;
            _orderService = orderService;
            _vnPayService = vnPayService;
            _moMoService = moMoService;
            _configuration = configuration;
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
                    model.PaymentMethod,
                    model.Notes
                );

                // Xử lý thanh toán dựa trên hình thức đã chọn
                if (model.PaymentMethod == "VNPAY")
                {
                    _cartService.ClearCart();
                    var paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, order);
                    return Redirect(paymentUrl);
                }
                else if (model.PaymentMethod == "MOMO")
                {
                    var momoResponse = await _moMoService.CreatePaymentAsync(HttpContext, order);
                    // Luôn xóa giỏ hàng khi đã tạo đơn (dù thành công hay thất bại)
                    _cartService.ClearCart();
                    if (momoResponse != null && !string.IsNullOrEmpty(momoResponse.PayUrl))
                    {
                        return Redirect(momoResponse.PayUrl);
                    }
                    else
                    {
                        var errorMsg = momoResponse?.Message ?? "Cổng thanh toán MoMo đang bảo trì.";
                        TempData["Error"] = $"Lỗi MoMo: {errorMsg} (Code: {momoResponse?.ResultCode})";
                        return RedirectToAction("Confirmation", new { orderId = order.OrderId });
                    }
                }

                // Nếu là COD, mới xóa giỏ hàng tại đây
                _cartService.ClearCart();

                // Chuyển đến trang xác nhận (cho COD hoặc lỗi thanh toán online nhưng đã tạo đơn)
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

        // GET: /Checkout/VnPayReturn
        public async Task<IActionResult> VnPayReturn()
        {
            var response = _vnPayService.ExecutePayment(Request.Query);

            if (response.Success)
            {
                await _orderService.UpdatePaymentStatusAsync(response.OrderId, OrderPaymentStatus.Paid, response.TransactionId);
                TempData["Message"] = "Thanh toán VN Pay thành công!";
            }
            else
            {
                await _orderService.UpdatePaymentStatusAsync(response.OrderId, OrderPaymentStatus.Failed);
                TempData["Error"] = "Thanh toán VN Pay thất bại hoặc đã bị hủy.";
            }

            return RedirectToAction("Confirmation", new { orderId = response.OrderId });
        }

        // GET: /Checkout/MoMoReturn
        // MoMo chuyển hướng người dùng về đây sau khi thanh toán (redirect)
        public async Task<IActionResult> MoMoReturn([FromQuery] MoMoCallbackParams p)
        {
            // MoMo orderId format: {OrderId}_{timestamp}
            var realOrderIdStr = p.OrderId?.Split('_')[0] ?? "";

            if (!int.TryParse(realOrderIdStr, out int id))
                return RedirectToAction("Index", "Home");

            // Xác minh chữ ký để chống giả mạo
            var secretKey = _configuration["MoMo:SecretKey"];
            if (!_moMoService.VerifySignature(p, secretKey))
            {
                TempData["Error"] = "Chữ ký MoMo không hợp lệ. Vui lòng liên hệ hỗ trợ.";
                return RedirectToAction("Confirmation", new { orderId = id });
            }

            if (p.ResultCode == 0) // 0 = Thành công
            {
                await _orderService.UpdatePaymentStatusAsync(id, OrderPaymentStatus.Paid, p.TransId);
                TempData["Message"] = "Thanh toán MoMo thành công!";
            }
            else
            {
                await _orderService.UpdatePaymentStatusAsync(id, OrderPaymentStatus.Failed);
                TempData["Error"] = $"Thanh toán MoMo thất bại: {p.Message} (Code: {p.ResultCode})";
            }

            return RedirectToAction("Confirmation", new { orderId = id });
        }

        // POST: /Checkout/MoMoIPN
        // MoMo gọi server-to-server để xác nhận giao dịch (IPN)
        [HttpPost]
        public async Task<IActionResult> MoMoIPN([FromBody] MoMoCallbackParams p)
        {
            var secretKey = _configuration["MoMo:SecretKey"];

            if (!_moMoService.VerifySignature(p, secretKey))
                return BadRequest(new { message = "Invalid signature" });

            var realOrderIdStr = p.OrderId?.Split('_')[0] ?? "";
            if (!int.TryParse(realOrderIdStr, out int id))
                return BadRequest(new { message = "Invalid orderId" });

            if (p.ResultCode == 0)
                await _orderService.UpdatePaymentStatusAsync(id, OrderPaymentStatus.Paid, p.TransId);
            else
                await _orderService.UpdatePaymentStatusAsync(id, OrderPaymentStatus.Failed);

            return Ok(new { message = "OK" });
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

        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán")]
        [Display(Name = "Hình thức thanh toán")]
        public string PaymentMethod { get; set; } = "COD";
    }
}

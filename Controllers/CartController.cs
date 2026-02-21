using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using OBeefSoup.Services;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;
        private readonly ApplicationDbContext _context;

        public CartController(CartService cartService, ApplicationDbContext context)
        {
            _cartService = cartService;
            _context = context;
        }

        // GET: /Cart
        public IActionResult Index()
        {
            var cart = _cartService.GetCart();
            ViewBag.CartTotal = _cartService.GetCartTotal();
            return View(cart);
        }

        // POST: /Cart/AddToCart
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            var product = await _context.Products.FindAsync(productId);
            
            if (product == null || !product.IsActive)
            {
                return Json(new { success = false, message = "Sản phẩm không tồn tại" });
            }

            _cartService.AddToCart(
                product.Id,
                product.Name,
                product.Price,
                product.ImageUrl,
                quantity
            );

            var cartCount = _cartService.GetCartItemCount();
            var cartTotal = _cartService.GetCartTotal();

            return Json(new 
            { 
                success = true, 
                message = $"Đã thêm {product.Name} vào giỏ hàng",
                cartCount = cartCount,
                cartTotal = cartTotal
            });
        }

        // POST: /Cart/UpdateQuantity
        [HttpPost]
        public IActionResult UpdateQuantity(int productId, int quantity)
        {
            _cartService.UpdateQuantity(productId, quantity);
            
            var cartTotal = _cartService.GetCartTotal();
            var cartCount = _cartService.GetCartItemCount();

            return Json(new 
            { 
                success = true,
                cartTotal = cartTotal,
                cartCount = cartCount
            });
        }

        // POST: /Cart/RemoveItem
        [HttpPost]
        public IActionResult RemoveItem(int productId)
        {
            _cartService.RemoveFromCart(productId);
            
            var cartTotal = _cartService.GetCartTotal();
            var cartCount = _cartService.GetCartItemCount();

            return Json(new 
            { 
                success = true,
                message = "Đã xóa món khỏi giỏ hàng",
                cartTotal = cartTotal,
                cartCount = cartCount
            });
        }

        // POST: /Cart/Clear
        [HttpPost]
        public IActionResult Clear()
        {
            _cartService.ClearCart();
            return Json(new { success = true, message = "Đã xóa toàn bộ giỏ hàng" });
        }
    }
}

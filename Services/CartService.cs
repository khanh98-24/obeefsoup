using OBeefSoup.Models;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace OBeefSoup.Services
{
    /// <summary>
    /// Service quản lý giỏ hàng (sử dụng Session)
    /// </summary>
    public class CartService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "ShoppingCart";

        public CartService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private ISession? Session => _httpContextAccessor.HttpContext?.Session;

        /// <summary>
        /// Lấy giỏ hàng từ session
        /// </summary>
        public List<CartItem> GetCart()
        {
            if (Session == null) return new List<CartItem>();

            var cartJson = Session.GetString(CartSessionKey);
            if (string.IsNullOrEmpty(cartJson))
            {
                return new List<CartItem>();
            }

            return JsonSerializer.Deserialize<List<CartItem>>(cartJson) ?? new List<CartItem>();
        }

        /// <summary>
        /// Lưu giỏ hàng vào session
        /// </summary>
        private void SaveCart(List<CartItem> cart)
        {
            if (Session == null) return;
            
            var cartJson = JsonSerializer.Serialize(cart);
            Session.SetString(CartSessionKey, cartJson);
        }

        /// <summary>
        /// Thêm món vào giỏ hàng
        /// </summary>
        public void AddToCart(int productId, string productName, decimal price, string imageUrl, int quantity = 1)
        {
            var cart = GetCart();
            var existingItem = cart.FirstOrDefault(item => item.ProductId == productId);

            if (existingItem != null)
            {
                // Món đã có trong giỏ, tăng số lượng
                existingItem.Quantity += quantity;
            }
            else
            {
                // Thêm món mới vào giỏ
                cart.Add(new CartItem
                {
                    ProductId = productId,
                    ProductName = productName,
                    Price = price,
                    ImageUrl = imageUrl,
                    Quantity = quantity
                });
            }

            SaveCart(cart);
        }

        /// <summary>
        /// Xóa món khỏi giỏ hàng
        /// </summary>
        public void RemoveFromCart(int productId)
        {
            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);

            if (item != null)
            {
                cart.Remove(item);
                SaveCart(cart);
            }
        }

        /// <summary>
        /// Cập nhật số lượng món
        /// </summary>
        public void UpdateQuantity(int productId, int quantity)
        {
            if (quantity <= 0)
            {
                RemoveFromCart(productId);
                return;
            }

            var cart = GetCart();
            var item = cart.FirstOrDefault(item => item.ProductId == productId);

            if (item != null)
            {
                item.Quantity = quantity;
                SaveCart(cart);
            }
        }

        /// <summary>
        /// Xóa toàn bộ giỏ hàng
        /// </summary>
        public void ClearCart()
        {
            Session?.Remove(CartSessionKey);
        }

        /// <summary>
        /// Tính tổng tiền giỏ hàng
        /// </summary>
        public decimal GetCartTotal()
        {
            var cart = GetCart();
            return cart.Sum(item => item.Subtotal);
        }

        /// <summary>
        /// Đếm số lượng món trong giỏ
        /// </summary>
        public int GetCartItemCount()
        {
            var cart = GetCart();
            return cart.Sum(item => item.Quantity);
        }
    }
}

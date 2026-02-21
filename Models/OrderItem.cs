using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Chi tiết món ăn trong đơn hàng
    /// </summary>
    public class OrderItem
    {
        public int OrderItemId { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        [Required]
        [StringLength(100)]
        public string ProductName { get; set; } = string.Empty; // Lưu tên tại thời điểm đặt

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0, 10000000)]
        public decimal UnitPrice { get; set; }

        [Required]
        [Range(0, 100000000)]
        public decimal Subtotal { get; set; }

        // Navigation properties
        public Order Order { get; set; } = null!;
        public Product? Product { get; set; }
    }
}

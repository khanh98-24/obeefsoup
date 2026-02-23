using System.ComponentModel.DataAnnotations;

namespace OBeefSoup.Models
{
    /// <summary>
    /// Đơn hàng
    /// </summary>
    public class Order
    {
        public int OrderId { get; set; }

        [Required]
        [StringLength(20)]
        public string OrderNumber { get; set; } = string.Empty; // Mã đơn: ORD20260211001

        public int CustomerId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 100000000)]
        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        [StringLength(200)]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; } = string.Empty;

        public DateTime? CompletedDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; } = null!;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public enum OrderStatus
    {
        Pending = 0,        // Chờ xác nhận
        Confirmed = 1,      // Đã xác nhận
        Preparing = 2,      // Đang chuẩn bị
        Delivering = 3,     // Đang giao hàng
        Completed = 4,      // Hoàn thành
        Cancelled = 5       // Đã hủy
    }
}

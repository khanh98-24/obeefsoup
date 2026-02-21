using OBeefSoup.Models;

namespace OBeefSoup.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        // Thống kê tổng quan
        public int TotalOrders { get; set; }
        public int PendingOrders { get; set; }
        public int TotalProducts { get; set; }
        public int TotalCustomers { get; set; }
        public decimal TotalRevenue { get; set; }

        // Dữ liệu cho biểu đồ (Revenue Chart)
        public List<ChartDataPoint> DailyRevenue { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> MonthlyRevenue { get; set; } = new List<ChartDataPoint>();
        public List<ChartDataPoint> YearlyRevenue { get; set; } = new List<ChartDataPoint>();

        // Danh sách đơn hàng gần đây
        public List<Order> RecentOrders { get; set; } = new List<Order>();

        // Thống kê trạng thái đơn hàng
        public Dictionary<string, int> OrderStatusCounts { get; set; } = new Dictionary<string, int>();

        // Thống kê sản phẩm
        public List<ProductStat> TopSellingProducts { get; set; } = new List<ProductStat>();
        public List<ProductStat> LeastSellingProducts { get; set; } = new List<ProductStat>();
    }

    public class ChartDataPoint
    {
        public string Label { get; set; } = string.Empty;
        public decimal Value { get; set; }
    }

    public class ProductStat
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalRevenue { get; set; }
        public string? ImageUrl { get; set; }
    }
}

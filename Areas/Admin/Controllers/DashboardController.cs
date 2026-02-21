using Microsoft.AspNetCore.Mvc;
using OBeefSoup.Data;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Filters;
using OBeefSoup.Models;
using OBeefSoup.Areas.Admin.Models;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var now = DateTime.Now;
            var startOfMonth = new DateTime(now.Year, now.Month, 1);
            var last7Days = Enumerable.Range(0, 7).Select(i => now.Date.AddDays(-i)).Reverse().ToList();
            var last6Months = Enumerable.Range(0, 6).Select(i => now.Date.AddMonths(-i)).Reverse().ToList();

            var viewModel = new DashboardViewModel
            {
                TotalOrders = await _context.Orders.CountAsync(),
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                TotalProducts = await _context.Products.CountAsync(p => p.IsActive),
                TotalCustomers = await _context.Customers.CountAsync(),
                TotalRevenue = await _context.Orders
                    .Where(o => o.Status == OrderStatus.Completed)
                    .SumAsync(o => o.TotalAmount),

                RecentOrders = await _context.Orders
                    .Include(o => o.Customer)
                    .OrderByDescending(o => o.OrderDate)
                    .Take(5)
                    .ToListAsync()
            };

            // Thống kê doanh thu 7 ngày gần nhất
            foreach (var day in last7Days)
            {
                var revenue = await _context.Orders
                    .Where(o => o.OrderDate.Date == day && o.Status == OrderStatus.Completed)
                    .SumAsync(o => o.TotalAmount);
                viewModel.DailyRevenue.Add(new ChartDataPoint { Label = day.ToString("dd/MM"), Value = revenue });
            }

            // Thống kê doanh thu 6 tháng gần nhất
            foreach (var month in last6Months)
            {
                var revenue = await _context.Orders
                    .Where(o => o.OrderDate.Year == month.Year && o.OrderDate.Month == month.Month && o.Status == OrderStatus.Completed)
                    .SumAsync(o => o.TotalAmount);
                viewModel.MonthlyRevenue.Add(new ChartDataPoint { Label = month.ToString("MM/yyyy"), Value = revenue });
            }

            // Thống kê doanh thu 5 năm gần nhất
            var last5Years = Enumerable.Range(0, 5).Select(i => now.Date.AddYears(-i)).Reverse().ToList();
            foreach (var yearDate in last5Years)
            {
                var revenue = await _context.Orders
                    .Where(o => o.OrderDate.Year == yearDate.Year && o.Status == OrderStatus.Completed)
                    .SumAsync(o => o.TotalAmount);
                viewModel.YearlyRevenue.Add(new ChartDataPoint { Label = yearDate.Year.ToString(), Value = revenue });
            }

            // Thống kê trạng thái đơn hàng
            var statuses = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>();
            foreach (var status in statuses)
            {
                var count = await _context.Orders.CountAsync(o => o.Status == status);
                string statusName = status switch
                {
                    OrderStatus.Pending => "Chờ xác nhận",
                    OrderStatus.Confirmed => "Đã xác nhận",
                    OrderStatus.Preparing => "Đang chuẩn bị",
                    OrderStatus.Delivering => "Đang giao hàng",
                    OrderStatus.Completed => "Hoàn thành",
                    OrderStatus.Cancelled => "Đã hủy",
                    _ => status.ToString()
                };
                viewModel.OrderStatusCounts.Add(statusName, count);
            }

            // Thống kê sản phẩm bán chạy nhất (Top 5)
            viewModel.TopSellingProducts = await _context.OrderItems
                .Where(oi => oi.Order.Status == OrderStatus.Completed)
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new ProductStat
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Subtotal),
                    ImageUrl = _context.Products.FirstOrDefault(p => p.Id == g.Key.ProductId)!.ImageUrl
                })
                .OrderByDescending(x => x.TotalQuantity)
                .Take(5)
                .ToListAsync();

            // Thống kê sản phẩm ít khách nhất (Top 5 ít nhất)
            viewModel.LeastSellingProducts = await _context.OrderItems
                .Where(oi => oi.Order.Status == OrderStatus.Completed)
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new ProductStat
                {
                    ProductId = g.Key.ProductId,
                    ProductName = g.Key.ProductName,
                    TotalQuantity = g.Sum(x => x.Quantity),
                    TotalRevenue = g.Sum(x => x.Subtotal),
                    ImageUrl = _context.Products.FirstOrDefault(p => p.Id == g.Key.ProductId)!.ImageUrl
                })
                .OrderBy(x => x.TotalQuantity)
                .Take(5)
                .ToListAsync();

            return View(viewModel);
        }
    }
}

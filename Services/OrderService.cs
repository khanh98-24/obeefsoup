using OBeefSoup.Data;
using OBeefSoup.Models;
using OBeefSoup.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace OBeefSoup.Services
{
    /// <summary>
    /// Service xử lý đơn hàng
    /// </summary>
    public class OrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Tạo đơn hàng từ giỏ hàng
        /// </summary>
        public async Task<Order> CreateOrderFromCartAsync(List<CartItem> cart, string fullName, string email, 
            string phone, string address, string notes = "")
        {
            // Tạo hoặc lấy thông tin khách hàng
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Email == email || c.Phone == phone);

            if (customer == null)
            {
                customer = new Customer
                {
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    Address = address ?? "",
                    Notes = notes ?? "",
                    CreatedDate = DateTime.Now
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Cập nhật thông tin nếu cần
                customer.FullName = fullName;
                customer.Address = address ?? "";
                if (!string.IsNullOrEmpty(notes))
                {
                    customer.Notes = notes;
                }
            }

            // Tạo đơn hàng
            var order = new Order
            {
                CustomerId = customer.CustomerId,
                OrderNumber = GenerateOrderNumber(),
                OrderDate = DateTime.Now,
                Status = OrderStatus.Pending,
                DeliveryAddress = address ?? "",
                PhoneNumber = phone,
                Notes = notes ?? "",
                TotalAmount = cart.Sum(item => item.Subtotal)
            };

            // Thêm chi tiết đơn hàng
            foreach (var cartItem in cart)
            {
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    ProductName = cartItem.ProductName,
                    Quantity = cartItem.Quantity,
                    UnitPrice = cartItem.Price,
                    Subtotal = cartItem.Subtotal
                });
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Cập nhật ngày đặt hàng cuối của khách
            customer.LastOrderDate = DateTime.Now;
            await _context.SaveChangesAsync();

            return order;
        }

        /// <summary>
        /// Lấy thông tin đơn hàng theo ID
        /// </summary>
        public async Task<Order?> GetOrderByIdAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        /// <summary>
        /// Lấy danh sách đơn hàng của khách
        /// </summary>
        public async Task<List<Order>> GetOrdersByPhoneAsync(string phone)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Phone == phone);

            if (customer == null) return new List<Order>();

            return await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.CustomerId == customer.CustomerId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// Lấy tất cả đơn hàng (cho admin) với bộ lọc ngày
        /// </summary>
        public async Task<List<Order>> GetAllOrdersAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                var startOfDay = fromDate.Value.Date;
                query = query.Where(o => o.OrderDate >= startOfDay);
            }

            if (toDate.HasValue)
            {
                var endOfDay = toDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(o => o.OrderDate <= endOfDay);
            }

            return await query
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();
        }

        /// <summary>
        /// Cập nhật trạng thái đơn hàng
        /// </summary>
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null) return false;

            var oldStatus = order.Status;
            order.Status = status;
            
            // Logic xử lý tồn kho
            if (status == OrderStatus.Completed)
            {
                order.CompletedDate = DateTime.Now;
                
                // Nếu chuyển từ trạng thái khác sang Hoàn thành -> Trừ kho
                if (oldStatus != OrderStatus.Completed)
                {
                    foreach (var item in order.OrderItems)
                    {
                        if (item.Product != null)
                        {
                            item.Product.Stock -= item.Quantity;
                        }
                    }
                }
            }
            else if (oldStatus == OrderStatus.Completed && status != OrderStatus.Completed)
            {
                // Nếu chuyển từ Hoàn thành sang trạng thái khác (ví dụ: Hủy) -> Cộng lại kho
                foreach (var item in order.OrderItems)
                {
                    if (item.Product != null)
                    {
                        item.Product.Stock += item.Quantity;
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Tạo mã đơn hàng tự động
        /// </summary>
        private string GenerateOrderNumber()
        {
            var today = DateTime.Now;
            var orderCount = _context.Orders
                .Count(o => o.OrderDate.Date == today.Date) + 1;

            return $"ORD{today:yyyyMMdd}{orderCount:D3}";
        }
    }
}

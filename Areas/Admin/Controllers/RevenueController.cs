using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBeefSoup.Data;
using OBeefSoup.Filters;
using OBeefSoup.Models;

namespace OBeefSoup.Areas.Admin.Controllers
{
    [Area("Admin")]
    [AdminAuthFilter]
    public class RevenueController : Controller
    {
        private readonly ApplicationDbContext _context;
        private const int PageSize = 5;

        public RevenueController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(
            DateTime? fromDate, DateTime? toDate,
            string groupBy = "day", string activeRange = "",
            string search = "", int page = 1)
        {
            ViewBag.ActiveRange = activeRange;
            ViewBag.Search      = search ?? "";

            var from = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var to   = (toDate  ?? DateTime.Now.Date).Date.AddDays(1).AddTicks(-1);

            var baseQuery = _context.Orders
                .Where(o => o.Status == OrderStatus.Completed
                         && o.OrderDate >= from && o.OrderDate <= to);

            var totalRevenue = await baseQuery.SumAsync(o => (decimal?)o.TotalAmount) ?? 0;
            var totalOrders  = await baseQuery.CountAsync();
            var avgOrder     = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            List<RevenueDataPoint> chartData;
            if (groupBy == "week")
            {
                var raw = await baseQuery
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.TotalAmount), Orders = g.Count() })
                    .OrderBy(x => x.Date).ToListAsync();
                chartData = raw
                    .GroupBy(x => { int dw = (int)x.Date.DayOfWeek; return x.Date.AddDays(dw == 0 ? -6 : 1 - dw).Date; })
                    .Select(g => new RevenueDataPoint { Label = $"{g.Key:dd/MM}–{g.Key.AddDays(6):dd/MM}", Revenue = g.Sum(x => x.Revenue), Orders = g.Sum(x => x.Orders) })
                    .OrderBy(x => x.Label).ToList();
            }
            else if (groupBy == "month")
            {
                var raw = await baseQuery
                    .GroupBy(o => new { o.OrderDate.Year, o.OrderDate.Month })
                    .Select(g => new { g.Key.Year, g.Key.Month, Revenue = g.Sum(o => o.TotalAmount), Orders = g.Count() })
                    .OrderBy(x => x.Year).ThenBy(x => x.Month).ToListAsync();
                chartData = raw.Select(x => new RevenueDataPoint { Label = $"{x.Month:D2}/{x.Year}", Revenue = x.Revenue, Orders = x.Orders }).ToList();
            }
            else if (groupBy == "year")
            {
                var raw = await baseQuery
                    .GroupBy(o => o.OrderDate.Year)
                    .Select(g => new { Year = g.Key, Revenue = g.Sum(o => o.TotalAmount), Orders = g.Count() })
                    .OrderBy(x => x.Year).ToListAsync();
                chartData = raw.Select(x => new RevenueDataPoint { Label = x.Year.ToString(), Revenue = x.Revenue, Orders = x.Orders }).ToList();
            }
            else
            {
                var raw = await baseQuery
                    .GroupBy(o => o.OrderDate.Date)
                    .Select(g => new { Date = g.Key, Revenue = g.Sum(o => o.TotalAmount), Orders = g.Count() })
                    .OrderBy(x => x.Date).ToListAsync();
                chartData = raw.Select(x => new RevenueDataPoint { Label = x.Date.ToString("dd/MM"), Revenue = x.Revenue, Orders = x.Orders }).ToList();
            }

            var topProducts = await _context.OrderItems
                .Where(oi => oi.Order.Status == OrderStatus.Completed && oi.Order.OrderDate >= from && oi.Order.OrderDate <= to)
                .GroupBy(oi => new { oi.ProductId, oi.ProductName })
                .Select(g => new ProductRevenueStat { ProductName = g.Key.ProductName, TotalQuantity = g.Sum(x => x.Quantity), TotalRevenue = g.Sum(x => x.Subtotal) })
                .OrderByDescending(x => x.TotalRevenue).Take(10).ToListAsync();

            var paymentStats = await baseQuery
                .GroupBy(o => o.PaymentMethod)
                .Select(g => new PaymentMethodStat { Method = g.Key, Count = g.Count(), Revenue = g.Sum(o => o.TotalAmount) })
                .ToListAsync();

            var ordersQuery = _context.Orders.Include(o => o.Customer)
                .Where(o => o.Status == OrderStatus.Completed && o.OrderDate >= from && o.OrderDate <= to);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                ordersQuery = ordersQuery.Where(o =>
                    o.OrderNumber.ToLower().Contains(s) ||
                    o.Customer.FullName.ToLower().Contains(s) ||
                    o.PhoneNumber.Contains(s));
            }
            ordersQuery = ordersQuery.OrderByDescending(o => o.OrderDate);
            var totalOrderCount = await ordersQuery.CountAsync();
            var totalPages      = (int)Math.Ceiling(totalOrderCount / (double)PageSize);
            var pagedOrders     = await ordersQuery.Skip((page - 1) * PageSize).Take(PageSize).ToListAsync();

            var vm = new RevenueViewModel
            {
                FromDate = from, ToDate = toDate ?? DateTime.Now.Date,
                GroupBy = groupBy, Search = search ?? "",
                TotalRevenue = totalRevenue, TotalOrders = totalOrders, AvgOrderValue = avgOrder,
                ChartData = chartData, TopProducts = topProducts, PaymentStats = paymentStats,
                Orders = pagedOrders, CurrentPage = page, TotalPages = totalPages, TotalOrderCount = totalOrderCount
            };

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                return PartialView("_RevenueContent", vm);

            return View(vm);
        }

        // ─── Export Excel (SpreadsheetML XML) ───────────────────────────────
        public async Task<IActionResult> ExportXls(
            DateTime? fromDate, DateTime? toDate, string search = "")
        {
            var from = fromDate ?? new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var to   = (toDate  ?? DateTime.Now.Date).Date.AddDays(1).AddTicks(-1);
            var now  = DateTime.Now;

            var query = _context.Orders.Include(o => o.Customer)
                .Where(o => o.Status == OrderStatus.Completed && o.OrderDate >= from && o.OrderDate <= to);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(o =>
                    o.OrderNumber.ToLower().Contains(s) ||
                    o.Customer.FullName.ToLower().Contains(s) ||
                    o.PhoneNumber.Contains(s));
            }

            var orders  = await query.OrderByDescending(o => o.OrderDate).ToListAsync();
            var total   = orders.Sum(o => o.TotalAmount);
            var avgVal  = orders.Count > 0 ? total / orders.Count : 0;
            var codRev  = orders.Where(o => o.PaymentMethod == "COD").Sum(o => o.TotalAmount);
            var vnpRev  = orders.Where(o => o.PaymentMethod == "VNPAY").Sum(o => o.TotalAmount);
            var momoRev = orders.Where(o => o.PaymentMethod == "MoMo").Sum(o => o.TotalAmount);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
            sb.AppendLine("<Styles>");

            // ── Style definitions ──
            void Sty(string id, string inner) => sb.AppendLine($"<Style ss:ID=\"{id}\">{inner}</Style>");
            Sty("cName",   "<Font ss:Bold=\"1\" ss:Size=\"22\" ss:Color=\"#8B0000\"/><Alignment ss:Vertical=\"Center\"/>");
            Sty("cSub",    "<Font ss:Size=\"10\" ss:Color=\"#666666\" ss:Italic=\"1\"/><Alignment ss:Vertical=\"Center\"/>");
            Sty("rTitle",  "<Font ss:Bold=\"1\" ss:Size=\"14\" ss:Color=\"#1a237e\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Interior ss:Color=\"#E8EAF6\" ss:Pattern=\"Solid\"/>");
            Sty("infoLbl", "<Font ss:Bold=\"1\" ss:Size=\"9\" ss:Color=\"#555555\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/>");
            Sty("infoVal", "<Font ss:Size=\"9\" ss:Color=\"#222222\"/><Alignment ss:Vertical=\"Center\"/>");
            Sty("secH",    "<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#FFFFFF\"/><Interior ss:Color=\"#37474F\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Left\" ss:Vertical=\"Center\"/>");
            string kB = "<Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDDDDD\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDDDDD\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDDDDD\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#CCCCCC\"/>";
            Sty("kpiL",  $"<Font ss:Bold=\"1\" ss:Size=\"9\" ss:Color=\"#777777\"/><Interior ss:Color=\"#F5F5F5\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{kB}</Borders>");
            Sty("kpiV",  $"<Font ss:Bold=\"1\" ss:Size=\"16\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/><NumberFormat ss:Format=\"#,##0\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{kB}</Borders>");
            Sty("kpiVN", $"<Font ss:Bold=\"1\" ss:Size=\"16\" ss:Color=\"#1a237e\"/><Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{kB}</Borders>");
            Sty("tH", "<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#FFFFFF\"/><Interior ss:Color=\"#8B0000\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#5D0000\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#5D0000\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#5D0000\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#5D0000\"/></Borders>");
            string dB = "<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#E0E0E0\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#E0E0E0\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#E0E0E0\"/>";
            Sty("dE", $"<Font ss:Size=\"10\"/><Alignment ss:Vertical=\"Center\"/><Borders>{dB}</Borders>");
            Sty("dO", $"<Font ss:Size=\"10\"/><Interior ss:Color=\"#FDF7F7\" ss:Pattern=\"Solid\"/><Alignment ss:Vertical=\"Center\"/><Borders>{dB}</Borders>");
            Sty("mE", $"<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#8B0000\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><NumberFormat ss:Format=\"#,##0\"/><Borders>{dB}</Borders>");
            Sty("mO", $"<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#FDF7F7\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><NumberFormat ss:Format=\"#,##0\"/><Borders>{dB}</Borders>");
            Sty("nE", $"<Font ss:Bold=\"1\" ss:Size=\"10\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{dB}</Borders>");
            Sty("nO", $"<Font ss:Bold=\"1\" ss:Size=\"10\"/><Interior ss:Color=\"#FDF7F7\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{dB}</Borders>");
            Sty("totL", "<Font ss:Bold=\"1\" ss:Size=\"11\" ss:Color=\"#1a237e\"/><Interior ss:Color=\"#E8EAF6\" ss:Pattern=\"Solid\"/><Alignment ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/></Borders>");
            Sty("totV", "<Font ss:Bold=\"1\" ss:Size=\"13\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#E8EAF6\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><NumberFormat ss:Format=\"#,##0\"/><Borders><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/></Borders>");
            Sty("ftr",  "<Font ss:Size=\"9\" ss:Color=\"#AAAAAA\" ss:Italic=\"1\"/><Alignment ss:Vertical=\"Center\"/>");

            sb.AppendLine("</Styles>");
            sb.AppendLine("<Worksheet ss:Name=\"Báo cáo doanh thu\"><Table ss:DefaultRowHeight=\"20\">");
            sb.AppendLine("<Column ss:Width=\"35\"/><Column ss:Width=\"130\"/><Column ss:Width=\"150\"/><Column ss:Width=\"105\"/><Column ss:Width=\"115\"/><Column ss:Width=\"90\"/><Column ss:Width=\"115\"/>");

            // Helper: row and cell factories
            void R(int h, string cells) => sb.AppendLine($"<Row ss:Height=\"{h}\">{cells}</Row>");
            void Rmt(string cells = "") => sb.AppendLine($"<Row ss:Height=\"8\">{cells}</Row>");
            string Txt(string style, string val, int merge = 0) =>
                merge > 0 ? $"<Cell ss:MergeAcross=\"{merge}\" ss:StyleID=\"{style}\"><Data ss:Type=\"String\">{val}</Data></Cell>"
                           : $"<Cell ss:StyleID=\"{style}\"><Data ss:Type=\"String\">{val}</Data></Cell>";
            string Num(string style, decimal val, int merge = 0) =>
                merge > 0 ? $"<Cell ss:MergeAcross=\"{merge}\" ss:StyleID=\"{style}\"><Data ss:Type=\"Number\">{val}</Data></Cell>"
                           : $"<Cell ss:StyleID=\"{style}\"><Data ss:Type=\"Number\">{val}</Data></Cell>";
            string Empty(int merge = 0) => merge > 0 ? $"<Cell ss:MergeAcross=\"{merge}\"><Data ss:Type=\"String\"></Data></Cell>"
                                                      : "<Cell><Data ss:Type=\"String\"></Data></Cell>";

            // ══════════════════════════════════════
            // SECTION 1: COMPANY HEADER
            // ══════════════════════════════════════
            Rmt();
            R(44, Txt("cName", "O&#39;BEEF SOUP", 6));
            R(18, Txt("cSub",  "Nhà hàng Phở Bò Cao Cấp  |  Hệ thống quản lý bán hàng nội bộ", 6));
            Rmt();
            // Divider via thick title bar
            R(30, Txt("rTitle", $"📊  BÁO CÁO DOANH THU  —  KỲ {from:dd/MM/yyyy} đến {to.Date:dd/MM/yyyy}", 6));
            Rmt();
            // Report metadata (2x2 grid across 4 cols, 2 empty cols right)
            R(18, Txt("infoLbl","Người xuất báo cáo:",1) + Txt("infoVal","Quản trị viên")
                + Txt("infoLbl","Ngày xuất:",1)         + Txt("infoVal",$"{now:dd/MM/yyyy HH:mm}") + Empty(1));
            R(18, Txt("infoLbl","Kỳ báo cáo:",1)        + Txt("infoVal",$"{from:dd/MM/yyyy} – {to.Date:dd/MM/yyyy}")
                + Txt("infoLbl","Số đơn hoàn thành:",1) + Txt("infoVal",$"{orders.Count} đơn") + Empty(1));
            if (!string.IsNullOrEmpty(search))
                R(18, Txt("infoLbl","Bộ lọc tìm kiếm:",1) + Txt("infoVal",$"«{search}»") + Empty(4));
            Rmt();

            // ══════════════════════════════════════
            // SECTION 2: KPI SUMMARY
            // ══════════════════════════════════════
            R(24, Txt("secH", $"  TỔNG QUAN DOANH THU", 6));
            // Row 1: labels (3 pairs, each spans 2 cols across 7 cols total → MergeAcross=1 each)
            R(20, Txt("kpiL","TỔNG DOANH THU (đ)",1)     + Txt("kpiL","SỐ ĐƠN HOÀN THÀNH",1) + Txt("kpiL","GIÁ TRỊ TRUNG BÌNH / ĐƠN",1));
            R(34, Num("kpiV", total, 1)                   + Num("kpiVN", orders.Count, 1)      + Num("kpiV", avgVal, 1));
            Rmt();
            R(20, Txt("kpiL","COD — Tiền mặt (đ)",1)     + Txt("kpiL","VN Pay (đ)",1)        + Txt("kpiL","Ví MoMo (đ)",1));
            R(30, Num("kpiV", codRev, 1)                  + Num("kpiV", vnpRev, 1)             + Num("kpiV", momoRev, 1));
            Rmt();

            // ══════════════════════════════════════
            // SECTION 3: ORDER DETAIL TABLE
            // ══════════════════════════════════════
            R(24, Txt("secH", $"  CHI TIẾT ĐƠN HÀNG  ({orders.Count} đơn được hiển thị)", 6));
            R(26,
                Txt("tH","STT") +
                Txt("tH","Mã đơn hàng") +
                Txt("tH","Khách hàng") +
                Txt("tH","Số điện thoại") +
                Txt("tH","Ngày đặt hàng") +
                Txt("tH","Thanh toán") +
                Txt("tH","Tổng tiền (đ)"));

            for (int i = 0; i < orders.Count; i++)
            {
                var o    = orders[i];
                bool alt = i % 2 == 1;
                var sD   = alt ? "dO" : "dE";
                var sM   = alt ? "mO" : "mE";
                var sN   = alt ? "nO" : "nE";
                var met  = o.PaymentMethod == "COD" ? "Tiền mặt" : o.PaymentMethod == "VNPAY" ? "VN Pay" : "Ví MoMo";
                R(18,
                    Num(sN, i + 1) +
                    Txt(sD, o.OrderNumber) +
                    Txt(sD, o.Customer?.FullName ?? "") +
                    Txt(sD, o.PhoneNumber ?? "") +
                    Txt(sD, o.OrderDate.ToString("dd/MM/yyyy HH:mm")) +
                    Txt(sD, met) +
                    Num(sM, o.TotalAmount));
            }

            // Total
            R(30,
                Txt("totL", "", 5) +
                Txt("totL", $"TỔNG CỘNG  ({orders.Count} đơn)") +
                Num("totV", total));
            Rmt();

            // ══════════════════════════════════════
            // SECTION 4: FOOTER
            // ══════════════════════════════════════
            R(14, Txt("ftr", $"Báo cáo được xuất tự động từ hệ thống O'Beef Soup vào lúc {now:dd/MM/yyyy HH:mm:ss}. Tài liệu chỉ dùng cho mục đích nội bộ.", 6));
            R(14, Txt("ftr", "Chỉ bao gồm các đơn hàng có trạng thái Hoàn thành. Liên hệ quản trị viên nếu phát hiện sai lệch.", 6));

            sb.AppendLine("</Table></Worksheet></Workbook>");

            var bytes    = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
            var filename = $"BaoCaoDoanhthu_{from:yyyyMMdd}_{to.Date:yyyyMMdd}.xls";
            return File(bytes, "application/vnd.ms-excel; charset=utf-8", filename);
        }
    }

    public class RevenueViewModel
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate   { get; set; }
        public string   GroupBy  { get; set; } = "day";
        public string   Search   { get; set; } = "";

        public decimal TotalRevenue  { get; set; }
        public int     TotalOrders   { get; set; }
        public decimal AvgOrderValue { get; set; }

        public List<RevenueDataPoint>   ChartData    { get; set; } = new();
        public List<ProductRevenueStat> TopProducts  { get; set; } = new();
        public List<PaymentMethodStat>  PaymentStats { get; set; } = new();
        public List<Order>              Orders       { get; set; } = new();

        public int CurrentPage     { get; set; } = 1;
        public int TotalPages      { get; set; } = 1;
        public int TotalOrderCount { get; set; }
    }

    public class RevenueDataPoint
    {
        public string  Label   { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public int     Orders  { get; set; }
    }

    public class ProductRevenueStat
    {
        public string  ProductName   { get; set; } = string.Empty;
        public int     TotalQuantity { get; set; }
        public decimal TotalRevenue  { get; set; }
    }

    public class PaymentMethodStat
    {
        public string  Method  { get; set; } = string.Empty;
        public int     Count   { get; set; }
        public decimal Revenue { get; set; }
    }
}

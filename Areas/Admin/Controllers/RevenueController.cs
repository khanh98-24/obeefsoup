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

            // ── Query ──────────────────────────────────────────────────
            var query = _context.Orders
                .Include(o => o.Customer)
                .Where(o => o.Status == OrderStatus.Completed
                         && o.OrderDate >= from && o.OrderDate <= to);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(o =>
                    o.OrderNumber.ToLower().Contains(s) ||
                    o.Customer.FullName.ToLower().Contains(s) ||
                    o.PhoneNumber.Contains(s));
            }

            var orders = await query.OrderByDescending(o => o.OrderDate).ToListAsync();

            // ── Tính toán chính xác ────────────────────────────────────
            var totalRevenue = orders.Sum(o => o.TotalAmount);
            var totalOrders  = orders.Count;
            var avgValue     = totalOrders > 0 ? totalRevenue / totalOrders : 0;

            var codOrders  = orders.Where(o => o.PaymentMethod == "COD").ToList();
            var vnpOrders  = orders.Where(o => o.PaymentMethod == "VNPAY").ToList();
            var momoOrders = orders.Where(o => o.PaymentMethod == "MOMO").ToList();

            var codRev  = codOrders.Sum(o  => o.TotalAmount);
            var vnpRev  = vnpOrders.Sum(o  => o.TotalAmount);
            var momoRev = momoOrders.Sum(o => o.TotalAmount);

            var codCount  = codOrders.Count;
            var vnpCount  = vnpOrders.Count;
            var momoCount = momoOrders.Count;

            // ── SpreadsheetML (6 cột) ──────────────────────────────────
            // Cột: [0]STT [1]Mã đơn [2]Khách hàng [3]Ngày đặt [4]Thanh toán [5]Tổng tiền
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            sb.AppendLine("<?mso-application progid=\"Excel.Sheet\"?>");
            sb.AppendLine("<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\">");
            sb.AppendLine("<Styles>");

            // ── Style definitions ──────────────────────────────────────
            void S(string id, string body) => sb.AppendLine($"<Style ss:ID=\"{id}\">{body}</Style>");

            S("hdr",  "<Font ss:Bold=\"1\" ss:Size=\"20\" ss:Color=\"#8B0000\"/><Alignment ss:Vertical=\"Center\"/>");
            S("sub",  "<Font ss:Size=\"9\" ss:Color=\"#888888\" ss:Italic=\"1\"/><Alignment ss:Vertical=\"Center\"/>");
            S("sec",  "<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#FFFFFF\"/><Interior ss:Color=\"#8B0000\" ss:Pattern=\"Solid\"/><Alignment ss:Vertical=\"Center\"/>");
            S("iL",   "<Font ss:Bold=\"1\" ss:Size=\"9\" ss:Color=\"#555\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/>");
            S("iV",   "<Font ss:Size=\"9\" ss:Color=\"#222\"/><Alignment ss:Vertical=\"Center\"/>");

            // KPI label style
            S("kL",  "<Font ss:Bold=\"1\" ss:Size=\"9\" ss:Color=\"#666\"/><Interior ss:Color=\"#F5F5F5\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/></Borders>");
            // KPI value style (currency - red)
            S("kV",  "<Font ss:Bold=\"1\" ss:Size=\"15\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/><NumberFormat ss:Format=\"#,##0\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#CCC\"/></Borders>");
            // KPI value style (count - blue)
            S("kN",  "<Font ss:Bold=\"1\" ss:Size=\"15\" ss:Color=\"#1a237e\"/><Interior ss:Color=\"#FFFFFF\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#DDD\"/><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#CCC\"/></Borders>");

            // Table header
            S("tH",  "<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#FFFFFF\"/><Interior ss:Color=\"#1a237e\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#0d1757\"/></Borders>");
            // Table data – even & odd rows
            string bd = "<Border ss:Position=\"Bottom\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#EBEBEB\"/><Border ss:Position=\"Left\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#EBEBEB\"/><Border ss:Position=\"Right\" ss:LineStyle=\"Continuous\" ss:Weight=\"1\" ss:Color=\"#EBEBEB\"/>";
            S("dE", $"<Font ss:Size=\"10\"/><Alignment ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            S("dO", $"<Font ss:Size=\"10\"/><Interior ss:Color=\"#FBF7F7\" ss:Pattern=\"Solid\"/><Alignment ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            S("mE", $"<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#8B0000\"/><NumberFormat ss:Format=\"#,##0\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            S("mO", $"<Font ss:Bold=\"1\" ss:Size=\"10\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#FBF7F7\" ss:Pattern=\"Solid\"/><NumberFormat ss:Format=\"#,##0\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            S("nE", $"<Font ss:Bold=\"1\" ss:Size=\"10\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            S("nO", $"<Font ss:Bold=\"1\" ss:Size=\"10\"/><Interior ss:Color=\"#FBF7F7\" ss:Pattern=\"Solid\"/><Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Center\"/><Borders>{bd}</Borders>");
            // Total row
            S("totL", "<Font ss:Bold=\"1\" ss:Size=\"11\" ss:Color=\"#1a237e\"/><Interior ss:Color=\"#E8EAF6\" ss:Pattern=\"Solid\"/><Alignment ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/></Borders>");
            S("totV", "<Font ss:Bold=\"1\" ss:Size=\"12\" ss:Color=\"#8B0000\"/><Interior ss:Color=\"#E8EAF6\" ss:Pattern=\"Solid\"/><NumberFormat ss:Format=\"#,##0\"/><Alignment ss:Horizontal=\"Right\" ss:Vertical=\"Center\"/><Borders><Border ss:Position=\"Top\" ss:LineStyle=\"Continuous\" ss:Weight=\"2\" ss:Color=\"#1a237e\"/></Borders>");
            S("ftr",  "<Font ss:Size=\"8\" ss:Color=\"#AAAAAA\" ss:Italic=\"1\"/><Alignment ss:Vertical=\"Center\"/>");

            sb.AppendLine("</Styles>");

            // 6 columns
            sb.AppendLine("<Worksheet ss:Name=\"Báo cáo doanh thu\"><Table ss:DefaultRowHeight=\"20\">");
            sb.AppendLine("<Column ss:Width=\"35\"/>"); // STT
            sb.AppendLine("<Column ss:Width=\"135\"/>");// Mã đơn
            sb.AppendLine("<Column ss:Width=\"140\"/>");// Khách hàng
            sb.AppendLine("<Column ss:Width=\"115\"/>");// Ngày đặt
            sb.AppendLine("<Column ss:Width=\"90\"/>"); // Thanh toán
            sb.AppendLine("<Column ss:Width=\"120\"/>");// Tổng tiền

            // Helper – MergeAcross=5 = toàn bộ 6 cột
            int FULL = 5; // = 6 cột - 1

            void Row(int h, string cells) => sb.AppendLine($"<Row ss:Height=\"{h}\">{cells}</Row>");
            void Gap(int h = 8) => sb.AppendLine($"<Row ss:Height=\"{h}\"/>");

            string T(string sty, string val, int merge = 0) =>
                merge > 0
                    ? $"<Cell ss:MergeAcross=\"{merge}\" ss:StyleID=\"{sty}\"><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(val)}</Data></Cell>"
                    : $"<Cell ss:StyleID=\"{sty}\"><Data ss:Type=\"String\">{System.Security.SecurityElement.Escape(val)}</Data></Cell>";

            var ic = System.Globalization.CultureInfo.InvariantCulture;

            string N(string sty, decimal val, int merge = 0)
            {
                var numStr = val.ToString("0.##", ic); // InvariantCulture: no comma issues
                return merge > 0
                    ? $"<Cell ss:MergeAcross=\"{merge}\" ss:StyleID=\"{sty}\"><Data ss:Type=\"Number\">{numStr}</Data></Cell>"
                    : $"<Cell ss:StyleID=\"{sty}\"><Data ss:Type=\"Number\">{numStr}</Data></Cell>";
            }

            string NI(string sty, int val, int merge = 0)
            {
                return merge > 0
                    ? $"<Cell ss:MergeAcross=\"{merge}\" ss:StyleID=\"{sty}\"><Data ss:Type=\"Number\">{val}</Data></Cell>"
                    : $"<Cell ss:StyleID=\"{sty}\"><Data ss:Type=\"Number\">{val}</Data></Cell>";
            }


            string E(int merge = 0) =>
                merge > 0
                    ? $"<Cell ss:MergeAcross=\"{merge}\"><Data ss:Type=\"String\"></Data></Cell>"
                    : "<Cell><Data ss:Type=\"String\"></Data></Cell>";

            // ═══════════════════════════════════════
            // PHẦN 1: TIÊU ĐỀ
            // ═══════════════════════════════════════
            Gap();
            Row(40, T("hdr", "O'BEEF SOUP", FULL));
            Row(16, T("sub", "Nhà hàng Phở Bò Cao Cấp  |  Hệ thống quản lý bán hàng nội bộ", FULL));
            Gap();
            Row(28, T("sec", $"  BÁO CÁO DOANH THU  —  {from:dd/MM/yyyy} đến {to.Date:dd/MM/yyyy}", FULL));
            Gap();

            // Thông tin báo cáo (2 cột × 2 hàng): iL=2col, iV=1col → 3col × 2 = 6col
            Row(16,
                T("iL", "Người xuất:", 1) + T("iV", "Quản trị viên") +
                T("iL", "Ngày xuất:", 1)  + T("iV", $"{now:dd/MM/yyyy HH:mm}"));
            Row(16,
                T("iL", "Kỳ báo cáo:", 1) + T("iV", $"{from:dd/MM/yyyy} – {to.Date:dd/MM/yyyy}") +
                T("iL", "Số đơn HT:", 1)   + T("iV", $"{totalOrders} đơn"));
            if (!string.IsNullOrEmpty(search))
                Row(16, T("iL", "Bộ lọc:", 1) + T("iV", $"«{search}»", FULL - 2));
            Gap();

            // ═══════════════════════════════════════
            // PHẦN 2: KPI — 3 cặp label+value, mỗi cặp chiếm 2 cột (6÷3=2)
            // ═══════════════════════════════════════
            Row(22, T("sec", "  TỔNG QUAN DOANH THU", FULL));

            // Labels: 3 × MergeAcross=1 = 3 × 2col = 6col ✓
            Row(18,
                T("kL", "TỔNG DOANH THU (đ)", 1) +
                T("kL", "SỐ ĐƠN HOÀN THÀNH", 1)  +
                T("kL", "GIÁ TRỊ TRUNG BÌNH / ĐƠN (đ)", 1));
            // Values
            Row(32,
                N("kV",  totalRevenue, 1) +
                NI("kN", totalOrders,  1) +
                N("kV",  avgValue,     1));

            Gap();

            // Labels hàng 2: COD / VNPAY / MOMO
            Row(18,
                T("kL", $"COD — Tiền mặt ({codCount} đơn)", 1) +
                T("kL", $"VN Pay ({vnpCount} đơn)", 1)          +
                T("kL", $"Ví MoMo ({momoCount} đơn)", 1));
            // Values
            Row(32,
                N("kV", codRev,  1) +
                N("kV", vnpRev,  1) +
                N("kV", momoRev, 1));

            Gap();

            // ═══════════════════════════════════════
            // PHẦN 3: BẢNG CHI TIẾT ĐƠN HÀNG
            // ═══════════════════════════════════════
            Row(22, T("sec", $"  CHI TIẾT ĐƠN HÀNG  ({totalOrders} đơn)", FULL));
            Row(24,
                T("tH", "STT")           +
                T("tH", "Mã đơn hàng")   +
                T("tH", "Khách hàng")    +
                T("tH", "Ngày đặt")      +
                T("tH", "Thanh toán")    +
                T("tH", "Tổng tiền (đ)"));

            for (int i = 0; i < orders.Count; i++)
            {
                var o   = orders[i];
                bool alt = i % 2 == 1;
                var sD  = alt ? "dO" : "dE";
                var sM  = alt ? "mO" : "mE";
                var sN  = alt ? "nO" : "nE";
                var met = o.PaymentMethod switch
                {
                    "COD"   => "Tiền mặt",
                    "VNPAY" => "VN Pay",
                    "MOMO"  => "Ví MoMo",
                    _       => o.PaymentMethod ?? ""
                };
                Row(18,
                    NI(sN, i + 1) +
                    T(sD, o.OrderNumber ?? "") +
                    T(sD, o.Customer?.FullName ?? "") +
                    T(sD, o.OrderDate.ToString("dd/MM/yyyy HH:mm")) +
                    T(sD, met) +
                    N(sM, o.TotalAmount));
            }

            // Tổng cộng: 4 col trống + label + value = 6 col ✓
            Row(28,
                E(3) +
                T("totL", $"TỔNG CỘNG  ({totalOrders} đơn)") +
                N("totV", totalRevenue));

            Gap();

            // ═══════════════════════════════════════
            // PHẦN 4: FOOTER
            // ═══════════════════════════════════════
            Row(14, T("ftr", $"Xuất tự động từ hệ thống O'Beef Soup lúc {now:dd/MM/yyyy HH:mm:ss}. Chỉ bao gồm đơn Hoàn thành.", FULL));

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

using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using OBeefSoup.Models;

namespace OBeefSoup.Services
{
    public class MoMoService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public MoMoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<MoMoPaymentResponse?> CreatePaymentAsync(HttpContext context, Order order)
        {
            var endpoint = _configuration["MoMo:Endpoint"];
            var partnerCode = _configuration["MoMo:PartnerCode"];
            var accessKey = _configuration["MoMo:AccessKey"];
            var secretKey = _configuration["MoMo:SecretKey"];

            // redirectUrl: trang người dùng sẽ được chuyển đến sau thanh toán
            var redirectUrl = $"{context.Request.Scheme}://{context.Request.Host}/Checkout/MoMoReturn";
            // ipnUrl: MoMo gọi server-to-server để thông báo kết quả (phải là endpoint riêng)
            var ipnUrl = $"{context.Request.Scheme}://{context.Request.Host}/Checkout/MoMoIPN";

            // orderId phải unique mỗi lần tạo
            var orderId = order.OrderId.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            var requestId = Guid.NewGuid().ToString();
            var extraData = "";
            var orderInfo = "Thanh toan don hang: " + order.OrderNumber;
            var amount = ((long)order.TotalAmount).ToString();
            var requestType = "payWithMethod";

            // Chữ ký theo đúng thứ tự bảng chữ cái như MoMo yêu cầu:
            // accessKey, amount, extraData, ipnUrl, orderId, orderInfo, partnerCode, redirectUrl, requestId, requestType
            var rawHash = $"accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}&redirectUrl={redirectUrl}&requestId={requestId}&requestType={requestType}";

            var signature = SignHMACSHA256(rawHash, secretKey!);

            var requestData = new
            {
                partnerCode,
                partnerName = "O'BeefSoup",
                storeId = "OBeefSoupStore",
                requestId,
                amount = long.Parse(amount),
                orderId,
                orderInfo,
                redirectUrl,
                ipnUrl,
                lang = "vi",
                extraData,
                requestType,
                signature
            };

            var content = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[MoMo] Request rawHash: {rawHash}");
                Console.WriteLine($"[MoMo] Response: {responseContent}");

                var result = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent);
                if (result != null && result.ResultCode != 0)
                {
                    Console.WriteLine($"[MoMo] Error ({result.ResultCode}): {result.Message}");
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MoMo] Exception: {ex.Message}");
                return new MoMoPaymentResponse { ResultCode = -1, Message = ex.Message };
            }
        }

        /// <summary>
        /// Xác minh chữ ký callback từ MoMo để đảm bảo tính toàn vẹn dữ liệu.
        /// </summary>
        public bool VerifySignature(MoMoCallbackParams p, string? secretKey)
        {
            if (string.IsNullOrEmpty(secretKey)) return false;

            var accessKey = _configuration["MoMo:AccessKey"];
            var partnerCode = _configuration["MoMo:PartnerCode"];

            // Thứ tự theo MoMo callback signature spec
            var rawHash = $"accessKey={accessKey}&amount={p.Amount}&extraData={p.ExtraData}&message={p.Message}&orderId={p.OrderId}&orderInfo={p.OrderInfo}&orderType={p.OrderType}&partnerCode={partnerCode}&payType={p.PayType}&requestId={p.RequestId}&responseTime={p.ResponseTime}&resultCode={p.ResultCode}&transId={p.TransId}";

            var expectedSignature = SignHMACSHA256(rawHash, secretKey);
            return string.Equals(p.Signature, expectedSignature, StringComparison.OrdinalIgnoreCase);
        }

        private string SignHMACSHA256(string message, string key)
        {
            byte[] keyByte = Encoding.UTF8.GetBytes(key);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }
    }

    public class MoMoPaymentResponse
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public long ResponseTime { get; set; }
        public string Message { get; set; } = string.Empty;
        public int ResultCode { get; set; }
        public string PayUrl { get; set; } = string.Empty;
        public string Deeplink { get; set; } = string.Empty;
        public string QrCodeUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// Tham số MoMo gửi về callback (cả redirectUrl và ipnUrl).
    /// </summary>
    public class MoMoCallbackParams
    {
        public string PartnerCode { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
        public string RequestId { get; set; } = string.Empty;
        public long Amount { get; set; }
        public string OrderInfo { get; set; } = string.Empty;
        public string OrderType { get; set; } = string.Empty;
        public string TransId { get; set; } = string.Empty;
        public int ResultCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string PayType { get; set; } = string.Empty;
        public long ResponseTime { get; set; }
        public string ExtraData { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}

# Tích hợp Thanh toán MoMo (MoMo Payment v2 API)

File này giải thích chi tiết cách tích hợp cổng thanh toán MoMo vào dự án ASP.NET Core OBeefSoup.  
**Không có bất kỳ thay đổi nào ảnh hưởng đến project** — đây chỉ là tài liệu mô tả.

---

## 1. Cấu hình (`appsettings.json`)

```json
"MoMo": {
  "PartnerCode": "MOMO",
  "AccessKey": "F8BBA842ECF85",
  "SecretKey": "K951B6PE1waDMi640xX08PD3vg6EkVlz",
  "Endpoint": "https://test-payment.momo.vn/v2/gateway/api/create"
}
```

| Key | Mô tả |
|-----|-------|
| `PartnerCode` | Mã đối tác do MoMo cấp |
| `AccessKey` | Dùng để tạo chữ ký (raw hash), **không dùng để mã hóa** |
| `SecretKey` | Khóa bí mật dùng với HMAC-SHA256 để ký request |
| `Endpoint` | URL API sandbox. Production: `https://payment.momo.vn/v2/gateway/api/create` |

> **Lưu ý**: Credentials trên là **sandbox/test**. Khi lên production cần thay bằng credentials thật từ MoMo Business Portal.

---

## 2. Luồng thanh toán tổng quan

```
User chọn MoMo → PlaceOrder → MoMoService.CreatePaymentAsync()
    → Gọi MoMo API → Nhận payUrl
    → Redirect user đến payUrl (trang thanh toán MoMo)
    → User thanh toán
    → MoMo redirect về /Checkout/MoMoReturn  (browser redirect)
    → MoMo gọi server /Checkout/MoMoIPN       (server-to-server, nền)
    → Cập nhật trạng thái đơn hàng → Hiển thị Confirmation
```

---

## 3. Tạo request thanh toán (`MoMoService.cs`)

### 3.1. Tham số bắt buộc

```csharp
var orderId    = order.OrderId + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"); // unique mỗi lần
var requestId  = Guid.NewGuid().ToString();  // unique, khác orderId
var amount     = ((long)order.TotalAmount).ToString(); // phải là long (VND nguyên)
var requestType = "payWithMethod"; // xem mục 3.3
```

### 3.2. Tạo chữ ký HMAC-SHA256

MoMo yêu cầu các tham số được **sắp xếp đúng thứ tự bảng chữ cái** trước khi ký:

```
accessKey={accessKey}&amount={amount}&extraData={extraData}&ipnUrl={ipnUrl}
&orderId={orderId}&orderInfo={orderInfo}&partnerCode={partnerCode}
&redirectUrl={redirectUrl}&requestId={requestId}&requestType={requestType}
```

```csharp
// Ký bằng HMAC-SHA256 với SecretKey
var signature = SignHMACSHA256(rawHash, secretKey);

private string SignHMACSHA256(string message, string key) {
    byte[] keyByte = Encoding.UTF8.GetBytes(key);
    byte[] msgBytes = Encoding.UTF8.GetBytes(message);
    using var hmac = new HMACSHA256(keyByte);
    return BitConverter.ToString(hmac.ComputeHash(msgBytes))
                       .Replace("-", "").ToLower();
}
```

> ⚠️ **Sai thứ tự tham số = lỗi 11007 (Invalid signature)**. Phải đúng ABC.

### 3.3. Các loại `requestType`

| Giá trị | Mô tả |
|---------|-------|
| `captureWallet` | Chỉ hiện QR — cần quét bằng app MoMo |
| `payWithATM` | Chỉ hiện form nhập thẻ ATM/ngân hàng |
| `payWithMethod` | **Hiện đang dùng** — cho phép user chọn phương thức (QR, ATM, thẻ tín dụng) |

### 3.4. Gửi request và nhận `payUrl`

```csharp
var response = await _httpClient.PostAsync(endpoint, content);
var result = JsonConvert.DeserializeObject<MoMoPaymentResponse>(responseContent);

// Redirect user đến trang thanh toán MoMo
if (result.ResultCode == 0 && !string.IsNullOrEmpty(result.PayUrl))
    return Redirect(result.PayUrl);
```

---

## 4. Xử lý callback từ MoMo

MoMo gửi kết quả về theo **hai kênh song song**:

### 4.1. Redirect (browser) — `/Checkout/MoMoReturn`

```csharp
// GET — MoMo chuyển hướng browser của user về đây
public async Task<IActionResult> MoMoReturn([FromQuery] MoMoCallbackParams p)
```

- Nhận toàn bộ tham số MoMo qua query string (`?orderId=...&resultCode=...`)
- **Verify signature trước** để chống giả mạo
- Cập nhật trạng thái đơn → redirect đến trang Confirmation

### 4.2. IPN (server-to-server) — `/Checkout/MoMoIPN`

```csharp
// POST — MoMo gọi trực tiếp server, không qua browser
[HttpPost]
public async Task<IActionResult> MoMoIPN([FromBody] MoMoCallbackParams p)
```

- MoMo tự gọi ngầm khi giao dịch hoàn tất (đáng tin cậy hơn redirect)
- Dùng để **đồng bộ trạng thái** khi browser bị đóng giữa chừng
- Phải return `200 OK` để MoMo biết đã nhận được

> ⚠️ **IPN không hoạt động trên localhost** vì MoMo server không thể kết nối vào máy dev. Dùng **ngrok** để test IPN: `ngrok http https://localhost:7175`

### 4.3. Verify Signature callback

```csharp
// Thứ tự tham số callback khác với request gốc!
var rawHash = $"accessKey={accessKey}&amount={p.Amount}&extraData={p.ExtraData}" +
              $"&message={p.Message}&orderId={p.OrderId}&orderInfo={p.OrderInfo}" +
              $"&orderType={p.OrderType}&partnerCode={partnerCode}&payType={p.PayType}" +
              $"&requestId={p.RequestId}&responseTime={p.ResponseTime}" +
              $"&resultCode={p.ResultCode}&transId={p.TransId}";

var expected = SignHMACSHA256(rawHash, secretKey);
return string.Equals(p.Signature, expected, StringComparison.OrdinalIgnoreCase);
```

---

## 5. Mã kết quả MoMo (`resultCode`)

| Code | Ý nghĩa | Xử lý |
|------|---------|-------|
| `0` | Thành công | Cập nhật `PaymentStatus = Paid` |
| `1005` | QR hết hạn / không hợp lệ | Sandbox limitation — dùng `payWithMethod` |
| `11007` | Chữ ký không hợp lệ | Kiểm tra thứ tự ABC trong rawHash, kiểm tra credentials |
| `1006` | User từ chối thanh toán | Cập nhật `PaymentStatus = Failed` |
| khác | Thất bại | Cập nhật `PaymentStatus = Failed` |

---

## 6. Test Sandbox

**Thẻ ATM test:**
| Trường | Giá trị |
|--------|---------|
| Số thẻ | `9704000000000018` |
| Tên chủ thẻ | `NGUYEN VAN A` |
| Ngày phát hành | `03/07` |
| OTP | `OTP` |

**Tài khoản MoMo test app:**
| Trường | Giá trị |
|--------|---------|
| Số điện thoại | `0000000000` |
| Mật khẩu / OTP | `000000` |

---

## 7. Lên Production

1. Đăng ký tài khoản merchant tại [business.momo.vn](https://business.momo.vn)
2. Thay credentials trong `appsettings.json` (hoặc dùng Azure Key Vault / biến môi trường)
3. Đổi endpoint sang: `https://payment.momo.vn/v2/gateway/api/create`
4. Đảm bảo `redirectUrl` và `ipnUrl` dùng domain HTTPS thật (không phải localhost)

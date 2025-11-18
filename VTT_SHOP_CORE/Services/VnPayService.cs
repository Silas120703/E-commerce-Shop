using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Text;
using VTT_SHOP_CORE.Helpers;
using VTT_SHOP_DATABASE.Repositories;
using VTT_SHOP_SHARED.Interfaces.Services;
using System.Net; 

namespace VTT_SHOP_CORE.Services
{
    public class VnPayService : IService
    {
        private readonly IConfiguration _config;
        private readonly OrderRepository _orderRepository;

        public VnPayService(IConfiguration config, OrderRepository orderRepository)
        {
            _config = config;
            _orderRepository = orderRepository;
        }

        public async Task<string?> CreatePaymentUrl(long orderId, string ipAddress)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order == null)
            {
                return null;
            }

            var vnPaySettings = _config.GetSection("VnPaySettings");
            var tmnCode = vnPaySettings["TmnCode"];
            var hashSecret = vnPaySettings["HashSecret"];
            var baseUrl = vnPaySettings["BaseUrl"];
            var returnUrl = vnPaySettings["ReturnUrl"];

            var pay = new SortedList<string, string>(new VnPayComparer())
            {
                { "vnp_Version", "2.1.0" },
                { "vnp_Command", "pay" },
                { "vnp_TmnCode", tmnCode! },
                { "vnp_Amount", ((long)order.FinalAmount * 100).ToString() },
                { "vnp_CreateDate", DateTime.Now.ToString("yyyyMMddHHmmss") },
                { "vnp_CurrCode", "VND" },
                { "vnp_IpAddr", ipAddress },
                { "vnp_Locale", "vn" },
                { "vnp_OrderInfo", $"Thanh toan don hang {order.Id}" },
                { "vnp_OrderType", "other" },
                { "vnp_ReturnUrl", returnUrl! },
                { "vnp_TxnRef", order.Id.ToString() }
            };


            var dataToHash = new StringBuilder();

            foreach (var (key, value) in pay)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    dataToHash.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
                }
            }

            string signData = dataToHash.ToString().TrimEnd('&');

            string secureHash = VnPayLibrary.HmacSHA512(hashSecret!, signData);

            string finalUrl = $"{baseUrl}?{signData}&vnp_SecureHash={WebUtility.UrlEncode(secureHash)}";

            return finalUrl;
        }

        public bool ValidateSignature(IQueryCollection query, out string RspCode, out string Message)
        {
            var vnPaySettings = _config.GetSection("VnPaySettings");
            var hashSecret = vnPaySettings["HashSecret"];
            var rspCode = query["vnp_ResponseCode"]!;

            RspCode = rspCode;

            var pay = new SortedList<string, string>(new VnPayComparer());
            foreach (var (key, value) in query)
            {
                if (!string.IsNullOrEmpty(key) && key.StartsWith("vnp_") && key != "vnp_SecureHash" && key != "vnp_SecureHashType")
                {
                    pay.Add(key, value.ToString());
                }
            }

            var dataToHash = new StringBuilder();
            foreach (var (key, value) in pay)
            {
                if (!string.IsNullOrEmpty(value))
                {
                    dataToHash.Append(WebUtility.UrlEncode(key) + "=" + WebUtility.UrlEncode(value) + "&");
                }
            }
            string signData = dataToHash.ToString().TrimEnd('&');

            string vnp_SecureHash = query["vnp_SecureHash"]!;

            string secureHash = VnPayLibrary.HmacSHA512(hashSecret!, signData);

            if (secureHash.Equals(vnp_SecureHash, StringComparison.InvariantCultureIgnoreCase))
            {
                Message = RspCode == "00" ? "Giao dịch thành công" : "Giao dịch thất bại";
                return true;
            }

            Message = "Chữ ký không hợp lệ";
            return false;
        }
    }
}
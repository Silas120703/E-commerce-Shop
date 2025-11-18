using System.Security.Cryptography;
using System.Text;
using System.Globalization;
using System.Net.Sockets;
using System.Net;
using Microsoft.AspNetCore.Http; 

namespace VTT_SHOP_CORE.Helpers
{
    public static class VnPayLibrary
    {
        public static string HmacSHA512(string key, string inputData)
        {
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);

                return BitConverter.ToString(hashValue).Replace("-", "");
            }
        }

        public static string GetIpAddress(HttpContext context)
        {
            var ipAddress = string.Empty;
            try
            {
                var remoteIpAddress = context.Connection.RemoteIpAddress;

                if (remoteIpAddress != null)
                {
                    if (remoteIpAddress.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        remoteIpAddress = Dns.GetHostEntry(remoteIpAddress).AddressList
                            .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                    }

                    if (remoteIpAddress != null) ipAddress = remoteIpAddress.ToString();
                }
            }
            catch (Exception)
            {
                ipAddress = "127.0.0.1";
            }

            return ipAddress ?? "127.0.0.1";
        }
    }

    public class VnPayComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            if (x == y) return 0;
            if (x == null) return -1;
            if (y == null) return 1;
            return string.Compare(x, y, StringComparison.Ordinal);
        }
    }
}
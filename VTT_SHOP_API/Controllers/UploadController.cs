using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VTT_SHOP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ApiControllerBase
    {
        private readonly Cloudinary _cloudinary;
        private readonly IConfiguration _config;

        public UploadController(Cloudinary cloudinary, IConfiguration config)
        {
            _cloudinary = cloudinary;
            _config = config;
        }
        //[Authorize]
        [HttpPost("generate-signature")]
        public IActionResult GenerateSignature()
        {
            var apiKey = _config["Cloudinary:ApiKey"];
            var apiSecret = _config["Cloudinary:ApiSecret"];
            var cloudName = _config["Cloudinary:CloudName"];
            if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret) || string.IsNullOrEmpty(cloudName))
            {
                return BadRequest(new { Message = "Cloudinary configuration is missing." });
            }
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            string stringToSign = $"timestamp={timestamp}{apiSecret}";
            string signature;
            using (var sha1 = System.Security.Cryptography.SHA1.Create())
            {
                var hashBytes = sha1.ComputeHash(System.Text.Encoding.UTF8.GetBytes(stringToSign));
                signature = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
            return Ok(new
            {
                apiKey,
                timestamp,
                signature,
                cloudName
            });
        }
    }
}

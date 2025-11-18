using FluentResults;
using Microsoft.AspNetCore.Mvc;
using System.Net.Sockets;
using System.Net;
using System.Security.Claims;
using VTT_SHOP_CORE.Errors;

namespace VTT_SHOP_API.Controllers
{
    public abstract class ApiControllerBase : ControllerBase
    {
        protected long CurrentUserId
        {
            get
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim != null && long.TryParse(userIdClaim.Value, out long userId))
                {
                    return userId;
                }
                return 0;
            }
        }

        protected string CurrentUserIpAddress
        {
            get
            {
                var ipAddress = string.Empty;
                try
                {
                    var remoteIpAddress = HttpContext.Connection.RemoteIpAddress;
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

        protected IActionResult HandleResult<T>(Result<T> result)
        {
            if (result.IsSuccess)
            {
                if (result.Value == null && result.Successes.Any())
                {
                    return Ok(new { Message = result.Successes.FirstOrDefault()?.Message ?? "Success" });
                }
                return Ok(result.Value);
            }

            return ProcessErrorResult(result.ToResult());
        }

        protected IActionResult HandleResult(Result result)
        {
            if (result.IsSuccess)
            {
                return Ok(new { Message = result.Successes.FirstOrDefault()?.Message ?? "Success" });
            }

            return ProcessErrorResult(result);
        }

        private IActionResult ProcessErrorResult(ResultBase result)
        {
            var error = result.Errors.FirstOrDefault();
            var message = new { Message = error?.Message ?? "An error occurred." };

            if (result.HasError<NotFoundError>())
            {
                return NotFound(message);
            }

            if (result.HasError<AccountNotVerifiedError>())
            {
                return Unauthorized(message);
            }

            if (result.HasError<InvalidCredentialsError>())
            {
                return BadRequest(message);
            }

            return BadRequest(message);
        }
    }
}


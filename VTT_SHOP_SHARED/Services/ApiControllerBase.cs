using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace VTT_SHOP_SHARED.Services
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
    }
}
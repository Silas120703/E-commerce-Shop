using FluentResults;

namespace VTT_SHOP_CORE.Errors
{
    public class AccountNotVerifiedError : Error
    {
        public AccountNotVerifiedError(string message) : base(message)
        {
        }
    }
}

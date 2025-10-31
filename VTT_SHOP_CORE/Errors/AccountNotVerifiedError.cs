using FluentResults;

namespace VTT_SHOP_CORE.Errors
{
    public class AccountNotVerifiedError : Error
    {
        public AccountNotVerifiedError() : base("Account not activated.")
        {
        }
    }
}

using FluentResults;

namespace VTT_SHOP_CORE.Errors
{
    public class InvalidCredentialsError : Error
    {
        public InvalidCredentialsError() : base("Incorrect login or password.")
        {
        }
    }
}

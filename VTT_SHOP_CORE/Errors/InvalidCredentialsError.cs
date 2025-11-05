using FluentResults;

namespace VTT_SHOP_CORE.Errors
{
    public class InvalidCredentialsError : Error
    {
        public InvalidCredentialsError(string message) : base(message)
        {
        }
    }
}

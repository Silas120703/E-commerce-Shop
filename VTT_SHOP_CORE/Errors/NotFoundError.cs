using FluentResults;

namespace VTT_SHOP_CORE.Errors
{
    public class NotFoundError : Error
    {
        public NotFoundError(string message) : base(message)
        {
        }  
    }
}

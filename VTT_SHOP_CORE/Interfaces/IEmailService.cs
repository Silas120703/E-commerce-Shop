namespace VTT_SHOP_CORE.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string verificationCode);
    }
}

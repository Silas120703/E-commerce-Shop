using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class EmailVerificationToken : EntityBase
    {
        public long UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiredAt { get; set; }
        public bool IsUsed { get; set; } = false;
        public User? User { get; set; }
    }
}

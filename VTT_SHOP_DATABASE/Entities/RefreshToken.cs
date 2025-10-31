using System.ComponentModel.DataAnnotations.Schema;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    [Table("RefreshTokens")]
    public class RefreshToken : EntityBase
    {
        public long UserId { get; set; }
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryAt { get; set; }
        public DateTime? RevokeAt { get; set; }
        public string? ReplanceByToken { get; set; }
        public bool IsActive => RevokeAt==null && ExpiryAt > DateTime.UtcNow;
        public virtual User User { get; set; } = null!;
    }
}

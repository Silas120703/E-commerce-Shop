using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{

    [Table("Users")]
    [Index(nameof(Email), IsUnique = true)]
    public partial class User : EntityBase
    {
        [Column(TypeName = "nvarchar(250)")]
        public string Name { get; set; } = null!;
        public string Gender { get; set; } = null!;
        [Column(TypeName = "nvarchar(250)")]
        public string? ProfilePicture { get; set; }
        public DateOnly? Birthday { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public string Email { get; set; } = null!;
        [Column(TypeName = "nvarchar(15)")]
        public string Phone { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsEmailVerified { get; set; } = false;
        public virtual UserRole? UserRole { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
        public EmailVerificationToken? EmailVerificationToken { get; set; }
    }
}


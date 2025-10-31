using System.ComponentModel.DataAnnotations.Schema;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    [Table("UserRoles")]
    public partial class UserRole : EntityBase
    {
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public virtual User? User { get; set; }
        public virtual Role? Role { get; set; }
    }
}

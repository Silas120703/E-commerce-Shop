using System.ComponentModel.DataAnnotations.Schema;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    [Table("Roles")]
    public partial class Role:EntityBase
    {
        public string? RoleName { get; set; }
        public virtual User? User { get; set; }
    }
}

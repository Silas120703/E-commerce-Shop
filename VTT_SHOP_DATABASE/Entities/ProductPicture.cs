using System.ComponentModel.DataAnnotations.Schema;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    [Table("ProductPictures")]
    public partial class ProductPicture : EntityBase
    {
        public long ProductId { get; set; }
        public string PictureUrl { get; set; } = string.Empty;
        public bool IsMain { get; set; }  
        public virtual Product? Product { get; set; }
        
    }
}

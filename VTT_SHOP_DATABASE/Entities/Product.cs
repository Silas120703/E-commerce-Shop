using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    [Table("Products")]
    [Index(nameof(Name),IsUnique =false)]
    [Index(nameof(Description), IsUnique = false)]
    public class Product : EntityBase
    {
        public string Name { get; set; } = null!;
        public string? SlugName { get; set; } 
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public double? Price { get; set; }
        public virtual ICollection<ProductPicture>? ProductPictures { get; set; }
    }
}

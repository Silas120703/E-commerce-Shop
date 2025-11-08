using VTT_SHOP_SHARED.Database.EntityBase;

namespace VTT_SHOP_DATABASE.Entities
{
    public class Address : EntityBase
    {
        public long UserId { get; set; }
        public bool IsDefault { get; set; }
        public string RecipientName { get; set; } 
        public string Phone { get; set; } 

        public string Street { get; set; } 

        public string Ward { get; set; }
        public string District { get; set; } 
        public string City { get; set; }

        public string PostalCode { get; set; } 
        public string Country { get; set; }
        public virtual User User { get; set; }
    }
}

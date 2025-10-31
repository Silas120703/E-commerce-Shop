
namespace VTT_SHOP_SHARED.Database.EntityBase
{
    public abstract class EntityBase : IEntityModel
    {
        public  long Id { get; set; }
        public  DateTime CreateAt { get; set; }
        public  DateTime? UpdateAt { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeleteAt { get; set; }
        public bool IsBlocked { get; set; } = false;
        public DateTime? BlockAt { get; set; }
    }
}

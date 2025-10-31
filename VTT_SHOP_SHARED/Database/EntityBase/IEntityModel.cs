namespace VTT_SHOP_SHARED.Database.EntityBase
{
    public interface IEntityModel
    {
        long Id { get; set; }
        DateTime CreateAt { get; set; }
        DateTime? UpdateAt { get; set; }
    }
}

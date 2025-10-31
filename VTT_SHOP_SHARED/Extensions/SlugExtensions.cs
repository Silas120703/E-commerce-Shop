using Slugify;

namespace VTT_SHOP_SHARED.Extensions
{
    public static class SlugExtensions
    {
        private static readonly SlugHelper _slugHelper = new SlugHelper();
        public static string ToSlug(this string input,long id)
        {
            var slug = _slugHelper.GenerateSlug(input);
            return slug+"-"+id;
        }
    }
}

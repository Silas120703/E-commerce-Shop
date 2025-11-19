using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.DTOs;

namespace VTT_SHOP_SHARED.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<PagedResult<T>> ToPagedListAsync<T>(
            this IQueryable<T> source,
            int pageIndex,
            int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>(items, count, pageIndex, pageSize);
        }
    }
}
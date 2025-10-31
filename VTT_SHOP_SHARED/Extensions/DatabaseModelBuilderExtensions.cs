using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using VTT_SHOP_SHARED.Database.EntityBase;
using VTT_SHOP_SHARED.Helpers;

namespace VTT_SHOP_SHARED.Extensions
{
    public static class DatabaseModelBuilderExtensions
    {
        public static ModelBuilder RegisterDatabaseEntities(this ModelBuilder builder, string assemblyName)
        {
            var types = ApplicationHelper.GetApplicationClasses(assemblyName);
            types = types.Where(e => e.IsAssignableTo(typeof(IEntityModel))).ToList();
            foreach (var type in types)
            {
                var baseType = type.BaseType;
                var isIgnoreModel = baseType != null && baseType.IsAssignableTo(typeof(IEntityModel)) &&
                                    !baseType.IsAbstract && !baseType.IsInterface;

                if (!isIgnoreModel)
                {
                    if (type.IsAssignableTo(typeof(IEntityModel)))
                    {
                        builder.Entity(type);
                    }
                }
                else
                {
                    // do nothing on custom type
                    Console.WriteLine($"By pass type " + type.Name);
                }
            }
            return builder;
        }
    }
}

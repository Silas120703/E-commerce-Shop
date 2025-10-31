using System.Reflection;

namespace VTT_SHOP_SHARED.Helpers
{
    public class ApplicationHelper
    {
        public static IEnumerable<Type> GetApplicationClasses(string assemblyName)
        {
            var types = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .SelectMany(e => e.GetReferencedAssemblies().Select(s => s))
                .Where(e => e.FullName.Contains(assemblyName))
                .DistinctBy(e => e.FullName)
                .Select(Assembly.Load)
                .SelectMany(e => e.GetExportedTypes())
                .Where(e => !e.IsAbstract && !e.IsInterface);
            return types;
        }
    }
}

using System.Linq;
using System.Reflection;
using Charites.Windows.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace WpfMvcApp
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddControllers(this IServiceCollection services)
            => typeof(WpfController).Assembly.DefinedTypes
                .Concat(typeof(ServiceExtensions).Assembly.DefinedTypes)
                .Where(type => type.GetCustomAttributes<ViewAttribute>(true).Any())
                .Aggregate(services, (s, t) => s.AddTransient(t));
    }
}

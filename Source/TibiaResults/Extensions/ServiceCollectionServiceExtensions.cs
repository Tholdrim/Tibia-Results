using Microsoft.Extensions.DependencyInjection;
using TibiaResults.Interfaces;

namespace TibiaResults.Extensions
{
    internal static class ServiceCollectionServiceExtensions
    {
        public static IServiceCollection AddApplicationService<TApplicationService>(this IServiceCollection services)
            where TApplicationService : class, IApplicationService
        {
            return services
                .AddSingleton<IApplicationService, TApplicationService>()
                .AddSingleton(serviceProvider => new Lazy<IApplicationService>(() => serviceProvider.GetRequiredService<IApplicationService>()));
        }
    }
}

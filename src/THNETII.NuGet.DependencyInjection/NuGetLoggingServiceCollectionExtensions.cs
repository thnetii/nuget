using Microsoft.Extensions.DependencyInjection;
using THNETII.Common;

namespace THNETII.NuGet.Logging.DependencyInjection
{
    using NuGetLogger = global::NuGet.Common.ILogger;

    public static class NuGetLoggingServiceCollectionExtensions
    {
        public static IServiceCollection AddNuGetLogging(this IServiceCollection services)
        {
            services.ThrowIfNull(nameof(services))
                .AddSingleton<NuGetLogger, NuGetWrapperLogger>()
                ;
            return services;
        }
    }
}

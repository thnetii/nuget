using System.Collections.Generic;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using NuGet.Common;
using NuGet.Configuration;
using NuGet.Frameworks;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using NuGet.Versioning;
using THNETII.Common;
using THNETII.NuGet.Logging;
using THNETII.NuGet.Logging.DependencyInjection;

namespace THNETII.NuGet.PackageSearchClient
{
    using NuGetLogger = global::NuGet.Common.ILogger;
    using MicrosoftNullLogger = Microsoft.Extensions.Logging.Abstractions.NullLogger;

    public static class Program
    {
        public static Task<int> Main(string[] args)
        {
            var commandHandler = CommandHandler.Create<IHost, CancellationToken>(RunAsync);
            var commandDefinition = new CommandDefinition(commandHandler);
            var commandParser = new CommandLineBuilder(commandDefinition.RootCommand)
                .UseDefaults()
                .UseHost(Host.CreateDefaultBuilder, host =>
                {
                    host.ConfigureServices(services => services.AddSingleton(commandDefinition));
                    ConfigureHost(host);
                })
                .Build();
            return commandParser.InvokeAsync(args);
        }

        private static async Task RunAsync(IHost host, CancellationToken cancelToken)
        {
            var arguments = host.Services.GetRequiredService<CommandArguments>();
            var logger = host.Services.GetService<NuGetLogger>()
                ?? NullLogger.Instance;
            var sourceRepositoryProvider = host.Services
                .GetRequiredService<ISourceRepositoryProvider>();
            using var sourceCacheContext = new SourceCacheContext();
            foreach (var sourceRepository in sourceRepositoryProvider.GetRepositories())
            {
                var findPkgByIdResource = await sourceRepository
                    .GetResourceAsync<FindPackageByIdResource>(
                        cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                NuGetVersion version;
                if (arguments.PackageVersion is null)
                {
                    var nugetVersions = await findPkgByIdResource
                        .GetAllVersionsAsync(arguments.PackageName,
                            sourceCacheContext, logger, cancelToken)
                        .ConfigureAwait(continueOnCapturedContext: false);
                    version = nugetVersions
                        .OrderByDescending(v => v, VersionComparer.VersionRelease)
                        .FirstOrDefault(v => v.IsPrerelease == arguments.IncludePreRelease);
                }
                else
                    version = arguments.PackageVersion;
                var depInfo = await findPkgByIdResource.GetDependencyInfoAsync(
                        arguments.PackageName, version, sourceCacheContext,
                        logger, cancelToken)
                    .ConfigureAwait(continueOnCapturedContext: false);
                if (!(depInfo is null))
                {
                    
                }
            }
        }

        private static void ConfigureHost(IHostBuilder host) =>
            host.ConfigureServices(ConfigureServices);

        private static void ConfigureServices(HostBuilderContext context,
            IServiceCollection services)
        {
            services.AddSingleton<CommandArguments>();
            services.AddNuGetLogging();
            services.AddSingleton(serviceProvider =>
            {
                var root = serviceProvider.GetService<CommandArguments>()?
                    .SettingsRoot;
                return Settings.LoadDefaultSettings(root);
            });
            services.AddSingleton<IPackageSourceProvider, PackageSourceProvider>();
            services.AddSingleton<ISourceRepositoryProvider>(serviceProvider =>
            {
                return new SourceRepositoryProvider(
                    serviceProvider.GetRequiredService<IPackageSourceProvider>(),
                    Repository.Provider.GetCoreV3()
                    );
            });
        }
    }
}

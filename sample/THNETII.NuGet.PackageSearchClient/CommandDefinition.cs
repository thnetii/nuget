using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using NuGet.Versioning;

namespace THNETII.NuGet.PackageSearchClient
{
    internal class CommandDefinition
    {
        public CommandDefinition(ICommandHandler commandHandler)
        {
            RootCommand = new RootCommand
            {
                Handler = commandHandler
            };

            PackageArgument = new Argument<string>
            {
                Arity = ArgumentArity.ExactlyOne,
                Name = "PACKAGE",
                Description = "The package name to search for",
            };
            RootCommand.AddArgument(PackageArgument);

            VersionOption = new Option("--package-version")
            {
                Description = "Optional. Match specific package version. Find highest if omitted.",
                Argument = new Argument<NuGetVersion>(ConvertArgumentToNuGetVersion)
                {
                    Name = "VERSION",
                    Description = "The NuGet version to match",
                }
            };
            VersionOption.AddAlias("-v");
            RootCommand.AddOption(VersionOption);

            PreReleaseOption = new Option("--include-prerelease")
            {
                Description = "Include pre-release package version.",
                Argument = new Argument<bool>("BOOL")
            };
            PreReleaseOption.AddAlias("--pre");
            PreReleaseOption.AddAlias("-p");
            RootCommand.AddOption(PreReleaseOption);

            SettingsRootOption = new Option("--settings-root")
            {
                Description = "Root directory for NuGet settings",
                Argument = new Argument<string>("ROOT")
            };
            RootCommand.AddOption(SettingsRootOption);
        }

        public RootCommand RootCommand { get; }
        public Argument PackageArgument { get; }
        public Option VersionOption { get; }
        public Option PreReleaseOption { get; }
        public Option SettingsRootOption { get; }

        private static bool ConvertArgumentToNuGetVersion(SymbolResult symbol,
            out NuGetVersion value)
        {
            value = symbol.Tokens
                .Select(t => NuGetVersion.Parse(t.Value))
                .FirstOrDefault(v => !(v is null));
            return !(value is null);
        }
    }
}

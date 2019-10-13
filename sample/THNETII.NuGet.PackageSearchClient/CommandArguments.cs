using System;
using System.CommandLine;
using NuGet.Versioning;

namespace THNETII.NuGet.PackageSearchClient
{
    internal class CommandArguments
    {
        public CommandArguments(CommandDefinition definition,
            ParseResult parseResult) : base()
        {
            if (definition is null)
                throw new ArgumentNullException(nameof(definition));
            if (parseResult is null)
                throw new ArgumentNullException(nameof(parseResult));

            PackageName = parseResult.FindResultFor(definition.PackageArgument)
                .GetValueOrDefault<string>();
            PackageVersion = parseResult.FindResultFor(definition.VersionOption)?
                .GetValueOrDefault<NuGetVersion>();
            IncludePreRelease = parseResult.FindResultFor(definition.PreReleaseOption)?
                .GetValueOrDefault<bool>() ?? false;
            SettingsRoot = parseResult.FindResultFor(definition.SettingsRootOption)?
                .GetValueOrDefault<string>();
        }

        public string PackageName { get; }
        public NuGetVersion PackageVersion { get; }
        public bool IncludePreRelease { get; }
        public string SettingsRoot { get; }
    }
}

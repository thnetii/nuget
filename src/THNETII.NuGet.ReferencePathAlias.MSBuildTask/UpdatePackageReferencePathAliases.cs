using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using MSBuildTaskBase = Microsoft.Build.Utilities.Task;

namespace THNETII.NuGet.ReferencePathAlias.MSBuildTask
{
    [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
    [SuppressMessage("Performance", "CA1819: Properties should not return arrays")]
    public class UpdatePackageReferencePathAliases : MSBuildTaskBase
    {
        [Required]
        public ITaskItem[] PackageReference { get; set; }

        [Required, Output]
        public ITaskItem[] ReferencePath { get; set; }

        public override bool Execute()
        {
            var pkgRefs = PackageReference
                .Where(pkgRef => pkgRef.MetadataNames.OfType<string>().Contains("ReferencePathAlias", StringComparer.OrdinalIgnoreCase))
                .ToDictionary(pkgRef => pkgRef.ItemSpec, StringComparer.OrdinalIgnoreCase);
            var itemPairs = ReferencePath
                .Where(refPath => refPath.MetadataNames.OfType<string>().Contains("NuGetPackageId", StringComparer.OrdinalIgnoreCase))
                .Where(refPath => !refPath.MetadataNames.OfType<string>().Contains("Aliases", StringComparer.OrdinalIgnoreCase))
                .Select(refPath =>
                {
                    var pkgId = refPath.GetMetadata("NuGetPackageId");
                    _ = pkgRefs.TryGetValue(pkgId, out var pkgRef);
                    return Tuple.Create(refPath, pkgRef);
                })
                .Where(t => !(t.Item2 is null));
            var referencePathList = new List<ITaskItem>(ReferencePath.Length);
            foreach (var refPath in ReferencePath)
            {
                if (!(refPath.GetMetadata("NuGetPackageId") is string nugetPkgId) ||
                    !pkgRefs.TryGetValue(nugetPkgId, out var pkgRef))
                    continue;
                var alias = pkgRef.GetMetadata("ReferencePathAlias");
                Log.LogMessage(MessageImportance.Low, "ReferencePath.Aliases = '{0}' (<ReferencePath NuGetPackageId=\"{1}\" />)",
                    alias, nugetPkgId);

                var newRefPath = new TaskItem(refPath);
                newRefPath.SetMetadata("Aliases", alias);
                referencePathList.Add(newRefPath);
            }

            ReferencePath = referencePathList.ToArray();

            return true;
        }
    }
}

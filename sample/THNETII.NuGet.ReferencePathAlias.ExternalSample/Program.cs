extern alias AWSSDK_Extensions_NETCore_Setup;

using System;
using System.Diagnostics.CodeAnalysis;

using AwsConfigurationExtensions = AWSSDK_Extensions_NETCore_Setup::Microsoft.Extensions.Configuration.ConfigurationExtensions;

namespace THNETII.NuGet.ReferencePathAlias.ExternalSample
{
    [SuppressMessage("Globalization", "CA1303: Do not pass literals as localized parameters")]
    public static class Program
    {
        public static void Main()
        {
            Console.WriteLine("AWS Configuration Extensions");
            Console.WriteLine($"{nameof(AwsConfigurationExtensions.DEFAULT_CONFIG_SECTION)}: {AwsConfigurationExtensions.DEFAULT_CONFIG_SECTION}");
        }
    }
}

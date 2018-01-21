namespace THNETII.NuGet.Logging
{
    using MicrosoftLogLevel = Microsoft.Extensions.Logging.LogLevel;
    using NuGetLogLevel = global::NuGet.Common.LogLevel;

    public static class NuGetLogLevelExtensions
    {
        public static MicrosoftLogLevel GetMicrosoftLogLevel(this NuGetLogLevel logLevel)
        {
            switch (logLevel)
            {
                case NuGetLogLevel.Debug:
                case NuGetLogLevel.Verbose:
                    return MicrosoftLogLevel.Debug;
                case NuGetLogLevel.Information:
                case NuGetLogLevel.Minimal:
                    return MicrosoftLogLevel.Information;
                case NuGetLogLevel.Warning:
                    return MicrosoftLogLevel.Warning;
                case NuGetLogLevel.Error:
                    return MicrosoftLogLevel.Error;
                default: return (MicrosoftLogLevel)logLevel;
            }
        }
    }
}

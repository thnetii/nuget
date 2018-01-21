using Microsoft.Extensions.Logging;
using NuGet.Common;

namespace THNETII.NuGet.Logging
{
    public static class NuGetLogCodeExtensions
    {
        public static EventId GetMicrosoftEventId(this NuGetLogCode logCode)
            => new EventId((int)logCode, logCode.ToString());
    }
}

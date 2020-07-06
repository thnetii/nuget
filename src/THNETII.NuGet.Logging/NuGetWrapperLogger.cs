using System;
using System.Threading.Tasks;

using NuGet.Common;

namespace THNETII.NuGet.Logging
{
    using MicrosoftLogger = Microsoft.Extensions.Logging.ILogger<ILogger>;

    public class NuGetWrapperLogger : LoggerBase
    {
        private readonly MicrosoftLogger msLogger;

        public NuGetWrapperLogger(MicrosoftLogger logger)
        {
            msLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override void Log(ILogMessage message)
        {
            if (message is null)
                return;
            msLogger.Log(
                message.Level.GetMicrosoftLogLevel(),
                message.Code.GetMicrosoftEventId(),
                message, null,
                (msg, except) => msg.FormatWithCode()
                );
        }

        public override Task LogAsync(ILogMessage message)
            => Task.Run(() => Log(message));
    }
}

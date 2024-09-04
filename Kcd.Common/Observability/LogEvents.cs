using Microsoft.Extensions.Logging;

namespace Stayr.Backend.Common.Observability;

/// <summary>
/// Log event class contains extra event id's for advanced filtering.
/// </summary>
public static class LogEvents
{
    public static EventId TraceMessage { get; } = new((int)LogEventId.TraceMessage, nameof(Api) + "." + nameof(TraceMessage));

    /// <summary>
    /// Log events
    /// </summary>
    public static class Api
    {
        /// <summary>Process loop started.</summary>
        public static EventId LoginError { get; } = new((int)LogEventId.LoginError, nameof(Api) + "." + nameof(LoginError));
        public static EventId RefreshToken { get; } = new((int)LogEventId.RefreshToken, nameof(Api) + "." + nameof(RefreshToken));
        public static EventId RegistrationError { get; } = new((int)LogEventId.RegistrationError, nameof(Api) + "." + nameof(RegistrationError));
        public static EventId UnhandledError { get; } = new((int)LogEventId.UnhandledError, nameof(Api) + "." + nameof(UnhandledError));
        public static EventId AvatarNotFoundError { get; } = new((int)LogEventId.AvatarNotFoundError, nameof(Api) + "." + nameof(AvatarNotFoundError));
        public static EventId UnAuthorizedError { get; } = new((int)LogEventId.UnAuthorizedError, nameof(Api) + "." + nameof(UnAuthorizedError));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class Application
    {
        /// <summary>Process loop started.</summary>
        public static EventId ApplyWarning { get; } = new((int)LogEventId.ApplyWarning, nameof(Api) + "." + nameof(ApplyWarning));
        public static EventId ApproveWarning { get; } = new((int)LogEventId.ApplyWarning, nameof(Api) + "." + nameof(ApplyWarning));
        public static EventId RejectWarning { get; } = new((int)LogEventId.RejectWarning, nameof(Api) + "." + nameof(RejectWarning));
        public static EventId InvalidFileType { get; } = new((int)LogEventId.InvalidFileType, nameof(Api) + "." + nameof(InvalidFileType));
        public static EventId ExceededMaxFileSize { get; } = new((int)LogEventId.ExceededMaxFileSize, nameof(Api) + "." + nameof(ExceededMaxFileSize));
        public static EventId FileNotFoundError { get; } = new((int)LogEventId.FileNotFoundError, nameof(Api) + "." + nameof(FileNotFoundError));
        public static EventId StorageStrategyNotFoundError { get; } = new((int)LogEventId.StorageStrategyNotFoundError, nameof(Api) + "." + nameof(StorageStrategyNotFoundError));
    }
}


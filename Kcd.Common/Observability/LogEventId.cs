namespace Stayr.Backend.Common.Observability;

public enum LogEventId
{
    // General error event ids: 0 - 100
    Undefined = 0,
    TraceMessage = 1,
    LoginError = 2,
    RefreshToken = 3,
    RegistrationError = 4,
    UnhandledError = 5,
    AvatarNotFoundError = 6,
    UnAuthorizedError = 7,

    // Application error event ids 100 - 200
    ApplyWarning = 100,
    ApproveWarning = 101,
    RejectWarning = 102,
    InvalidFileType = 103,
    ExceededMaxFileSize = 104,
    FileNotFoundError = 105,
    StorageStrategyNotFoundError = 106,
}

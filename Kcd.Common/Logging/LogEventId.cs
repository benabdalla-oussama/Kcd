namespace Stayr.Backend.Common.Logging;

public enum LogEventId
{
    // General error event ids: 0 - 100
    Undefined = 0,
    TraceMessage = 1,
    LoginError = 2,
    RefreshToken = 3,
    RegistrationError = 4,
    UserUpdateError = 5,
    UserDeleteError = 6,
    RoleCreateError = 7,
    RoleUpdateError = 8,
    RoleDeleteError = 9,
    PasswordResetError = 10,
    PasswordResetTrace = 11,

    // Service bus
    ServiceBusTraceMessage = 101,
    ServiceBusError = 102,

    // Messaging
    MessagingTraceMessage = 1001,
    FlowProcessingServiceTrace = 1002,
    FlowProcessingServiceError = 1003,
    FlowCreationServiceError = 1004,
    FlowCreationServiceTrace = 1005,
    FlowCreationServiceWarning = 1006,
    FailedFlowProcessingServiceTrace = 1007,
    PendingFlowProcessingServiceTrace = 1008,

    // Scoring service 
    ScoringServiceError = 1101,
    ScoringServiceTraceMessage = 1102,
    ScoringApiError = 1103,
    ScoringApiWarning = 1103,

    // Data service 
    DataServiceError = 1201,
    DataServiceTraceMessage = 1202,
    DataApiError = 1203,
    DataApiWarning = 1204,

    // Theme service
    ThemeServiceTraceMessage = 1301,
    ThemeServiceError = 1302,
    ThemeApiError = 1303,
    ThemeApiWarning = 1304,
}

using Microsoft.Extensions.Logging;

namespace Stayr.Backend.Common.Logging;

/// <summary>
/// Log event class contains extra event id's for advanced filtering.
/// </summary>
public static class LogEvents
{
    /// <summary>
    /// Log events
    /// </summary>
    public static class Api
    {
        /// <summary>Process loop started.</summary>
        public static EventId TraceMessage { get; } = new((int)LogEventId.TraceMessage, nameof(Api) + "." + nameof(TraceMessage));
        public static EventId LoginError { get; } = new((int)LogEventId.LoginError, nameof(Api) + "." + nameof(LoginError));
        public static EventId RefreshToken { get; } = new((int)LogEventId.RefreshToken, nameof(Api) + "." + nameof(RefreshToken));
        public static EventId RegistrationError { get; } = new((int)LogEventId.RegistrationError, nameof(Api) + "." + nameof(RegistrationError));
        public static EventId UserUpdateError { get; } = new((int)LogEventId.UserUpdateError, nameof(Api) + "." + nameof(UserUpdateError));
        public static EventId UserDeleteError { get; } = new((int)LogEventId.UserDeleteError, nameof(Api) + "." + nameof(UserDeleteError));
        public static EventId RoleCreateError { get; } = new((int)LogEventId.RoleCreateError, nameof(Api) + "." + nameof(RoleCreateError));
        public static EventId RoleUpdateError { get; } = new((int)LogEventId.RoleUpdateError, nameof(Api) + "." + nameof(RoleUpdateError));
        public static EventId RoleDeleteError { get; } = new((int)LogEventId.RoleDeleteError, nameof(Api) + "." + nameof(RoleDeleteError));
        public static EventId PasswordResetError { get; } = new((int)LogEventId.PasswordResetError, nameof(Api) + "." + nameof(PasswordResetError));
        public static EventId PasswordResetTrace { get; } = new((int)LogEventId.PasswordResetTrace, nameof(Api) + "." + nameof(PasswordResetTrace));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class ServiceBus
    {
        /// <summary>Process loop started.</summary>
        public static EventId ServiceBusTraceMessage { get; } = new((int)LogEventId.ServiceBusTraceMessage, nameof(ServiceBus) + "." + nameof(ServiceBusTraceMessage));
        public static EventId ServiceBusError { get; } = new((int)LogEventId.ServiceBusError, nameof(ServiceBus) + "." + nameof(ServiceBusError));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class Messaging
    {
        /// <summary>Process loop started.</summary>
        public static EventId MessagingTraceMessage { get; } = new((int)LogEventId.MessagingTraceMessage, nameof(Messaging) + "." + nameof(MessagingTraceMessage));
        public static EventId FlowProcessingServiceTrace { get; } = new((int)LogEventId.FlowProcessingServiceTrace, nameof(Messaging) + "." + nameof(FlowProcessingServiceTrace));
        public static EventId PendingFlowProcessingServiceTrace { get; } = new((int)LogEventId.PendingFlowProcessingServiceTrace, nameof(Messaging) + "." + nameof(PendingFlowProcessingServiceTrace));
        public static EventId FailedFlowProcessingServiceTrace { get; } = new((int)LogEventId.FailedFlowProcessingServiceTrace, nameof(Messaging) + "." + nameof(FailedFlowProcessingServiceTrace));
        public static EventId FlowProcessingServiceError { get; } = new((int)LogEventId.FlowProcessingServiceError, nameof(Messaging) + "." + nameof(FlowProcessingServiceError));
        public static EventId FlowCreationServiceError { get; } = new((int)LogEventId.FlowCreationServiceError, nameof(Messaging) + "." + nameof(FlowCreationServiceError));
        public static EventId FlowCreationServiceTrace { get; } = new((int)LogEventId.FlowCreationServiceTrace, nameof(Messaging) + "." + nameof(FlowCreationServiceTrace));
        public static EventId FlowCreationServiceWarning { get; } = new((int)LogEventId.FlowCreationServiceWarning, nameof(Messaging) + "." + nameof(FlowCreationServiceWarning));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class Scoring
    {
        /// <summary>Process loop started.</summary>
        public static EventId ScoringServiceTraceMessage { get; } = new((int)LogEventId.ScoringServiceTraceMessage, nameof(Scoring) + "." + nameof(ScoringServiceTraceMessage));
        public static EventId ScoringServiceError { get; } = new((int)LogEventId.ScoringServiceError, nameof(Scoring) + "." + nameof(ScoringServiceError));
        public static EventId ScoringApiError { get; } = new((int)LogEventId.ScoringApiError, nameof(Scoring) + "." + nameof(ScoringApiError));
        public static EventId ScoringApiWarning { get; } = new((int)LogEventId.ScoringApiWarning, nameof(Scoring) + "." + nameof(ScoringApiWarning));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class Data
    {
        /// <summary>Process loop started.</summary>
        public static EventId DataServiceTraceMessage { get; } = new((int)LogEventId.DataServiceTraceMessage, nameof(Data) + "." + nameof(DataServiceTraceMessage));
        public static EventId DataServiceError { get; } = new((int)LogEventId.DataServiceError, nameof(Data) + "." + nameof(DataServiceError));
        public static EventId DataApiError { get; } = new((int)LogEventId.DataApiError, nameof(Data) + "." + nameof(DataApiError));
        public static EventId DataApiWarning { get; } = new((int)LogEventId.DataApiWarning, nameof(Data) + "." + nameof(DataApiWarning));
    }

    /// <summary>
    /// Log events
    /// </summary>
    public static class Theme
    {
        /// <summary>Process loop started.</summary>
        public static EventId ThemeServiceTraceMessage { get; } = new((int)LogEventId.ThemeServiceTraceMessage, nameof(Theme) + "." + nameof(ThemeServiceTraceMessage));
        public static EventId ThemeServiceError { get; } = new((int)LogEventId.ThemeServiceError, nameof(Theme) + "." + nameof(ThemeServiceError));
        public static EventId ThemeApiError { get; } = new((int)LogEventId.ThemeApiError, nameof(Theme) + "." + nameof(ThemeApiError));
        public static EventId ThemeApiWarning { get; } = new((int)LogEventId.ThemeApiWarning, nameof(Theme) + "." + nameof(ThemeApiWarning));
    }
}


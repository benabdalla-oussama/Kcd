using Microsoft.Extensions.Logging;
using Stayr.Backend.Common.Observability;

namespace Kcd.Infrastructure.Services;

/// <summary>
/// Implementation of <see cref="IEmailSender"/> that logs email details instead of sending them.
/// </summary>
public class LoggingEmailSender(ILogger<LoggingEmailSender> logger) : IEmailSender
{
    private readonly ILogger<LoggingEmailSender> _logger = logger;

    /// <summary>
    /// Logs the email details asynchronously instead of sending the email.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The content of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public Task SendEmailAsync(string email, string subject, string message)
    {
        // Log the email details
        _logger.LogTrace(LogEvents.TraceMessage, "Sending email to: {Email}", email);
        _logger.LogTrace(LogEvents.TraceMessage, "Subject: {Subject}", subject);
        _logger.LogTrace(LogEvents.TraceMessage, "Message: {Message}", message);

        // Simulate asynchronous operation
        return Task.CompletedTask;
    }
}

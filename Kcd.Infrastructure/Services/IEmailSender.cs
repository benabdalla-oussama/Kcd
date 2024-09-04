namespace Kcd.Infrastructure.Services;

/// <summary>
/// Defines methods for sending emails.TODO: to be implemented
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email asynchronously.
    /// </summary>
    /// <param name="email">The recipient's email address.</param>
    /// <param name="subject">The subject of the email.</param>
    /// <param name="message">The content of the email.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task SendEmailAsync(string email, string subject, string message);
}

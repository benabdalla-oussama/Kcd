using Kcd.Identity.Models;

namespace Kcd.Identity.Services;

/// <summary>
/// Provides authentication and registration services for users.
/// </summary>
/// <remarks>
/// This interface defines methods for logging in and registering users. Implementations should handle user authentication, including verifying credentials and generating authentication tokens, as well as user registration, which may include creating user accounts and setting initial passwords.
/// </remarks>
public interface IAuthService
{
    /// <summary>
    /// Logs in a user using the provided credentials.
    /// </summary>
    /// <param name="request">The authentication request containing user credentials.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains an <see cref="AuthResponse"/> object with authentication details.</returns>
    Task<AuthResponse> LoginAsync(AuthRequest request);

    /// <summary>
    /// Registers a new user with the provided details.
    /// </summary>
    /// <param name="request">The registration request containing user information.</param>
    /// <returns>A task representing the asynchronous operation. The task result contains a <see cref="RegistrationResponse"/> object with registration details.</returns>
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}

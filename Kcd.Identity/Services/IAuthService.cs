using Kcd.Identity.Models;

namespace Kcd.Identity.Services;

public interface IAuthService
{
    Task<AuthResponse> LoginAsync(AuthRequest request);
    Task<RegistrationResponse> RegisterAsync(RegistrationRequest request);
}

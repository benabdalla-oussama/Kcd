using Kcd.UI.Models;

namespace Kcd.UI.Services.AuthService
{
    public interface IAuthService
    {
        Task<AuthResponse> Login(AuthRequest request);
        Task<UserApplicationResponse> Register(UserApplicationRequest registerDto);
        Task Logout();
        Task<bool> IsUserAuthenticated();
    }
}

using Kcd.UI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Kcd.UI.Services.AuthService;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly AuthenticationStateProvider _authStateProvider;

    public AuthService(HttpClient httpClientFactory,
                       AuthenticationStateProvider authenticationStateProvider,
                       ILocalStorageService localStorage,
                       AuthenticationStateProvider authStateProvider)
    {
        //_httpClient = httpClientFactory.CreateClient("BackendApi");
        _httpClient = httpClientFactory;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _authStateProvider = authStateProvider;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("auth/Login", request);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
        if (result != null)
        {
            await _localStorage.SetItemAsync("token", result.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(result.Token);
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", result.Token);
            return result;
        }
        return result;
    }

    public async Task<UserApplicationResponse> Register(UserApplicationRequest request)
    {
        using var content = new MultipartFormDataContent();

        content.Add(new StringContent(request.Name), nameof(request.Name));
        content.Add(new StringContent(request.Email), nameof(request.Email));
        content.Add(new StringContent(request.Country), nameof(request.Country));
        content.Add(new StringContent(request.Company ?? ""), nameof(request.Company));
        content.Add(new StringContent(request.Referral ?? ""), nameof(request.Referral));

        if (request.Avatar != null)
        {
            var buffer = new byte[request.Avatar.Size];
            await request.Avatar.OpenReadStream().ReadAsync(buffer);

            var fileContent = new ByteArrayContent(buffer);
            fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
            content.Add(fileContent, nameof(request.Avatar), request.Avatar.Name);
        }

        // Use PostAsync instead of PostAsJsonAsync to send the form-data
        var response = await _httpClient.PostAsync("applications", content);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"Failed to register application: {response.ReasonPhrase}");
        }

        var result = await response.Content.ReadFromJsonAsync<UserApplicationResponse>();
        return result;
    }

    public async Task<bool> IsUserAuthenticated()
    {
        return (await _authStateProvider.GetAuthenticationStateAsync()).User.Identity.IsAuthenticated;
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("token");
        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}

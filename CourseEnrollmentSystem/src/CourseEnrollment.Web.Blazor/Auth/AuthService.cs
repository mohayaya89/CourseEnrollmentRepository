using System.Net.Http.Json;

namespace CourseEnrollment.Web.Blazor.Auth;

/// <summary>
/// Thin HTTP client wrapper for the API's <c>/api/auth</c> endpoints.
/// Updates <see cref="JwtAuthenticationStateProvider"/> on login/register/logout.
/// The HttpOnly refresh cookie is handled automatically by the browser via
/// the <c>credentials: include</c> mode set on the named HttpClient.
/// </summary>
public class AuthService
{
    private readonly HttpClient _http;
    private readonly JwtAuthenticationStateProvider _stateProvider;

    public AuthService(HttpClient http, JwtAuthenticationStateProvider stateProvider)
    {
        _http = http;
        _stateProvider = stateProvider;
    }

    /// <summary>Registers a student and logs them in on success.</summary>
    public async Task<(bool Success, string[] Errors)> RegisterAsync(RegisterStudentRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorEnvelope>();
            return (false, error?.Errors ?? ["Registration failed."]);
        }

        var dto = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (dto is null) return (false, ["Unexpected empty response."]);

        _stateProvider.NotifyLogin(dto.AccessToken, dto.AccessTokenExpiresAt);
        return (true, []);
    }

    /// <summary>Authenticates the user and stores the access token in memory.</summary>
    public async Task<(bool Success, string[] Errors)> LoginAsync(LoginRequest request)
    {
        var response = await _http.PostAsJsonAsync("api/auth/login", request);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadFromJsonAsync<ErrorEnvelope>();
            return (false, error?.Errors ?? ["Login failed."]);
        }

        var dto = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
        if (dto is null) return (false, ["Unexpected empty response."]);

        _stateProvider.NotifyLogin(dto.AccessToken, dto.AccessTokenExpiresAt);
        return (true, []);
    }

    /// <summary>
    /// Attempts a silent token refresh using the HttpOnly refresh cookie.
    /// Returns <c>true</c> if the cookie was valid and a new access token was issued.
    /// Called once on app startup to restore sessions.
    /// </summary>
    public async Task<bool> TryRefreshAsync()
    {
        try
        {
            var response = await _http.PostAsync("api/auth/refresh", null);
            if (!response.IsSuccessStatusCode) return false;

            var dto = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
            if (dto is null) return false;

            _stateProvider.NotifyLogin(dto.AccessToken, dto.AccessTokenExpiresAt);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>Logs out the user, revokes server-side tokens, and clears the cookie.</summary>
    public async Task LogoutAsync()
    {
        await _http.PostAsync("api/auth/logout", null);
        _stateProvider.NotifyLogout();
    }

    private record ErrorEnvelope(string[] Errors);
}

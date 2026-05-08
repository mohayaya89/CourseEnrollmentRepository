using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;

namespace CourseEnrollment.Web.Blazor.Auth;

/// <summary>
/// Derives authentication state from the in-memory access token by parsing the
/// JWT payload directly (Base64 decode) — no validation library needed in WASM.
/// </summary>
public class JwtAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly AccessTokenStore _store;

    public JwtAuthenticationStateProvider(AccessTokenStore store)
    {
        _store = store;
    }

    /// <inheritdoc />
    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (!_store.HasToken || _store.Token is null)
            return Task.FromResult(Anonymous);

        try
        {
            var claims = ParseClaimsFromJwt(_store.Token);
            var identity = new ClaimsIdentity(claims, "jwt");
            return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
        }
        catch
        {
            return Task.FromResult(Anonymous);
        }
    }

    /// <summary>Call this after a successful login or register to notify subscribers.</summary>
    public void NotifyLogin(string rawToken, DateTime expiresAt)
    {
        _store.Set(rawToken, expiresAt);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    /// <summary>Call this after logout to clear state and notify subscribers.</summary>
    public void NotifyLogout()
    {
        _store.Clear();
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];

        // Pad base64url to standard base64
        switch (payload.Length % 4)
        {
            case 2: payload += "=="; break;
            case 3: payload += "="; break;
        }
        payload = payload.Replace('-', '+').Replace('_', '/');

        var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
        using var doc = JsonDocument.Parse(json);

        var claims = new List<Claim>();
        foreach (var element in doc.RootElement.EnumerateObject())
        {
            if (element.Value.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in element.Value.EnumerateArray())
                    claims.Add(new Claim(element.Name, item.GetString() ?? string.Empty));
            }
            else
            {
                claims.Add(new Claim(element.Name, element.Value.ToString()));
            }
        }
        return claims;
    }
}

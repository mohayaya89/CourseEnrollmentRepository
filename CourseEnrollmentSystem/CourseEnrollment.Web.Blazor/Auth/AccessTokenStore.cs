namespace CourseEnrollment.Web.Blazor.Auth;

/// <summary>
/// Singleton in-memory store for the current access token.
/// The token lives only as long as the page/tab is open — never written to localStorage or sessionStorage.
/// </summary>
public class AccessTokenStore
{
    private string? _token;
    private DateTime _expiresAt;

    /// <summary>Whether a non-expired token is currently held.</summary>
    public bool HasToken => _token is not null && DateTime.UtcNow < _expiresAt;

    /// <summary>Returns the raw token string, or <c>null</c> if none is stored.</summary>
    public string? Token => _token;

    /// <summary>Stores a new access token along with its expiry.</summary>
    public void Set(string token, DateTime expiresAt)
    {
        _token = token;
        _expiresAt = expiresAt;
    }

    /// <summary>Clears the stored token (used on logout).</summary>
    public void Clear()
    {
        _token = null;
        _expiresAt = default;
    }
}

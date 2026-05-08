namespace CourseEnrollment.Application.Common.Auth.Dtos
{
    /// <summary>
    /// Carries both the public <see cref="AuthResponseDto"/> (returned in the response body)
    /// and the raw refresh token (set by the API layer as an HttpOnly cookie).
    /// The raw token is never serialised to JSON.
    /// </summary>
    public record AuthTokenPair(AuthResponseDto Response, string RawRefreshToken);
}

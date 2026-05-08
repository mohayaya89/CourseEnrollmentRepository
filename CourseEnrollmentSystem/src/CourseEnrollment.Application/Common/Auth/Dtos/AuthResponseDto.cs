namespace CourseEnrollment.Application.Common.Auth.Dtos
{
    /// <summary>
    /// Returned to the client after a successful login or register.
    /// The refresh token is NOT included here — it travels via an HttpOnly cookie.
    /// </summary>
    public record AuthResponseDto(
        string AccessToken,
        DateTime AccessTokenExpiresAt,
        Guid UserId,
        string Email,
        string[] Roles,
        Guid? StudentId);
}

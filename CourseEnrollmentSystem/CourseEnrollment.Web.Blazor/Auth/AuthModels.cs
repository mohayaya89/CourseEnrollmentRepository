namespace CourseEnrollment.Web.Blazor.Auth;

/// <summary>Matches <c>AuthResponseDto</c> returned by the API (refresh token NOT included).</summary>
public record AuthResponseDto(
    string AccessToken,
    DateTime AccessTokenExpiresAt,
    Guid UserId,
    string Email,
    string[] Roles,
    Guid? StudentId);

public record LoginRequest(string Email, string Password);

public record RegisterStudentRequest(
    string FirstName,
    string LastName,
    string Email,
    string Password);

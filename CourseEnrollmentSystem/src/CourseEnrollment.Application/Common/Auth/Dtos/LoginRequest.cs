namespace CourseEnrollment.Application.Common.Auth.Dtos
{
    /// <summary>Payload sent by the client to authenticate.</summary>
    public record LoginRequest(string Email, string Password);
}

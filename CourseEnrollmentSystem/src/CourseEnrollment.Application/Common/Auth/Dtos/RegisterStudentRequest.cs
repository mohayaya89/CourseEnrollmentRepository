namespace CourseEnrollment.Application.Common.Auth.Dtos
{
    /// <summary>Payload sent by the client to create a student account.</summary>
    public record RegisterStudentRequest(
        string FirstName,
        string LastName,
        string Email,
        string Password);
}

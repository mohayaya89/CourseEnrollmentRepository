namespace CourseEnrollment.Application.Common.Auth.Dtos
{
    /// <summary>
    /// A projection of an Identity user that is safe to pass between Application
    /// and Infrastructure without leaking Identity types into Application.
    /// </summary>
    public record ApplicationUserDto(
        Guid Id,
        string Email,
        Guid? StudentId);
}

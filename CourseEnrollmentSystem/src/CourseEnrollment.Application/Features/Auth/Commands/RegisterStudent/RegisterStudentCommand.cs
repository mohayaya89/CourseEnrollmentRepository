using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.RegisterStudent
{
    /// <summary>Creates a student Identity account, assigns the Student role, and links the domain Student entity.</summary>
    public record RegisterStudentCommand(
        string FirstName,
        string LastName,
        string Email,
        string Password) : IRequest<Result<AuthResponseDto>>;
}

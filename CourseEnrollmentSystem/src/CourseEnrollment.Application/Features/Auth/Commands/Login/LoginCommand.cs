using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.Login
{
    /// <summary>Authenticates a user and returns a token pair on success.</summary>
    public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
}

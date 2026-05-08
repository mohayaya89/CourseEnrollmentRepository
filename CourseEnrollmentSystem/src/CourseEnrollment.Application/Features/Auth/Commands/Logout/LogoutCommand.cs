using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.Logout
{
    /// <summary>Revokes all refresh tokens for the authenticated user, effectively signing them out.</summary>
    public record LogoutCommand(Guid UserId) : IRequest;
}

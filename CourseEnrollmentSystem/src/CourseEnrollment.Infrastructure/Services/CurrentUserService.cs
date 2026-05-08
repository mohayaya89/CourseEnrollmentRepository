using System.Security.Claims;
using CourseEnrollment.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CourseEnrollment.Infrastructure.Services
{
    /// <summary>Reads the current user's identity from the HTTP context claims principal.</summary>
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <inheritdoc />
        public Guid UserId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                return string.IsNullOrEmpty(value)
                    ? throw new Exception("User not authenticated")
                    : Guid.Parse(value);
            }
        }

        /// <inheritdoc />
        public string? Email =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value;

        /// <inheritdoc />
        public IReadOnlyList<string> Roles =>
            _httpContextAccessor.HttpContext?.User?.Claims
                .Where(c => c.Type == ClaimTypes.Role)
                .Select(c => c.Value)
                .ToList()
                .AsReadOnly()
            ?? (IReadOnlyList<string>)[];

        /// <inheritdoc />
        public Guid? StudentId
        {
            get
            {
                var value = _httpContextAccessor.HttpContext?
                    .User?.FindFirst("student_id")?.Value;

                return Guid.TryParse(value, out var id) ? id : null;
            }
        }
    }
}

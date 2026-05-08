using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;

namespace CourseEnrollment.Application.Common.Interfaces
{
    /// <summary>
    /// Abstracts Identity operations so Application stays free of Identity framework types.
    /// All methods return <see cref="Result"/> or <see cref="Result{T}"/> — never throw for
    /// predictable failures such as duplicate email or wrong password.
    /// </summary>
    public interface IIdentityService
    {
        /// <summary>Creates a new user account and assigns the Student role.</summary>
        Task<Result<Guid>> RegisterStudentAsync(string firstName, string lastName, string email, string password);

        /// <summary>Validates the supplied credentials. Returns the user id on success.</summary>
        Task<Result<Guid>> ValidatePasswordAsync(string email, string password);

        /// <summary>Retrieves a user projection by id. Returns failure if not found.</summary>
        Task<Result<ApplicationUserDto>> GetUserByIdAsync(Guid userId);

        /// <summary>Retrieves a user projection by email. Returns failure if not found.</summary>
        Task<Result<ApplicationUserDto>> GetUserByEmailAsync(string email);

        /// <summary>Returns the role names assigned to the given user.</summary>
        Task<IReadOnlyList<string>> GetRolesAsync(Guid userId);

        /// <summary>Links the given Identity user to a <c>Student</c> entity.</summary>
        Task<Result> SetStudentIdClaimAsync(Guid userId, Guid studentId);
    }
}

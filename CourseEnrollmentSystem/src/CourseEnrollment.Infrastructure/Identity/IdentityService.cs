using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace CourseEnrollment.Infrastructure.Identity
{
    /// <summary>
    /// Implements <see cref="IIdentityService"/> using ASP.NET Core Identity.
    /// Wraps <see cref="UserManager{TUser}"/> and <see cref="RoleManager{TRole}"/> so the
    /// Application layer never references Identity types directly.
    /// </summary>
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;

        public IdentityService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        /// <inheritdoc />
        public async Task<Result<Guid>> RegisterStudentAsync(
            string firstName, string lastName, string email, string password)
        {
            var existing = await _userManager.FindByEmailAsync(email);
            if (existing is not null)
                return Result<Guid>.Failure("A user with that email already exists.");

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
            };

            var createResult = await _userManager.CreateAsync(user, password);
            if (!createResult.Succeeded)
                return Result<Guid>.Failure([.. createResult.Errors.Select(e => e.Description)]);

            if (!await _roleManager.RoleExistsAsync(Roles.Student))
                await _roleManager.CreateAsync(new IdentityRole<Guid>(Roles.Student));

            var roleResult = await _userManager.AddToRoleAsync(user, Roles.Student);
            if (!roleResult.Succeeded)
                return Result<Guid>.Failure([.. roleResult.Errors.Select(e => e.Description)]);

            return Result<Guid>.Success(user.Id);
        }

        /// <inheritdoc />
        public async Task<Result<Guid>> ValidatePasswordAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<Guid>.Failure("Invalid credentials.");

            var locked = await _userManager.IsLockedOutAsync(user);
            if (locked)
                return Result<Guid>.Failure("Account is temporarily locked.");

            var valid = await _userManager.CheckPasswordAsync(user, password);
            if (!valid)
            {
                await _userManager.AccessFailedAsync(user);
                return Result<Guid>.Failure("Invalid credentials.");
            }

            await _userManager.ResetAccessFailedCountAsync(user);
            return Result<Guid>.Success(user.Id);
        }

        /// <inheritdoc />
        public async Task<Result<ApplicationUserDto>> GetUserByIdAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result<ApplicationUserDto>.Failure("User not found.");

            return Result<ApplicationUserDto>.Success(
                new ApplicationUserDto(user.Id, user.Email!, user.StudentId));
        }

        /// <inheritdoc />
        public async Task<Result<ApplicationUserDto>> GetUserByEmailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<ApplicationUserDto>.Failure("User not found.");

            return Result<ApplicationUserDto>.Success(
                new ApplicationUserDto(user.Id, user.Email!, user.StudentId));
        }

        /// <inheritdoc />
        public async Task<IReadOnlyList<string>> GetRolesAsync(Guid userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return [];

            var roles = await _userManager.GetRolesAsync(user);
            return roles.AsReadOnly();
        }

        /// <inheritdoc />
        public async Task<Result> SetStudentIdClaimAsync(Guid userId, Guid studentId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Result.Failure("User not found.");

            user.StudentId = studentId;
            var updateResult = await _userManager.UpdateAsync(user);

            return updateResult.Succeeded
                ? Result.Success()
                : Result.Failure([.. updateResult.Errors.Select(e => e.Description)]);
        }
    }
}

using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Domain.Entities;
using CourseEnrollment.Domain.Interfaces;
using CourseEnrollment.Domain.ValueObjects;
using MediatR;

namespace CourseEnrollment.Application.Features.Auth.Commands.RegisterStudent
{
    /// <summary>
    /// Registers a new student: creates the Identity user, creates the domain Student,
    /// links them, then issues an access + refresh token pair.
    /// All identity + student creation is transactional — if either step fails everything rolls back.
    /// </summary>
    public class RegisterStudentCommandHandler
        : IRequestHandler<RegisterStudentCommand, Result<AuthTokenPair>>
    {
        private readonly IIdentityService _identity;
        private readonly IJwtTokenService _jwt;
        private readonly IRefreshTokenStore _tokenStore;
        private readonly IStudentRepository _students;
        private readonly IUnitOfWork _unitOfWork;

        public RegisterStudentCommandHandler(
            IIdentityService identity,
            IJwtTokenService jwt,
            IRefreshTokenStore tokenStore,
            IStudentRepository students,
            IUnitOfWork unitOfWork)
        {
            _identity = identity;
            _jwt = jwt;
            _tokenStore = tokenStore;
            _students = students;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<AuthTokenPair>> Handle(
            RegisterStudentCommand request, CancellationToken ct)
        {
            var registerResult = await _identity.RegisterStudentAsync(
                request.FirstName, request.LastName, request.Email, request.Password);

            if (!registerResult.Succeeded)
                return Result<AuthTokenPair>.Failure([.. registerResult.Errors]);

            var userId = registerResult.Value;

            var student = new Student(request.FirstName, request.LastName, Email.Create(request.Email));
            student.LinkToUser(userId);
            await _students.AddAsync(student, ct);

            var linkResult = await _identity.SetStudentIdClaimAsync(userId, student.Id);
            if (!linkResult.Succeeded)
                return Result<AuthTokenPair>.Failure([.. linkResult.Errors]);

            await _unitOfWork.SaveChangesAsync(ct);

            var userDto = new ApplicationUserDto(userId, request.Email, student.Id);
            var roles = await _identity.GetRolesAsync(userId);
            var (accessToken, expiresAt) = _jwt.GenerateAccessToken(userDto, roles);
            var rawRefresh = _jwt.GenerateRefreshToken();
            await _tokenStore.StoreAsync(userId, rawRefresh, DateTime.UtcNow.AddDays(7), ct);

            var dto = new AuthResponseDto(accessToken, expiresAt, userId, request.Email, [.. roles], student.Id);
            return Result<AuthTokenPair>.Success(new AuthTokenPair(dto, rawRefresh));
        }
    }
}

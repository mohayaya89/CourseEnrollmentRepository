using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Auth.Commands.RegisterStudent;
using CourseEnrollment.Domain.Interfaces;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Auth
{
    [TestFixture]
    public class RegisterStudentCommandHandlerTests
    {
        private IIdentityService _identity = null!;
        private IJwtTokenService _jwt = null!;
        private IRefreshTokenStore _tokenStore = null!;
        private IStudentRepository _students = null!;
        private IUnitOfWork _unitOfWork = null!;
        private RegisterStudentCommandHandler _handler = null!;

        private static readonly Guid UserId = Guid.NewGuid();
        private static readonly Guid StudentId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _identity = Substitute.For<IIdentityService>();
            _jwt = Substitute.For<IJwtTokenService>();
            _tokenStore = Substitute.For<IRefreshTokenStore>();
            _students = Substitute.For<IStudentRepository>();
            _unitOfWork = Substitute.For<IUnitOfWork>();

            _handler = new RegisterStudentCommandHandler(
                _identity, _jwt, _tokenStore, _students, _unitOfWork);
        }

        [Test]
        public async Task Handle_ValidCommand_ReturnsSuccessWithTokens()
        {
            var command = new RegisterStudentCommand("Jane", "Doe", "jane@example.com", "Password1");

            _identity.RegisterStudentAsync("Jane", "Doe", "jane@example.com", "Password1")
                .Returns(Result<Guid>.Success(UserId));

            _identity.SetStudentIdClaimAsync(UserId, Arg.Any<Guid>())
                .Returns(Result.Success());

            _identity.GetRolesAsync(UserId)
                .Returns(new List<string> { "Student" });

            _identity.GetUserByIdAsync(UserId)
                .Returns(Result<ApplicationUserDto>.Success(
                    new ApplicationUserDto(UserId, "jane@example.com", StudentId)));

            _jwt.GenerateAccessToken(Arg.Any<ApplicationUserDto>(), Arg.Any<IEnumerable<string>>())
                .Returns(("access-token", DateTime.UtcNow.AddMinutes(15)));

            _jwt.GenerateRefreshToken().Returns("raw-refresh-token");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value!.Response.AccessToken, Is.EqualTo("access-token"));
            Assert.That(result.Value.Response.Email, Is.EqualTo("jane@example.com"));
            Assert.That(result.Value.Response.Roles, Contains.Item("Student"));
            Assert.That(result.Value.RawRefreshToken, Is.EqualTo("raw-refresh-token"));

            await _students.Received(1).AddAsync(Arg.Any<Domain.Entities.Student>(), Arg.Any<CancellationToken>());
            await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
            await _tokenStore.Received(1).StoreAsync(UserId, "raw-refresh-token", Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_IdentityRegistrationFails_ReturnsFailure()
        {
            var command = new RegisterStudentCommand("Jane", "Doe", "jane@example.com", "Password1");

            _identity.RegisterStudentAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<string>())
                .Returns(Result<Guid>.Failure("Email already in use."));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Has.Member("Email already in use."));

            await _students.DidNotReceive().AddAsync(Arg.Any<Domain.Entities.Student>(), Arg.Any<CancellationToken>());
            await _unitOfWork.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
        }
    }
}

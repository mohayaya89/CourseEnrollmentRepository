using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Auth.Commands.Login;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Auth
{
    [TestFixture]
    public class LoginCommandHandlerTests
    {
        private IIdentityService _identity = null!;
        private IJwtTokenService _jwt = null!;
        private IRefreshTokenStore _tokenStore = null!;
        private LoginCommandHandler _handler = null!;

        private static readonly Guid UserId = Guid.NewGuid();
        private static readonly Guid StudentId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _identity = Substitute.For<IIdentityService>();
            _jwt = Substitute.For<IJwtTokenService>();
            _tokenStore = Substitute.For<IRefreshTokenStore>();

            _handler = new LoginCommandHandler(_identity, _jwt, _tokenStore);
        }

        [Test]
        public async Task Handle_ValidCredentials_ReturnsSuccessWithTokens()
        {
            var command = new LoginCommand("jane@example.com", "Password1");

            _identity.ValidatePasswordAsync("jane@example.com", "Password1")
                .Returns(Result<Guid>.Success(UserId));

            _identity.GetUserByIdAsync(UserId)
                .Returns(Result<ApplicationUserDto>.Success(
                    new ApplicationUserDto(UserId, "jane@example.com", StudentId)));

            _identity.GetRolesAsync(UserId)
                .Returns(new List<string> { "Student" });

            _jwt.GenerateAccessToken(Arg.Any<ApplicationUserDto>(), Arg.Any<IEnumerable<string>>())
                .Returns(("access-token", DateTime.UtcNow.AddMinutes(15)));

            _jwt.GenerateRefreshToken().Returns("raw-refresh-token");

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Value!.Response.AccessToken, Is.EqualTo("access-token"));
            Assert.That(result.Value.Response.Roles, Contains.Item("Student"));
            Assert.That(result.Value.Response.StudentId, Is.EqualTo(StudentId));
            Assert.That(result.Value.RawRefreshToken, Is.EqualTo("raw-refresh-token"));

            await _tokenStore.Received(1).StoreAsync(UserId, "raw-refresh-token", Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_InvalidCredentials_ReturnsFailureWithGenericMessage()
        {
            var command = new LoginCommand("jane@example.com", "WrongPass");

            _identity.ValidatePasswordAsync(Arg.Any<string>(), Arg.Any<string>())
                .Returns(Result<Guid>.Failure("Wrong password."));

            var result = await _handler.Handle(command, CancellationToken.None);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Has.Member("Invalid credentials."));

            await _tokenStore.DidNotReceive().StoreAsync(Arg.Any<Guid>(), Arg.Any<string>(), Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        }
    }
}

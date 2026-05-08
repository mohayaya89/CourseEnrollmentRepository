using CourseEnrollment.Application.Common.Auth;
using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Auth.Commands.RefreshToken;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Auth
{
    [TestFixture]
    public class RefreshTokenCommandHandlerTests
    {
        private IRefreshTokenStore _tokenStore = null!;
        private IIdentityService _identity = null!;
        private IJwtTokenService _jwt = null!;
        private RefreshTokenCommandHandler _handler = null!;

        private static readonly Guid UserId = Guid.NewGuid();
        private static readonly Guid TokenId = Guid.NewGuid();
        private static readonly Guid StudentId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _tokenStore = Substitute.For<IRefreshTokenStore>();
            _identity = Substitute.For<IIdentityService>();
            _jwt = Substitute.For<IJwtTokenService>();

            _handler = new RefreshTokenCommandHandler(_tokenStore, _identity, _jwt);
        }

        [Test]
        public async Task Handle_ValidToken_RotatesTokenAndReturnsNewPair()
        {
            var rawToken = "valid-refresh-token";

            _tokenStore.FindAsync(rawToken, Arg.Any<CancellationToken>())
                .Returns((TokenId, UserId));

            _identity.GetUserByIdAsync(UserId)
                .Returns(Result<ApplicationUserDto>.Success(
                    new ApplicationUserDto(UserId, "jane@example.com", StudentId)));

            _identity.GetRolesAsync(UserId)
                .Returns(new List<string> { "Student" });

            _jwt.GenerateAccessToken(Arg.Any<ApplicationUserDto>(), Arg.Any<IEnumerable<string>>())
                .Returns(("new-access-token", DateTime.UtcNow.AddMinutes(15)));

            _jwt.GenerateRefreshToken().Returns("new-refresh-token");

            var result = await _handler.Handle(new RefreshTokenCommand(rawToken), CancellationToken.None);

            Assert.That(result.Succeeded, Is.True);
            Assert.That(result.Value!.Response.AccessToken, Is.EqualTo("new-access-token"));
            Assert.That(result.Value.RawRefreshToken, Is.EqualTo("new-refresh-token"));

            await _tokenStore.Received(1).RevokeAsync(TokenId, ct: Arg.Any<CancellationToken>());
            await _tokenStore.Received(1).StoreAsync(UserId, "new-refresh-token", Arg.Any<DateTime>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Handle_InvalidToken_ReturnsFailure()
        {
            _tokenStore.FindAsync(Arg.Any<string>(), Arg.Any<CancellationToken>())
                .Returns(((Guid TokenId, Guid UserId)?)null);

            var result = await _handler.Handle(new RefreshTokenCommand("bad-token"), CancellationToken.None);

            Assert.That(result.Succeeded, Is.False);
            Assert.That(result.Errors, Has.Member("Refresh token is invalid or expired."));

            await _tokenStore.DidNotReceive().RevokeAsync(Arg.Any<Guid>(), Arg.Any<string?>(), Arg.Any<CancellationToken>());
        }
    }
}

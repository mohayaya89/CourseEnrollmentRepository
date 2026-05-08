using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Auth.Commands.Logout;
using NSubstitute;

namespace CourseEnrollment.Application.Tests.Auth
{
    [TestFixture]
    public class LogoutCommandHandlerTests
    {
        private IRefreshTokenStore _tokenStore = null!;
        private LogoutCommandHandler _handler = null!;

        private static readonly Guid UserId = Guid.NewGuid();

        [SetUp]
        public void SetUp()
        {
            _tokenStore = Substitute.For<IRefreshTokenStore>();
            _handler = new LogoutCommandHandler(_tokenStore);
        }

        [Test]
        public async Task Handle_ValidUser_RevokesAllTokens()
        {
            await _handler.Handle(new LogoutCommand(UserId), CancellationToken.None);

            await _tokenStore.Received(1).RevokeAllForUserAsync(UserId, Arg.Any<CancellationToken>());
        }
    }
}

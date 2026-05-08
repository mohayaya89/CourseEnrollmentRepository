using CourseEnrollment.Application.Common.Auth.Dtos;
using CourseEnrollment.Application.Common.Interfaces;
using CourseEnrollment.Application.Features.Auth.Commands.Login;
using CourseEnrollment.Application.Features.Auth.Commands.Logout;
using CourseEnrollment.Application.Features.Auth.Commands.RefreshToken;
using CourseEnrollment.Application.Features.Auth.Commands.RegisterStudent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseEnrollment.Api.Controllers
{
    /// <summary>Handles user registration, login, token refresh, logout, and identity queries.</summary>
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private const string RefreshTokenCookie = "refreshToken";

        private readonly ISender _mediator;
        private readonly ICurrentUserService _currentUser;

        public AuthController(ISender mediator, ICurrentUserService currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        /// <summary>Registers a new student account and returns an access token.</summary>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterStudentRequest request, CancellationToken ct)
        {
            var command = new RegisterStudentCommand(
                request.FirstName, request.LastName, request.Email, request.Password);

            var result = await _mediator.Send(command, ct);
            if (!result.Succeeded)
                return BadRequest(new { errors = result.Errors });

            SetRefreshCookie(result.Value!.RawRefreshToken);
            return StatusCode(StatusCodes.Status201Created, result.Value.Response);
        }

        /// <summary>Authenticates a user and returns an access token.</summary>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
        {
            var result = await _mediator.Send(new LoginCommand(request.Email, request.Password), ct);
            if (!result.Succeeded)
                return Unauthorized(new { errors = result.Errors });

            SetRefreshCookie(result.Value!.RawRefreshToken);
            return Ok(result.Value.Response);
        }

        /// <summary>Uses the HttpOnly refresh cookie to issue a new access + refresh token pair.</summary>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AuthResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh(CancellationToken ct)
        {
            var rawToken = Request.Cookies[RefreshTokenCookie];
            if (string.IsNullOrEmpty(rawToken))
                return Unauthorized(new { errors = new[] { "Refresh token cookie is missing." } });

            var result = await _mediator.Send(new RefreshTokenCommand(rawToken), ct);
            if (!result.Succeeded)
                return Unauthorized(new { errors = result.Errors });

            SetRefreshCookie(result.Value!.RawRefreshToken);
            return Ok(result.Value.Response);
        }

        /// <summary>Revokes all refresh tokens for the current user and clears the cookie.</summary>
        [Authorize]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            await _mediator.Send(new LogoutCommand(_currentUser.UserId), ct);
            ClearRefreshCookie();
            return NoContent();
        }

        /// <summary>Returns the current user's identity information.</summary>
        [Authorize]
        [HttpGet("me")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Me()
        {
            return Ok(new
            {
                UserId = _currentUser.UserId,
                Email = _currentUser.Email,
                Roles = _currentUser.Roles,
                StudentId = _currentUser.StudentId,
            });
        }

        private void SetRefreshCookie(string rawToken)
        {
            Response.Cookies.Append(RefreshTokenCookie, rawToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(7),
            });
        }

        private void ClearRefreshCookie()
        {
            Response.Cookies.Delete(RefreshTokenCookie, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
            });
        }
    }
}

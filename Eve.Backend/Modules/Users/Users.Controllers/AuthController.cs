using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Users.Application.Dtos.Requests;
using Users.Application.Interfaces;

namespace Controllers
{
    /// <summary>
    /// Provides API endpoint for user login, logout, session refresh and password recovery
    /// </summary>
    [Route("api/")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        /// <summary>
        /// Attempts to login a user.
        /// </summary>
        /// <param name="request">request payload of type <see cref="LoginRequestDto"/></param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("auth/login")]
        [EnableRateLimiting("strict")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status423Locked)]
        public async Task<ActionResult<bool>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
        {
            var loginResponse = await _authService.LoginAsync(request, ct);
            return Ok(loginResponse);
        }

        /// <summary>
        /// Attempts to logout a user - deletes a given session.
        /// </summary>
        /// <param name="deviceUuid">a session id to be deleted (logout)</param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("auth/logout")]
        [EnableRateLimiting("free")]
        public async Task<ActionResult<bool>> Logout([FromBody] Guid deviceUuid, CancellationToken ct)
        {
            var logoutResponse = await _authService.LogoutAsync(deviceUuid, ct);
            return Ok(logoutResponse);
        }

        /// <summary>
        /// Attempts to refresh a session.
        /// </summary>
        /// <param name="request">request payload of type <see cref="RefreshRequestDto"/></param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("auth/refresh")]
        [EnableRateLimiting("free")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<bool>> RefreshSession([FromBody] RefreshRequestDto request, CancellationToken ct)
        {
            var logoutResponse = await _authService.RefreshSessionAsync(request, ct);
            return Ok(logoutResponse);
        }

        /// <summary>
        /// Attempts to recover a password for a user.
        /// </summary>
        /// <remarks>
        /// Method generates a new temporary password for a user and it is marked as temporary through user settings - <see cref="UserSettings.MustChangePassword"/>
        /// </remarks>
        /// <param name="email">user's email</param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("auth/recover-password")]
        [EnableRateLimiting("free")]
        public async Task<ActionResult<bool>> RecoverPassword([FromBody] string email, CancellationToken ct)
        {
            var logoutResponse = await _authService.RecoverPassword(email, ct);
            return Ok(logoutResponse);
        }
    }
}

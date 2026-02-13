using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Users.Application.Dtos.Requests;
using Users.Application.Interfaces;

namespace Users.Controllers
{
    /// <summary>
    /// Provides API endpoints for user registration, account activation, and resending activation emails.
    /// </summary>
    /// <remarks>This controller exposes endpoints for registering new users, activating user accounts via
    /// token, and resending activation emails. All endpoints are rate-limited and allow anonymous access. Responses are
    /// returned in JSON format, and a 429 status code is returned if rate limits are exceeded.</remarks>
    [Route("api/")]
    [ApiController]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Produces("application/json")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRegistrationService _registrationService;

        public RegistrationController(IUserRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        /// <summary>
        /// Create a registration and return bool.
        /// </summary>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <param name="request">request payload of type <see cref="UserRegistrationRequestDto"/></param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("user-registration")]
        [EnableRateLimiting("strict")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] UserRegistrationRequestDto request, CancellationToken ct)
        {
            var registered = await _registrationService.RegisterUserAsync(request, ct);
            return Ok(registered);
        }

        /// <summary>
        /// Activate user's account and return bool.
        /// </summary>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <param name="token">Token added to user when he first registered his account - <see cref="UserSettings.ActivationToken"/></param>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("user-activation")]
        [EnableRateLimiting("strict")]
        public async Task<ActionResult<bool>> ActivateUser([FromQuery] string token, CancellationToken ct)
        {
            var activated = await _registrationService.ActivateUserAsync(token, ct);
            return Ok(activated);
        }

        /// <summary>
        /// Resend activation email to user and return bool.
        /// </summary>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <param name="email">User's email</param>
        /// <remarks>Can be called only once per minute per ip</remarks>
        /// <returns>
        /// An <see cref="ActionResult"/>.
        /// </returns>
        [HttpPost("user-resend-email")]
        [EnableRateLimiting("strict")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<bool>> ResendActivationEmail([FromBody] string email, CancellationToken ct)
        {
            var emailResent = await _registrationService.ResendActivationEmailAsync(email, ct);
            return Ok(emailResent);
        }
    }
}

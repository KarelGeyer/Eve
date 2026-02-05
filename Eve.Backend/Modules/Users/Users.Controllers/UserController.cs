using System.Security.Claims;
using Common.Shared.Exceptions;
using Common.Shared.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Users.Application.Dtos.Requests;
using Users.Application.Dtos.ResponseDtos;
using Users.Application.Interfaces;

namespace Controllers
{
    [Route("api/")]
    [ApiController]
    [Produces("application/json")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // Todo: dodělej autorizační middleware pro běžného usera.

        /// <summary>
        /// Retrieves a <see cref="UserBasicResponseDto"/>.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token used to cancel the request.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a <see cref="UserBasicResponseDto"/>.
        /// </returns>
        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserBasicResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserBasicResponseDto>> GetUser(CancellationToken ct)
        {
            var user = await _userService.GetUserAsync(User.GetUserId(), ct);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a <see cref="UserFullResponseDto"/>.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token used to cancel the request.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a <see cref="UserFullResponseDto"/>.
        /// </returns>
        [HttpGet("user/full")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserFullResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserFullResponseDto>> GetFullUser(CancellationToken ct)
        {
            var user = await _userService.GetUserFullAsync(User.GetUserId(), ct);
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a <see cref="UserSettingsResponseDto"/>.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token used to cancel the request.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a <see cref="UserSettingsResponseDto"/>.
        /// </returns>
        [HttpGet("user/settings")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserSettingsResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserSettingsResponseDto>> GetUserSettings(CancellationToken ct)
        {
            var settings = await _userService.GetUserSettings(User.GetUserId(), ct);
            return Ok(settings);
        }

        /// <summary>
        /// Retrieves a <see cref="UserIdentityResponseDto"/>.
        /// </summary>
        /// <param name="ct">
        /// Cancellation token used to cancel the request.
        /// </param>
        /// <returns>
        /// An <see cref="ActionResult"/> containing a <see cref="UserIdentityResponseDto"/>.
        /// </returns>
        [HttpGet("user/identity")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserIdentityResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<UserIdentityResponseDto>> GetUserIdentity(CancellationToken ct)
        {
            var identity = await _userService.GetUserIdentity(User.GetUserId(), ct);
            return Ok(identity);
        }

        /// <summary>
        /// Update User email and phone number and returns <see cref="UserUpdateResponseDto"/>.
        /// </summary>
        /// <param name="request">The user profile update data.</param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>An ActionResult containing a UserUpdateResponseDto.</returns>
        [HttpPatch("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserUpdateResponseDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserUpdateResponseDto>> UpdateUserInfo([FromBody] UpdateProfileRequestDto request, CancellationToken ct)
        {
            var response = await _userService.UpdateUserAsync(User.GetUserId(), request, ct);
            return Ok(response);
        }

        /// <summary>
        /// Update User password and returns bool expression success and failure.
        /// </summary>
        /// <param name="request">The user profile update data.</param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>An ActionResult containing a UserUpdateResponseDto.</returns>
        [HttpPut("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangePasswordRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> UpdateUserPassword([FromBody] ChangePasswordRequestDto request, CancellationToken ct)
        {
            var response = await _userService.UpdateUserPasswordAsync(User.GetUserId(), request, ct);
            return Ok(response);
        }

        /// <summary>
        /// Marks user as deleted and returns bool expression success and failure. Job is planned to be done by background service
        /// that will delete user from database after 30 days. Until then, user will be marked as not-active and won't be able to log in.
        /// </summary>
        /// <param name="request">The user profile update data.</param>
        /// <param name="ct">Cancellation token used to cancel the request.</param>
        /// <returns>An ActionResult containing a UserUpdateResponseDto.</returns>
        [HttpDelete("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ChangePasswordRequestDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<bool>> DeleteUser([FromBody] string password, CancellationToken ct)
        {
            // todo - check if user is admin before deletion
            var response = await _userService.DeleteUserAsync(User.GetUserId(), password, ct);
            return Ok(response);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Users.Application.Dtos.Requests;
using Users.Application.Interfaces;

namespace Controllers
{
    [Route("api/")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("auth/login")]
        [AllowAnonymous]
        [EnableRateLimiting("strict")]
        public async Task<ActionResult<bool>> Login([FromBody] LoginRequestDto request, CancellationToken ct)
        {
            var loginResponse = await _authService.LoginAsync(request, ct);
            return Ok(loginResponse);
        }
    }
}

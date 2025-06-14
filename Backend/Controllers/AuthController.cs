using Backend.Authentication;
using Backend.DTOs;
using Backend.ErrorHandling;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController(IAuthService _authService) : ControllerBase
    {
        [HttpPost("register")]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status201Created)]
        public async Task<IActionResult> RegisterUser([FromBody] UserRegisterRequestDTO request)
        {
            var result = await _authService.RegisterAsync(request);
            if (result.Error == AuthenticationErrors.UserAlreadyExist)
            {
                return BadRequest(result.Error.Message);
            }
            if (result.Error == AuthenticationErrors.Identity)
            {
                return StatusCode(500, result.Error.Message);
            }

            return Created();
        }
        
        [HttpPost("login")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UserLoginResponseDTO), StatusCodes.Status200OK)]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginRequestDTO request)
        {
            var result = await _authService.LoginAsync(request);
            if (result.Error == AuthenticationErrors.UserNotFound)
            {
                return NotFound(result.Error.Message);
            }
            else if (result.Error == AuthenticationErrors.InvalidLogin)
            {
                return BadRequest(result.Error.Message);
            }

            return Ok(result.Value);
        }
        
        [HttpPost("logout")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [Authorize(AuthenticationSchemes = "refreshTokenCookie", Policy = "RefreshTokenPolicy")]
        public async Task<IActionResult> LogoutUser()
        {
            var result = await _authService.LogoutUserAsync();
            return result.IsSuccess ? Ok() : BadRequest();
        }
        
        [HttpPost("confirmemail")] 
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmEmail([FromQuery] string userId, [FromQuery] string code)
        {
            var result = await _authService.ConfirmEmailAsync(userId, code);
            if (result.Error == AuthenticationErrors.UserNotFound)
            {
                return NotFound(result.Error.Message);
            }
            else if (result.Error == AuthenticationErrors.ConfirmationEmail)
            {
                return StatusCode(500, result.Error.Message);
            }
            return Ok();
        }
        
        [HttpPost("forgotpassword")]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
        [Authorize]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDTO request)
        {
            var result = await _authService.ForgotPasswordEmailAsync(request);
            return result.IsSuccess ? Ok() : NotFound(result.Error.Message);
        }
        
        [HttpPost("resetpassword")]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
        [ProducesResponseType(typeof(void), StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (result.Error == AuthenticationErrors.UserNotFound)
            {
                return NotFound(result.Error.Message);
            }
            else if (result.Error == AuthenticationErrors.ConfirmedEmail)
            {
                return Forbid(result.Error.Message);
            }
            else if (result.Error == AuthenticationErrors.PasswordChange)
            {
                return StatusCode(500, result.Error.Message);
            }
            return Ok();
        }
        
        [HttpGet("refreshaccess")]
        [ProducesResponseType(typeof(void), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(void), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(string), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = "refreshTokenCookie", Policy = "RefreshTokenPolicy")]
        public async Task<IActionResult> RefreshAccess()
        {
            var result = await _authService.RefreshAccessTokenAsync();
            if (result.Error == AuthenticationErrors.WrongToken)
            {
                return BadRequest(result.Error.Message);
            }
            else if (result.Error == AuthenticationErrors.UserNotFound)
            {
                return NotFound(result.Error.Message);
            }
            return Ok(result.Value);
        }
    }
}
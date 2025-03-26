using Application.Dto.UserDto;
using Application.Interfaces;
using Application.Services.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("/register")]
        public async Task<ActionResult> Register(RegisterUserDto registerUserDto)
        {
            var registerResult = await _userRepository.RegisterUserAsync(registerUserDto);

            return Ok(registerResult);
        }

        [HttpPost("/login")]
        public async Task<ActionResult> Login(LoginUserDto loginUserDto)
        {
            var loginResult = await _userRepository.LoginUserAsync(loginUserDto);

            return Ok(loginResult);
        }

        [HttpGet("user/me")]
        [Authorize]
        public async Task<IActionResult> GetUser([FromHeader] string authorization)
        {
            try
            {
                var token = authorization.Replace("Bearer ", "");
                var claims = await new JwtService().DecodeJwtAsync(token);

                return Ok(new
                {
                    Id = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                    Name = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                    Email = claims["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"]
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "Invalid JWT token", error = ex.Message });
            }
        }
    }
}

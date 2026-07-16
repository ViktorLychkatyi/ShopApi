using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShopApplication.DTOs.UserDTOs;
using ShopApplication.Interfaces.Services;

namespace ShopApi.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class AuthController(IAuthService _authService) : ControllerBase
    {
        //[Authorize]
        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] UserCreateDTO dto)
        {
            var user = await _authService.RegisterAsync(dto);
            if (user.User == null || user.Token == null || user.RefreshToken == null)
                return BadRequest("Користувач за таким email вже існує");

            Response.Cookies.Append(
                "refreshToken",
                user.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    //Expires = result.RefreshTokenExpires
                });

            return Ok(new { user = user.User }); // token = user.Token // refresh = user.RefreshToken
        }

        //[Authorize]
        [HttpPost("login")]
        public async Task<IActionResult> LoginUser([FromBody] UserLoginDTO dto)
        {
            var user = await _authService.LoginAsync(dto.Email, dto.Password);
            if (user.User == null || user.Token == null)
                return Unauthorized("Невірний email або пароль");

            Response.Cookies.Append(
                "refreshToken",
                user.RefreshToken,
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    //Expires = result.RefreshTokenExpires
                });

            return Ok(new { user = user.User }); // token = user.Token // refresh = user.RefreshToken
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Відсутній refresh token");

            var user = await _authService.RefreshTokenAsync(refreshToken);
            if (user.User == null || user.Token == null)
                return Unauthorized("Невірний refresh token");

            Response.Cookies.Append(
                "refreshToken", 
                user.RefreshToken, 
                new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    //Expires = result.RefreshTokenExpires
                });

            return Ok(new { user = user.User }); // token = user.Token // refresh = user.RefreshToken
        }
    }
}

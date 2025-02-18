using Microsoft.AspNetCore.Mvc;
using TaskManagerApp.DTO;
using TaskManagerApp.Service;
using TaskManagerApp.Service.Impl;

namespace TaskManagerApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
               var result = await _authService.RegisterAsync(registerDto);
                if (result.Succeeded) 
                { 
                    return Ok(new {message = "Kullanıcı başarıyla oluşturuldu." });
                }
                    return BadRequest("Yeni Kullanıcı oluşturulamadı , Sunucu olarak özür dileriz ! ");
            }
            catch (InvalidOperationException)
            {
                return BadRequest("Bu e-posta adresi zaten kullanımda.");
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var token = await _authService.AuthenticateAsync(loginDto.Email, loginDto.Password);
                return Ok(new { Token = token });
            }
            catch (UnauthorizedAccessException)
            {
                return Unauthorized("Geçersiz kullanıcı adı veya şifre.");
            }
        }
    }
}

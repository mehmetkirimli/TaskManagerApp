using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagerApp.DTO;
using TaskManagerApp.Entity;
using TaskManagerApp.Service.Impl;

namespace TaskManagerApp.Service
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;

        public AuthService(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterDto registerDto)
        {
            var user = new ApplicationUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                // 🔹 JSON formatında hata mesajları oluştur
                var errorMessages = new List<string>();

                foreach (var error in result.Errors)
                {
                    switch (error.Code)
                    {
                        case "PasswordRequiresNonAlphanumeric":
                            errorMessages.Add("Şifreniz en az bir özel karakter (!@#$%^&* gibi) içermelidir.");
                            break;
                        case "PasswordRequiresUpper":
                            errorMessages.Add("Şifreniz en az bir büyük harf (A-Z) içermelidir.");
                            break;
                        case "PasswordRequiresDigit":
                            errorMessages.Add("Şifreniz en az bir rakam (0-9) içermelidir.");
                            break;
                        case "PasswordTooShort":
                            errorMessages.Add("Şifreniz en az 6 karakter olmalıdır.");
                            break;
                        default:
                            errorMessages.Add(error.Description); // Diğer hata mesajlarını olduğu gibi ekleyelim
                            break;
                    }
                }

                // JSON formatında hata mesajlarını döndür
                return IdentityResult.Failed(result.Errors.ToArray());
            }

            // Kayıt başarılı mesajı
            return IdentityResult.Success;
        }


        // Kullanıcıyı doğrula ve JWT Token üret
        public async Task<string> GenerateJWTToken(ApplicationUser user)
        {
            // Kullanıcıdan claim'leri alıyoruz.
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                // İhtiyaca göre başka claim'ler ekleyebilirsiniz.
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Geçerlilik süresi
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Kullanıcıyı giriş yaptırıp token al
        public async Task<string> AuthenticateAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, password))
            {
                throw new UnauthorizedAccessException("Geçersiz kullanıcı adı ya da şifre.");
            }

            // JWT token üret
            return await GenerateJWTToken(user);
        }
    }
}

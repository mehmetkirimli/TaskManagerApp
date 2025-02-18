using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace TaskManagerApp.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;

        public TokenValidationMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            _secretKey = configuration["Jwt:Key"] ?? throw new ArgumentNullException(nameof(configuration), "Jwt:Key configuration is missing");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                var principal = ValidateToken(token);
                if (principal != null)
                {
                    // Kullanıcı bilgilerini HttpContexte ekleyelim
                    context.User = principal;
                }
            }

            await _next(context);

        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch
            {
                return null; // Token geçersizse null döner
            }
        }
    }
}

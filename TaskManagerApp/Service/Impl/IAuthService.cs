using Microsoft.AspNetCore.Identity;
using TaskManagerApp.DTO;
using TaskManagerApp.Entity;

namespace TaskManagerApp.Service.Impl
{
    public interface IAuthService
    {
        Task<IdentityResult> RegisterAsync(RegisterDto registerDto);
        Task<string> GenerateJWTToken(ApplicationUser user);
        Task<string> AuthenticateAsync(string username, string password);
    }
}

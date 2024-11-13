using Backend.Core.Models.Auth;

namespace Backend.Core.Services
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AuthDto user, IEnumerable<string> roles);
        string GenerateExpiredToken(AuthDto user, IEnumerable<string> roles);
    }
}

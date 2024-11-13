using Backend.Core.Models.Auth;

namespace Backend.Core.Repositories
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(AuthDto user, IEnumerable<string> roles);
        string GenerateExpiredToken(AuthDto user, IEnumerable<string> roles);
    }
}

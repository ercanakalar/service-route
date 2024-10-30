using Backend.Core.Models.Auth;

namespace Backend.Core.Repositories
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Auth user, IEnumerable<string> roles);
        string GenerateExpiredToken(Auth user, IEnumerable<string> roles);
    }
}

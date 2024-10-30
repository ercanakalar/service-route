using Backend.Core.Models.User;

namespace Backend.Core.Repositories
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user, IEnumerable<string> roles);
        string GenerateExpiredToken(User user, IEnumerable<string> roles);
    }
}

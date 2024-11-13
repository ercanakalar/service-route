using System.Threading.Tasks;
using Backend.Core.Models.Auth;

namespace Backend.Core.Repositories
{
    public interface IAuthRepository : IRepository<AuthDto>
    {
        Task<AuthResponse> Signin(SigninRequest request);
        Task<AuthDto> GetByEmailAsync(string email);
        Task<List<AllUsers>> GetUsers();
        Task<AuthGeneralResponse> GetUserById(int id);
        Task<int> GetCompanyIdByUserId(int userId);
    }
}

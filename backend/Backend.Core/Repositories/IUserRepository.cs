using System.Threading.Tasks;
using Backend.Core.Models.Auth;

namespace Backend.Core.Repositories
{
    public interface IUserRepository: IRepository<Auth>
    {
        Task<AuthResponse> Signin(SigninRequest request);
        Task<Auth> GetByEmailAsync(string email);
        Task<List<AllUsers>> GetUsers();
        Task<AuthGeneralResponse> GetUserById(int id);
    }
}

using System.Threading.Tasks;
using Backend.Core.Models.User;

namespace Backend.Core.Repositories
{
    public interface IUserRepository : IRepository<UserDto> {
        Task<int> GetCompanyIdByUserId(int userId);
    }
}

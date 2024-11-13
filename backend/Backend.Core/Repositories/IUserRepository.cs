using System.Threading.Tasks;
using Backend.Core.Models.User;

namespace Backend.Core.Repositories
{
    public interface IUserRepository : IRepository<User> {
        Task<int> GetCompanyIdByUserId(int userId);
    }
}

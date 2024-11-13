using System.Threading.Tasks;
using Backend.Core.Models.Company;

namespace Backend.Core.Repositories
{
    public interface ICompanyRepository : IRepository<Company>
    {
        Task<Company> GetByEmailAsync(string email);
        Task<Company> GetByNameAsync(string name);
        Task<Company> GetByIdAsync(int id);
    }
}

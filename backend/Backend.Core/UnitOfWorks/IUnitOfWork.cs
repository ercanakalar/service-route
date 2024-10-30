using System.Threading.Tasks;
using Backend.Core.Repositories;

namespace Backend.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IUserRepository Auth { get; }
        ICompanyRepository Companies { get; }

        Task CommitAsync();
        void Commit();

        Task UpdateAsync();
    }
}

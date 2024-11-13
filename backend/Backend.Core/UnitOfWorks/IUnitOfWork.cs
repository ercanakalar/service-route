using System.Threading.Tasks;
using Backend.Core.Repositories;

namespace Backend.Core.UnitOfWorks
{
    public interface IUnitOfWork
    {
        IAuthRepository Auth { get; }
        ICompanyRepository Companies { get; }
        IMapRepository Waypoints { get; }

        Task CommitAsync();
        void Commit();

        Task UpdateAsync();
    }
}

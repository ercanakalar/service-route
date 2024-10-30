using System.Threading.Tasks;
using Backend.Core.Repositories;
using Backend.Core.UnitOfWorks;
using Backend.Data.Repositories;

namespace Backend.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        private IUserRepository _userRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IUserRepository Users =>
            _userRepository ??= (IUserRepository)new UserRepository(_appDbContext);

        public void Commit()
        {
            _appDbContext.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync()
        {
            await _appDbContext.SaveChangesAsync();
        }
    }
}

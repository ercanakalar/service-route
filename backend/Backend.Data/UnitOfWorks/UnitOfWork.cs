using System.Threading.Tasks;
using Backend.Core.Repositories;
using Backend.Core.UnitOfWorks;
using Backend.Data.Repositories;

namespace Backend.Data.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _appDbContext;

        private IAuthRepository _userRepository;
        private ICompanyRepository _companyRepository;
        private IMapRepository _mapRepository;

        public UnitOfWork(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IAuthRepository Auth =>
            _userRepository ??= (IAuthRepository)new AuthRepository(_appDbContext);

        public ICompanyRepository Companies =>
            _companyRepository ??= (ICompanyRepository)new CompanyRepository(_appDbContext);

        public IMapRepository Waypoints =>
            _mapRepository ??= (IMapRepository)new MapRepository(_appDbContext);

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

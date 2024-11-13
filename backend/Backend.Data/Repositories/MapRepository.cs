using System.Threading.Tasks;
using Backend.Core.Models.Map;
using Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    public class MapRepository : Repository<Waypoints>, IMapRepository
    {
        public AppDbContext appDbContext
        {
            get => _context as AppDbContext;
        }

        public MapRepository(AppDbContext context)
            : base(context) { }

        public async Task<List<Waypoints>> GetWaypointsByCompanyId(int companyId)
        {
            return await _context.Waypoints.Where(x => x.CompanyId == companyId).ToListAsync();
        }
    }
}

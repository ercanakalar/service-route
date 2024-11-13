using System.Threading.Tasks;
using Backend.Core.Models.Map;

namespace Backend.Core.Repositories
{
    public interface IMapRepository : IRepository<Waypoints>
    {
        Task<List<Waypoints>> GetWaypointsByCompanyId(int companyId);
    }
}

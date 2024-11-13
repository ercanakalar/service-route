using System.Threading.Tasks;
using Backend.Core.Models.Map;

namespace Backend.Core.Repositories
{
    public interface IMapRepository : IRepository<WaypointsDto>
    {
        Task<List<WaypointsDto>> GetWaypointsByCompanyId(int companyId);
    }
}

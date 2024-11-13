using Backend.Core.Models.Map;

namespace Backend.Core.Services
{
    public interface IMapService
    {
        Task<MapResponse> InsertWaypoints(MapRequest request, int userId);
        Task<MapResponse> GetWaypointsAsAdmin();
        Task<MapResponse> GetWaypointsAsUser(int userId);
    }
}

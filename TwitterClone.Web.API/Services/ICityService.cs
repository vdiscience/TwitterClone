using TwitterClone.Web.API.DTOs;
using TwitterClone.Web.API.Services.Models;

namespace TwitterClone.Web.API.Services
{
    public interface ICityService
    {
        Task<PagedResult<CityResponse>> GetCitiesAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken);
        Task<CityResponse?> GetCityAsync(Guid id, CancellationToken cancellationToken);
        Task<Result<CityResponse>> CreateCityAsync(CityCreateRequest request, CancellationToken cancellationToken);
        Task<Result> UpdateCityAsync(Guid id, CityUpdateRequest request, CancellationToken cancellationToken);
        Task<Result> DeleteCityAsync(Guid id, CancellationToken cancellationToken);
    }
}
using TwitterCloneBackend.Entities.Models;

namespace TwitterClone.Web.API.Repositories
{
    internal interface ICityRepository
    {
        Task AddAsync(City city, CancellationToken cancellationToken);
        Task<bool> ExistsByNameAsync(string cityName, CancellationToken cancellationToken);
        Task<City> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<(IReadOnlyList<City> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search, CancellationToken cancellationToken);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
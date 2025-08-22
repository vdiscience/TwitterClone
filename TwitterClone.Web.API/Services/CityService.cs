using Microsoft.EntityFrameworkCore;
using TwitterClone.Web.API.DTOs;
using TwitterClone.Web.API.Repositories;
using TwitterClone.Web.API.Services.Models;
using TwitterCloneBackend.Entities.Models;

namespace TwitterClone.Web.API.Services;

internal sealed class CityService : ICityService
{
    #region Fields
    private readonly ICityRepository _repo;
    private readonly ILogger<CityService> _logger;
    private readonly DbContext _dbContext; // For SaveChanges exception typing (optional)

    #endregion

    #region Constructor
    public CityService(ICityRepository repo, ILogger<CityService> logger)
    {
        _repo = repo;
        _logger = logger;
        _dbContext = (DbContext?)repo.GetType()
            .GetField("_context", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.GetValue(repo) ?? throw new InvalidOperationException("DataContext unavailable.");
    }
    #endregion

    #region Public Methods
    public async Task<PagedResult<CityResponse>> GetCitiesAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken)
    {
        (var items, var total) = await _repo.GetPagedAsync(pageNumber, pageSize, search, cancellationToken);

        var responses = items.Select(ToResponse).ToList();

        return new PagedResult<CityResponse>(responses, total, pageNumber, pageSize);
    }

    public Task<CityResponse?> GetCityAsync(Guid id, CancellationToken cancellationToken) =>
        GetCityInternalAsync(id, cancellationToken);

    public async Task<Result<CityResponse>> CreateCityAsync(CityCreateRequest request, CancellationToken cancellationToken)
    {
        var trimmed = request.CityName.Trim();

        if (await _repo.ExistsByNameAsync(trimmed, cancellationToken))
        {
            return Result<CityResponse>.Fail("City name already exists.");
        }

        var city = new City
        {
            Id = Guid.NewGuid(),
            CityName = trimmed,
            DateTimeEntered = DateTime.UtcNow,
            Deleted = 0
        };

        await _repo.AddAsync(city, cancellationToken);
        await _repo.SaveChangesAsync(cancellationToken);

        return Result<CityResponse>.Ok(ToResponse(city));
    }

    public async Task<Result> UpdateCityAsync(Guid id, CityUpdateRequest request, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Set<City>().FirstOrDefaultAsync(c => c.Id == id && c.Deleted == 0, cancellationToken);

        if (city == null)
        {
            return Result.Fail("NotFound");
        }

        city.CityName = request.CityName.Trim();

        try
        {
            await _repo.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error updating city {CityId}", id);
            return Result.Fail("Update failed.");
        }

        return Result.Ok();
    }

    public async Task<Result> DeleteCityAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _dbContext.Set<City>().FirstOrDefaultAsync(c => c.Id == id && c.Deleted == 0, cancellationToken);

        if (city == null)
        {
            return Result.Fail("NotFound");
        }

        city.Deleted = 1;
        city.DateTimeDeleted = DateTime.UtcNow;

        try
        {
            await _repo.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting city {CityId}", id);
            return Result.Fail("Delete failed.");
        }

        return Result.Ok();
    }

    #endregion

    #region Private Methods
    private async Task<CityResponse?> GetCityInternalAsync(Guid id, CancellationToken cancellationToken)
    {
        var city = await _repo.GetByIdAsync(id, cancellationToken);

        return city == null ? null : ToResponse(city);
    }

    private static CityResponse ToResponse(City c) =>
        new CityResponse(c.Id, c.CityName, c.DateTimeEntered, c.Deleted == 1, c.DateTimeDeleted == default ? null : c.DateTimeDeleted);

    #endregion
}
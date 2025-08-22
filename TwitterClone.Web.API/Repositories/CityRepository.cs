using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.Entities;
using TwitterCloneBackend.Entities.Models;

namespace TwitterClone.Web.API.Repositories;

internal sealed class CityRepository : ICityRepository, IAsyncDisposable, IDisposable
{
    // Disposal is not required here when the repository is registered with DI and receives a scoped DataContext.
    // The container disposes the context at the end of the scope.
    // Adding disposal that disposes the injected context can cause ObjectDisposedExceptions if other services share the same scope.

    #region Fields
    private readonly DataContext _context;
    private readonly bool _ownsContext;
    private bool _disposed;

    #endregion

    #region Constructors
    // DI constructor (does NOT own the context)
    public CityRepository(DataContext context)
    {
        _context = context;
        _ownsContext = false;
    }

    // Optional manual usage constructor (repository OWNS the context)
    public CityRepository(DbContextOptions<DataContext> options)
    {
        _context = new DataContext(options);
        _ownsContext = true;
    }
    #endregion

    #region Public Methods
    public async Task<(IReadOnlyList<City> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? search, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        IQueryable<City> query = _context.Cities.AsNoTracking().Where(c => c.Deleted == 0);

        if (!string.IsNullOrWhiteSpace(search))
        {
            search = search.Trim();
            query = query.Where(c => EF.Functions.Like(c.CityName, $"%{search}%"));
        }

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(c => c.CityName)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, total);
    }

    public Task<City?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return _context.Cities.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id && c.Deleted == 0, cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string cityName, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return _context.Cities.AnyAsync(c => c.CityName == cityName && c.Deleted == 0, cancellationToken);
    }

    public Task AddAsync(City city, CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return _context.Cities.AddAsync(city, cancellationToken).AsTask();
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        ThrowIfDisposed();

        return _context.SaveChangesAsync(cancellationToken);
    }
    #endregion

    #region Disposal
    private void ThrowIfDisposed()
    {
        if (_disposed)
        {
            throw new ObjectDisposedException(nameof(CityRepository));
        }
    }

    public void Dispose()
    {
        if (_disposed) return;

        if (_ownsContext)
        {
            _context.Dispose();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }

    public async ValueTask DisposeAsync()
    {
        if (_disposed) return;

        if (_ownsContext)
        {
            await _context.DisposeAsync();
        }

        _disposed = true;
        GC.SuppressFinalize(this);
    }
    #endregion
}
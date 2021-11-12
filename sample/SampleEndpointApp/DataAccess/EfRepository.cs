using Microsoft.EntityFrameworkCore;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.DataAccess;

/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// Check it out if you need filtering/paging/etc.
/// Also consider Ardalis.Specification and its built-in generic repository
/// </summary>
public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
{
  protected readonly AppDbContext _dbContext;

  public EfRepository(AppDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public virtual async Task<T> GetByIdAsync(int id, CancellationToken cancellationToken)
  {
    return await _dbContext.Set<T>().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
  }

  public async Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken)
  {
    return await _dbContext.Set<T>().ToListAsync(cancellationToken);
  }

  /// <inheritdoc />
  public async Task<IReadOnlyList<T>> ListAllAsync(
    int perPage,
    int page,
          CancellationToken cancellationToken)
  {
    return await _dbContext.Set<T>().Skip(perPage * (page - 1)).Take(perPage).ToListAsync(cancellationToken);
  }

  public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
  {
    await _dbContext.Set<T>().AddAsync(entity, cancellationToken);
    await _dbContext.SaveChangesAsync(cancellationToken);

    return entity;
  }

  public async Task UpdateAsync(T entity, CancellationToken cancellationToken)
  {
    _dbContext.Entry(entity).State = EntityState.Modified;
    await _dbContext.SaveChangesAsync(cancellationToken);
  }

  public async Task DeleteAsync(T entity, CancellationToken cancellationToken)
  {
    _dbContext.Set<T>().Remove(entity);
    await _dbContext.SaveChangesAsync(cancellationToken);
  }
}

namespace SampleEndpointApp.DomainModel;

/// <summary>
/// Source: My reference app https://github.com/dotnet-architecture/eShopOnWeb
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IAsyncRepository<T> where T : BaseEntity
{
  Task<T> GetByIdAsync(int id, CancellationToken cancellationToken);

  Task<IReadOnlyList<T>> ListAllAsync(CancellationToken cancellationToken);

  Task<IReadOnlyList<T>> ListAllAsync(int perPage, int page, CancellationToken cancellationToken);

  //Task<IReadOnlyList<T>> ListAsync(ISpecification<T> spec);

  Task<T> AddAsync(T entity, CancellationToken cancellationToken);

  Task UpdateAsync(T entity, CancellationToken cancellationToken);

  Task DeleteAsync(T entity, CancellationToken cancellationToken);

  //Task<int> CountAsync(ISpecification<T> spec);
}

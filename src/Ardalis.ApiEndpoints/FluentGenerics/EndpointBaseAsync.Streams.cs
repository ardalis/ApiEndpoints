namespace Ardalis.ApiEndpoints;

#if NETSTANDARD2_1
public static partial class EndpointBaseAsync
{
  public static partial class WithRequest<TRequest>
  {
    public abstract class WithAsyncEnumerableResult<T> : EndpointBase
    {
      public abstract IAsyncEnumerable<T> HandleAsync(
        TRequest request,
        CancellationToken cancellationToken = default
      );
    }
  }
}
#endif

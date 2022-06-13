using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints;

/// <summary>
/// A base class for an endpoint that accepts parameters.
/// </summary>
public static class EndpointBaseAsync
{
  public static class WithRequest<TRequest>
  {
    public abstract class WithResult<TResponse> : EndpointBase
    {
      public abstract Task<TResponse> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointBase
    {
      public abstract Task HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult<TResponse> : EndpointBase
    {
      public abstract Task<ActionResult<TResponse>> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult : EndpointBase
    {
      public abstract Task<ActionResult> HandleAsync(
          TRequest request,
          CancellationToken cancellationToken = default
      );
    }
  }

  public static class WithIdAndRequest<TId,TRequest>
  {
    public abstract class WithResult<TResponse> : EndpointBase
    {
      public abstract Task<TResponse> HandleAsync(
          TId id, TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointBase
    {
      public abstract Task HandleAsync(
          TId id, TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult<TResponse> : EndpointBase
    {
      public abstract Task<ActionResult<TResponse>> HandleAsync(
          TId id, TRequest request,
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult : EndpointBase
    {
      public abstract Task<ActionResult> HandleAsync(
          TId id, TRequest request,
          CancellationToken cancellationToken = default
      );
    }
  }


  public static class WithoutRequest
  {
    public abstract class WithResult<TResponse> : EndpointBase
    {
      public abstract Task<TResponse> HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithoutResult : EndpointBase
    {
      public abstract Task HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult<TResponse> : EndpointBase
    {
      public abstract Task<ActionResult<TResponse>> HandleAsync(
          CancellationToken cancellationToken = default
      );
    }

    public abstract class WithActionResult : EndpointBase
    {
      public abstract Task<ActionResult> HandleAsync(
          CancellationToken cancellationToken = default
      );
    }
  }
}

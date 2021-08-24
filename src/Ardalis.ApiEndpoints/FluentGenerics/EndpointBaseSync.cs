using Microsoft.AspNetCore.Mvc;

namespace Ardalis.ApiEndpoints
{
    /// <summary>
    /// A base class for an endpoint that accepts parameters.
    /// </summary>
    public static class EndpointBaseSync
    {
        public static class WithRequest<TRequest>
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                public abstract ActionResult<TResponse> Handle(TRequest request);
            }

            public abstract class WithoutResponse : EndpointBase
            {
                public abstract ActionResult Handle(TRequest request);
            }
        }

        public static class WithoutRequest
        {
            public abstract class WithResponse<TResponse> : EndpointBase
            {
                public abstract ActionResult<TResponse> Handle();
            }

            public abstract class WithoutResponse : EndpointBase
            {
                public abstract ActionResult Handle();
            }
        }
    }
}

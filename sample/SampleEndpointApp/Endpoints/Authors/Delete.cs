using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    public class Delete : EndpointBaseAsync
        .WithRequest<DeleteAuthorRequest>
        .WithActionResult
    {
        private readonly IAsyncRepository<Author> _repository;

        public Delete(IAsyncRepository<Author> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Deletes an Author
        /// </summary>
        [HttpDelete("api/[namespace]/{id}")]
        public override async Task<ActionResult> HandleAsync([FromRoute] DeleteAuthorRequest request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id, cancellationToken);

            if (author is null)
            {
                return NotFound(request.Id);
            }

            await _repository.DeleteAsync(author, cancellationToken);

            // see https://restfulapi.net/http-methods/#delete
            return NoContent();
        }
    }
}

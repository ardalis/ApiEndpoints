using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Authors
{
    public class Delete : BaseAsyncEndpoint
        .WithRequest<DeleteAuthorRequest>
        .WithoutResponse
    {
        private readonly IAsyncRepository<Author> _repository;

        public Delete(IAsyncRepository<Author> repository)
        {
            _repository = repository;
        }

        [HttpDelete("/authors/{id}")]
		[SwaggerOperation(
			Summary = "Deletes an Author",
			Description = "Deletes an Author",
			OperationId = "Author.Delete",
			Tags = new[] { "AuthorEndpoint" })
		]
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

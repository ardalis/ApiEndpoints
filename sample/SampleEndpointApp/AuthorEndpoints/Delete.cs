using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;

namespace SampleEndpointApp.Authors
{
    public class Delete : BaseAsyncEndpoint<int, DeletedAuthorResult>
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
        public override async Task<ActionResult<DeletedAuthorResult>> HandleAsync(int id)
        {
            var author = await _repository.GetByIdAsync(id);
            if (author == null) return NotFound(id);
            await _repository.DeleteAsync(author);

            return Ok(new DeletedAuthorResult { DeletedAuthorId = id });
        }
    }
}
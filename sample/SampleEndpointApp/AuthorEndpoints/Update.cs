using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Authors
{
    public class Update : BaseAsyncEndpoint<UpdateAuthorCommand, UpdatedAuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Update(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPut("/authors")]
		[SwaggerOperation(
			Summary = "Updates an existing Author",
			Description = "Updates an existing Author",
			OperationId = "Author.Update",
			Tags = new[] { "AuthorEndpoint" })
		]
        public override async Task<ActionResult<UpdatedAuthorResult>> HandleAsync([FromBody]UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(request.Id, cancellationToken);
            _mapper.Map(request, author);
            await _repository.UpdateAsync(author, cancellationToken);

            var result = _mapper.Map<UpdatedAuthorResult>(author);
            return Ok(result);
        }
    }
}

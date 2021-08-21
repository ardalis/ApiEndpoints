using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace SampleEndpointApp.Authors
{
    public partial class Create : BaseEndpoint
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Create(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "Creates a new Author",
            Description = "Creates a new Author",
            OperationId = "Author.Create",
            Tags = new[] { "AuthorEndpoint" })
        ]
        public async Task<ActionResult<CreateAuthorResult>> HandleAsync([FromBody]CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            _mapper.Map(request, author);
            await _repository.AddAsync(author, cancellationToken);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return Ok(result);
        }
    }
}

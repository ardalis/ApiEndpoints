using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Create : EndpointBaseAsync
        .WithRequest<CreateAuthorCommand>
        .WithResponse<CreateAuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Create(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new Author
        /// </summary>
        [HttpPost(CreateAuthorCommand.ROUTE)]
        public override async Task<ActionResult<CreateAuthorResult>> HandleAsync([FromBody]CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            _mapper.Map(request, author);
            await _repository.AddAsync(author, cancellationToken);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return Ok(result);
        }
    }
}

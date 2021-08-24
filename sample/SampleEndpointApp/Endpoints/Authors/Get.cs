using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    [Route("api/[namespace]")]
    public class Get : EndpointBaseAsync
        .WithRequest<int>
        .WithResult<AuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Get(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get a specific Author
        /// </summary>
        [HttpGet("{id}", Name = "Authors.Get")]
        public override async Task<AuthorResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            var result = _mapper.Map<AuthorResult>(author);

            return result;
        }
    }
}

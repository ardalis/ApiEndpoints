using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    public partial class Create : EndpointBase
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
        [HttpPost]
        public async Task<CreateAuthorResult> HandleAsync([FromBody] CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            _mapper.Map(request, author);
            await _repository.AddAsync(author, cancellationToken);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return result;
        }
    }
}

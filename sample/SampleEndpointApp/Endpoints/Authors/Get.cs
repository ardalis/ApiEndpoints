﻿using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Annotations;
using System.Threading;

namespace SampleEndpointApp.Endpoints.Authors
{
    public partial class Get : EndpointBase
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Get(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
		[SwaggerOperation(
			Summary = "Get a specific Author",
			Description = "Get a specific Author",
			OperationId = "Author.Get",
			Tags = new[] { "AuthorEndpoint" })
		]
        public async Task<AuthorResult> HandleAsync(int id, CancellationToken cancellationToken)
        {
            var author = await _repository.GetByIdAsync(id, cancellationToken);

            var result = _mapper.Map<AuthorResult>(author);
            return result;
        }
    }
}

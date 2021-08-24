﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    [Route("api/[namespace]")]
    public class Create : EndpointBaseAsync
        .WithRequest<CreateAuthorCommand>
        .WithActionResult
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
        public override async Task<ActionResult> HandleAsync([FromBody] CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            var author = new Author();
            _mapper.Map(request, author);
            await _repository.AddAsync(author, cancellationToken);

            var result = _mapper.Map<CreateAuthorResult>(author);
            return CreatedAtRoute("Authors.Get", new { id = result.Id }, result);
        }
    }
}

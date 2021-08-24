﻿using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class Update : EndpointBaseAsync
        .WithRequest<UpdateAuthorCommand>
        .WithActionResult<UpdatedAuthorResult>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public Update(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Updates an existing Author
        /// </summary>
        [HttpPut("/authors")]
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

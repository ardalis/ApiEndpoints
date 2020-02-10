using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SampleEndpointApp.Authors
{
    public class List : BaseAsyncEndpoint<List<AuthorListResult>>
    {
        private readonly IAsyncRepository<Author> _repository;
        private readonly IMapper _mapper;

        public List(IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet("/authors")]
        public override async Task<ActionResult<List<AuthorListResult>>> HandleAsync()
        {
            var result = (await _repository.ListAllAsync())
                .Select(i => _mapper.Map<AuthorListResult>(i));

            return Ok(result);
        }
    }
}
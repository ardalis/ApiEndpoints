using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Linq;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Collections.Generic;

namespace SampleEndpointApp.Authors
{
    public class List : BaseAsyncEndpoint
        .WithRequest<AuthorListRequest>
        .WithResponse<IList<AuthorListResult>>
    {
        private readonly IAsyncRepository<Author> repository;
        private readonly IMapper mapper;

        public List(
            IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("/authors")]
        [SwaggerOperation(
            Summary = "List all Authors",
            Description = "List all Authors",
            OperationId = "Author.List",
            Tags = new[] { "AuthorEndpoint" })
        ]
        public override async Task<ActionResult<IList<AuthorListResult>>> HandleAsync(

            [FromQuery] AuthorListRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request.PerPage == 0)
            {
                request.PerPage = 10;
            }
            if (request.Page == 0)
            {
                request.Page = 1;
            }
            var result = (await repository.ListAllAsync(request.PerPage, request.Page, cancellationToken))
                .Select(i => mapper.Map<AuthorListResult>(i));

            return Ok(result);
        }
    }
}

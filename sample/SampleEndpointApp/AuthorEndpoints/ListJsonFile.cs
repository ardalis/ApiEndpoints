using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using Swashbuckle.AspNetCore.Annotations;

namespace SampleEndpointApp.Authors
{
    /// <summary>
    /// Provides a list of authors in a JSON file format
    /// </summary>
    public class ListJsonFile : EndpointBaseAsync
        .WithoutRequest
        .WithoutResponse // TODO: Maybe have a custom file response?
    {
        private readonly IAsyncRepository<Author> repository;
        private readonly IMapper mapper;

        public ListJsonFile(
            IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            this.repository = repository;
            this.mapper = mapper;
        }
        [HttpGet("/authorsJson")]
        [SwaggerOperation(
            Summary = "List all Authors as a JSON file",
            Description = "List all Authors as a JSON file",
            OperationId = "Author.List",
            Tags = new[] { "AuthorEndpoint" })
        ]
        public override async Task<ActionResult> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var result = (await repository.ListAllAsync(cancellationToken)).ToList();

             var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
            return File(streamData, "text/json", "authors.json");
        }
    }
}

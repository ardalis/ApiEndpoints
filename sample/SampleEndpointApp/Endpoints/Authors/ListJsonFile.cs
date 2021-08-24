using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    [Route("api/[namespace]")]
    public class ListJsonFile : EndpointBaseAsync
        .WithoutRequest
        .WithActionResult
    {
        private readonly IAsyncRepository<Author> repository;

        public ListJsonFile(IAsyncRepository<Author> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// List all Authors as a JSON file
        /// </summary>
        [HttpGet("Json")]
        public override async Task<ActionResult> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var result = (await repository.ListAllAsync(cancellationToken)).ToList();

            var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
            return File(streamData, "text/json", "authors.json");
        }
    }
}

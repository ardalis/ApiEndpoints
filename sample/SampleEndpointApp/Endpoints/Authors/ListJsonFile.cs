using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Authors
{
    public class ListJsonFile : EndpointBaseAsync
        .WithoutRequest
        .WithoutResponse // TODO: Maybe have a custom file response?
    {
        private readonly IAsyncRepository<Author> repository;

        public ListJsonFile(IAsyncRepository<Author> repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// List all Authors as a JSON file
        /// </summary>
        [HttpGet("/authorsJson")]
        public override async Task<ActionResult> HandleAsync(
            CancellationToken cancellationToken = default)
        {
            var result = (await repository.ListAllAsync(cancellationToken)).ToList();

             var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
            return File(streamData, "text/json", "authors.json");
        }
    }
}

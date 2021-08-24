using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;

namespace SampleEndpointApp.Endpoints.Authors
{
    public partial class ListJsonFile : EndpointBase
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
        public async Task<FileContentResult> HandleAsync(CancellationToken cancellationToken)
        {
            var result = await repository.ListAllAsync(cancellationToken);

            var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
            return File(streamData, "text/json", "authors.json");
        }
    }
}

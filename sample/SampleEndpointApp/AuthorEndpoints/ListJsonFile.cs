using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using System.Linq;
using System.Threading.Tasks;

using Swashbuckle.AspNetCore.Annotations;
using System.Threading;
using System.Collections.Generic;
using System;
using System.Text.Json;

namespace SampleEndpointApp.Authors
{
    /// <summary>
    /// Provides a list of authors in a JSON file format
    /// </summary>
    public class ListJsonFile : BaseAsyncEndpoint
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

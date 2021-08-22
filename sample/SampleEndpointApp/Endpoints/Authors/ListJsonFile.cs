﻿using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.ApiEndpoints;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleEndpointApp.DomainModel;
using Swashbuckle.AspNetCore.Annotations;

namespace SampleEndpointApp.Endpoints.Authors
{
    /// <summary>
    /// Provides a list of authors in a JSON file format
    /// </summary>
    public partial class ListJsonFile : EndpointBase
    {
        private readonly IAsyncRepository<Author> repository;

        public ListJsonFile(
            IAsyncRepository<Author> repository,
            IMapper mapper)
        {
            this.repository = repository;
        }

        [HttpGet("Json")]
        [SwaggerOperation(
            Summary = "List all Authors as a JSON file",
            Description = "List all Authors as a JSON file",
            OperationId = "Author.List",
            Tags = new[] { "AuthorEndpoint" })
        ]
        public async Task<ActionResult> HandleAsync(CancellationToken cancellationToken)
        {
            var result = await repository.ListAllAsync(cancellationToken);

             var streamData = JsonSerializer.SerializeToUtf8Bytes(result);
            return File(streamData, "text/json", "authors.json");
        }
    }
}
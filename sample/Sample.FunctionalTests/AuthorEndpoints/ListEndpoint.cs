﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.HttpClientTestExtensions;
using Newtonsoft.Json;
using Sample.FunctionalTests.Models;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
  public class ListEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
  {
    private readonly HttpClient _client;

    public ListEndpoint(CustomWebApplicationFactory<Startup> factory)
    {
      _client = factory.CreateClient();
    }

    [Fact]
    public async Task ReturnsTwoGivenTwoAuthors()
    {
      var result = await _client.GetAndDeserialize<IEnumerable<Author>>(Routes.Authors.List());

      Assert.NotNull(result);
      Assert.Equal(SeedData.Authors().Count(), result.Count());
    }

    [Fact]
    public async Task GivenLongRunningListRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
    {
      // Arrange, generate a token source that times out instantly
      var tokenSource = new CancellationTokenSource(TimeSpan.Zero);

      // Act
      var request = _client.GetAsync(Routes.Authors.List(), tokenSource.Token);

      // Assert
      var response = await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
    }
  }
}

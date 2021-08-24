using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Sample.FunctionalTests.Models;
using SampleEndpointApp;
using SampleEndpointApp.DomainModel;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class ListPaginatedEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public ListPaginatedEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Page1PerPage1_ShouldReturnFirstAuthor()
        {
            var response = await _client.GetAsync(Routes.Authors.List(1, 1));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse);

            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact]
        public async Task GivenLongRunningPaginatedListRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));

            // Act
            var request = _client.GetAsync(Routes.Authors.List(1, 1), tokenSource.Token);

            // Assert
            var response = await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}

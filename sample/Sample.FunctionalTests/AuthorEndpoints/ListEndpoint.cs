using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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
            var response = await _client.GetAsync($"/authors");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<IEnumerable<Author>>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(SeedData.Authors().Count(), result.Count());
        }

        [Fact]
        public async Task GivenLongRunningRequest_WhenTokenSourceCallsForCancellation_RequestIsTermainated()
        {
            // Arrange, generate a token source that times out after 1 millisecond
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(1));

            // Act/Assert
            var response = await Assert.ThrowsAsync<OperationCanceledException>(async () => await _client.GetAsync("/authors", tokenSource.Token));
        }
    }
}

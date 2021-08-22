using Newtonsoft.Json;
using Sample.FunctionalTests.Models;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class GetEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GetEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsAuthorById()
        {
            var firstAuthor = SeedData.Authors().First();

            var response = await _client.GetAsync(Routes.Authors.Get(firstAuthor.Id));
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Author>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(firstAuthor.Id, result.Id);
            Assert.Equal(firstAuthor.Name, result.Name);
            Assert.Equal(firstAuthor.PluralsightUrl, result.PluralsightUrl);
            Assert.Equal(firstAuthor.TwitterAlias, result.TwitterAlias);
        }

        [Fact]
        public async Task GivenLongRunningGetRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.Zero);
            var firstAuthor = SeedData.Authors().First();

            // Act
            var request = _client.GetAsync(Routes.Authors.Get(firstAuthor.Id), tokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}

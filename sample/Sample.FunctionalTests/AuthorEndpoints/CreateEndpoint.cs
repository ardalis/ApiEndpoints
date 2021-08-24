using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.Endpoints.Authors;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class CreateEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public CreateEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreatesANewAuthor()
        {
            var newAuthor = new CreateAuthorCommand()
            {
                Name = "James Eastham",
                PluralsightUrl = "https://app.pluralsight.com",
                TwitterAlias = "jeasthamdev",
            };
            
            var lastAuthor = SeedData.Authors().Last();

            var response = await _client.PostAsync(CreateAuthorCommand.ROUTE, new StringContent(JsonConvert.SerializeObject(newAuthor), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateAuthorResult>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(result.Id, lastAuthor.Id + 1);
            Assert.Equal(result.Name, newAuthor.Name);
            Assert.Equal(result.PluralsightUrl, newAuthor.PluralsightUrl);
            Assert.Equal(result.TwitterAlias, newAuthor.TwitterAlias);
        }

        [Fact]
        public async Task GivenLongRunningCreateRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));
            var lastAuthor = SeedData.Authors().Last();
            var newAuthor = new CreateAuthorCommand()
            {
                Name = "James Eastham",
                PluralsightUrl = "https://app.pluralsight.com",
                TwitterAlias = "jeasthamdev",
            };

            // Act
            var request = _client.PostAsync(CreateAuthorCommand.ROUTE, new StringContent(JsonConvert.SerializeObject(newAuthor), Encoding.UTF8, "application/json"), tokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}

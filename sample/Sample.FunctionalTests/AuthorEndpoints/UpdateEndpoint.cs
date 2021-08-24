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
    public class UpdateEndpoint : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public UpdateEndpoint(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task UpdateAnExistingAuthor()
        {
            var updatedAuthor = new UpdateAuthorCommand()
            {
                Id = 2,
                Name = "James Eastham",
            };
            
            var authorPreUpdate = SeedData.Authors().First(p => p.Id == 2);

            var response = await _client.PutAsync($"/authors", new StringContent(JsonConvert.SerializeObject(updatedAuthor), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UpdatedAuthorResult>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(result.Id, updatedAuthor.Id.ToString());
            Assert.NotEqual(result.Name, authorPreUpdate.Name);
            Assert.Equal("James Eastham", result.Name);
            Assert.Equal(result.PluralsightUrl, authorPreUpdate.PluralsightUrl);
            Assert.Equal(result.TwitterAlias, authorPreUpdate.TwitterAlias);
        }

        [Fact]
        public async Task GivenLongRunningUpdateRequest_WhenTokenSourceCallsForCancellation_RequestIsTerminated()
        {
            // Arrange, generate a token source that times out instantly
            var tokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(0));
            var authorPreUpdate = SeedData.Authors().FirstOrDefault(p => p.Id == 2);
            var updatedAuthor = new UpdateAuthorCommand()
            {
                Id = 2,
                Name = "James Eastham",
            };

            // Act
            var request = _client.PutAsync("/authors", new StringContent(JsonConvert.SerializeObject(updatedAuthor), Encoding.UTF8, "application/json"), tokenSource.Token);

            // Assert
            await Assert.ThrowsAsync<OperationCanceledException>(async () => await request);
        }
    }
}

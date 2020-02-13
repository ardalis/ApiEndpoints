using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.Authors;
using SampleEndpointApp.DataAccess;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class DeleteTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public DeleteTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsDeletedAuthorId()
        {
            var firstAuthor = SeedData.Authors().First();

            var response = await _client.DeleteAsync($"/authors/{firstAuthor.Id}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<DeletedAuthorResult>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(firstAuthor.Id, result.DeletedAuthorId);
        }
    }
}

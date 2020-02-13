using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class GetTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;

        public GetTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task ReturnsAuthorById()
        {
            var firstAuthor = SeedData.Authors().First();

            var response = await _client.GetAsync($"/authors/{firstAuthor.Id}");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Author>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(firstAuthor.Id, result.Id);
            Assert.Equal(firstAuthor.Name, result.Name);
            Assert.Equal(firstAuthor.PluralsightUrl, result.PluralsightUrl);
            Assert.Equal(firstAuthor.TwitterAlias, result.TwitterAlias);
        }
    }
}

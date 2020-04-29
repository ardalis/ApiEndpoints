using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.Authors;
using SampleEndpointApp.DataAccess;
using SampleEndpointApp.DomainModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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

            var response = await _client.PostAsync($"/authors", new StringContent(JsonConvert.SerializeObject(newAuthor), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateAuthorResult>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(result.Id, lastAuthor.Id + 1);
            Assert.Equal(result.Name, newAuthor.Name);
            Assert.Equal(result.PluralsightUrl, newAuthor.PluralsightUrl);
            Assert.Equal(result.TwitterAlias, newAuthor.TwitterAlias);
        }
    }
}

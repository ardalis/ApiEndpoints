using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using SampleEndpointApp;
using SampleEndpointApp.Authors;
using SampleEndpointApp.DataAccess;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints
{
    public class CreateTests : IClassFixture<CustomWebApplicationFactory<Startup>>
    {
        private readonly HttpClient _client;
        private int currentMaxAuthorId;

        public CreateTests(CustomWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();

            using (var scope = factory.Services.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var appDbContext = scopedServices.GetRequiredService<AppDbContext>();

                currentMaxAuthorId = appDbContext.Authors
                    .Select(a => a.Id)
                    .Max();
            }
        }

        [Fact]
        public async Task ReturnsCreatedAuthor()
        {
            var author = SeedData.Authors()
                .First();

            var createAuthorCommand = new CreateAuthorCommand
            {
                Name = author.Name,
                PluralsightUrl = author.PluralsightUrl,
                TwitterAlias = author.TwitterAlias
            };

            var content = new StringContent(
                content: JsonConvert.SerializeObject(createAuthorCommand),
                encoding: Encoding.UTF8,
                mediaType: "application/json");

            var response = await _client.PostAsync("/authors", content);
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateAuthorResult>(stringResponse);

            Assert.NotNull(result);
            Assert.Equal(currentMaxAuthorId + 1, result.Id);
        }
    }
}

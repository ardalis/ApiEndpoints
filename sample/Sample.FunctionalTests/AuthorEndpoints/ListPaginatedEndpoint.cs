using Ardalis.HttpClientTestExtensions;
using Sample.FunctionalTests.Models;
using SampleEndpointApp;
using SampleEndpointApp.DomainModel;
using Xunit;

namespace Sample.FunctionalTests.AuthorEndpoints;

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
    var result = await _client.GetAndDeserialize<IEnumerable<Author>>(Routes.Authors.List(1, 1));

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

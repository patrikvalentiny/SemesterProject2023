using System.Net;

namespace apitests;

public class StatusTest
{
    private HttpClient _httpClient = null!;
    [SetUp]
    public void Setup()
    {
        _httpClient = new HttpClient();
    }
    
    [Test]
    public async Task TestStatus()
    {
        var response = await _httpClient.GetAsync("http://localhost:5000/api/v1/status");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [TearDown]
    public void TearDown()
    {
        _httpClient.Dispose();
    }
}
using System.Net;

namespace apitests;

public class Tests
{
    [Test]
    public async Task Test1()
    {
        var _httpClient = new HttpClient();

        var url = "http://localhost:5000/api/v1/login";

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }

        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}
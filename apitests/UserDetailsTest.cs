

namespace apitests;

public class UserDetailsTest
{
    private HttpClient _httpClient = null!;
    private Faker<UserDetails> _faker = null!;
    [SetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
        Helper.InsertUser1();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());
        
        _faker = new Faker<UserDetails>()
            .RuleFor(u => u.Height, f => f.Random.Int(50, 250))
            .RuleFor(u => u.TargetWeight, f => Math.Round(f.Random.Decimal(50, 200), 2))
            .RuleFor(u => u.TargetDate, f => f.Date.Future().Date.OrNull(f, 0.2f))
            .RuleFor(u => u.LossPerWeek, f => Math.Round(f.Random.Decimal(0.1m, 5.0m), 2).OrNull(f, 0.2f))
            .RuleFor(u => u.Firstname, f => f.Person.FirstName.OrNull(f, 0.2f))
            .RuleFor(u => u.Lastname, f => f.Person.LastName.OrNull(f, 0.2f))
            .RuleFor(u => u.UserId, f => 1);
        
        
    }
    
    [Test]
    public async Task TestAddUserDetails()
    {
        var userDetails = _faker.Generate();
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/v1/profile", userDetails);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        UserDetails? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<UserDetails>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(userDetails);
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}


using System.Net;

namespace apitests;

public class UserDetailsTest
{
    private HttpClient _httpClient = null!;
    private Faker<UserDetailsCommandModel> _faker = null!;
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
        await Helper.InsertUser1();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());
        
        _faker = new Faker<UserDetailsCommandModel>()
            .RuleFor(u => u.Height, f => f.Random.Int(150, 250))
            .RuleFor(u => u.TargetWeight, f => Math.Round(f.Random.Decimal(50, 200), 2))
            .RuleFor(u => u.TargetDate, f => f.Date.Future().Date.OrNull(f, 0.2f))
            .RuleFor(u => u.LossPerWeek, f => Math.Round(f.Random.Decimal(0.1m, 5.0m), 2).OrNull(f, 0.2f))
            .RuleFor(u => u.Firstname, f => f.Person.FirstName.OrNull(f, 0.2f))
            .RuleFor(u => u.Lastname, f => f.Person.LastName.OrNull(f, 0.2f))
            ;
        
        
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
            userDetails.Should().BeEquivalentTo(responseObject, options => options.Excluding(u => u!.UserId));
        }
    }

    public enum TestCases
    {
        Height,
        TargetWeight
    }
    
    [TestCase(TestCases.Height, TestName = "TestMissingParameterTargetWeight")]
    [TestCase(TestCases.TargetWeight, TestName = "TestMissingParameterHeight")]
    public async Task TestMissingParameters(TestCases testCases)
    {
        var userDetails = _faker.Generate();
        
        HttpResponseMessage response;
        try
        {
            object body;
            if (testCases == TestCases.Height)
            {
                body = new {userDetails.TargetWeight};
            }
            else
            {
                body = new {userDetails.Height};
            }
            response = await _httpClient.PostAsJsonAsync("http://localhost:5000/api/v1/profile", body);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    
    [TestCase]
    [TestCase(5000, 100, TestName = "TestInvalidParametersHeightMax")]
    [TestCase(25, 100, TestName = "TestInvalidParametersTargetHeightMin")]
    [TestCase(180, 5000, TestName = "TestInvalidParametersTargetWeightMax")]
    [TestCase(180, 25, TestName = "TestInvalidParametersTargetWeightMin")]
    public async Task TestInvalidParameters(int height = 0, decimal targetWeight = 0)
    {
        var userDetails = _faker.Generate();
        userDetails.Height = height;
        userDetails.TargetWeight = targetWeight;
        
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
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }
    [Test]
    public async Task TestGetUserDetails()
    {
        var userDetails = _faker.Generate();

        const string sql = $@"INSERT INTO weight_tracker.user_details (user_id, firstname, lastname, height_cm, target_weight_kg, target_date, loss_per_week) 
        VALUES (
                1,
                @{nameof(UserDetails.Firstname)},
                @{nameof(UserDetails.Lastname)},
                @{nameof(UserDetails.Height)},
                @{nameof(UserDetails.TargetWeight)},
                @{nameof(UserDetails.TargetDate)},
                @{nameof(UserDetails.LossPerWeek)}
        );";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, userDetails);
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync("http://localhost:5000/api/v1/profile");
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

    [Test]
    public async Task TestUpdateUserDetails()
    {
        var userDetails = _faker.Generate();

        const string sql =
            $@"INSERT INTO weight_tracker.user_details (user_id, firstname, lastname, height_cm, target_weight_kg, target_date, loss_per_week) VALUES (
                1,
                @{nameof(UserDetails.Firstname)},
                @{nameof(UserDetails.Lastname)},
                @{nameof(UserDetails.Height)},
                @{nameof(UserDetails.TargetWeight)},
                @{nameof(UserDetails.TargetDate)},
                @{nameof(UserDetails.LossPerWeek)}
        );";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, userDetails);
        
        
        userDetails = _faker.Generate();
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync("http://localhost:5000/api/v1/profile", userDetails);
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
            userDetails.Should().BeEquivalentTo(responseObject, options => options.Excluding(u => u!.UserId));
        }
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}
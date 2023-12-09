namespace apitests;

public class BmiTests
{
    private HttpClient _httpClient = null!;
    private const string Url = "http://localhost:5000/api/v1/bmi";
    private Faker<WeightInput> _weightFaker = null!;
    [SetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
        Helper.InsertUser1();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());
        
        _weightFaker = new Faker<WeightInput>()
            .RuleFor(u => u.Weight, f => Math.Round(f.Random.Decimal(50, 200), 2))
            .RuleFor(u => u.Date, f => f.Date.Past().Date)
            .RuleFor(u => u.UserId, f => 1);
    }

    [Test]
    public async Task GetLatestTest()
    {
        var weight = _weightFaker.Generate();
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);

        const int height = 180;
        const decimal targetWeight = 80.0m;
        await conn.ExecuteAsync(
            "INSERT INTO weight_tracker.user_details (height_cm, target_weight_kg, user_id) VALUES (@Height, @TargetWeight, @UserId)",
            new { Height = height, TargetWeight = targetWeight, UserId = 1 });

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/latest");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        BmiCommandModel responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<BmiCommandModel>(await response.Content.ReadAsStringAsync())!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            var bmi = decimal.Round(weight.Weight / (height / 100m * height / 100m), 2);
            var expected = new BmiCommandModel
            {
                Bmi = bmi,
                Date = weight.Date.Date,
                Category = bmi switch
                {
                    < 18.5m => "Underweight",
                    < 25m => "Normal",
                    < 30m => "Overweight",
                    _ => "Obese"
                }
            };
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(expected);
        }
    }

    [Test]
    public async Task GetAllBmiTest()
    {
        await using var conn = Helper.OpenConnection();
        var weights = new List<WeightInput>();
        for (int i = 0; i < 50; i++)
        {
            WeightInput weight;
            do
            {
                 weight = _weightFaker.Generate();
            } while (weights.Exists(w => w.Date == weight.Date));
            weights.Add(weight);
            await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);
        }
        
        const int height = 180;
        const decimal targetWeight = 80.0m;
        await conn.ExecuteAsync(
            "INSERT INTO weight_tracker.user_details (height_cm, target_weight_kg, user_id) VALUES (@Height, @TargetWeight, @UserId)",
            new { Height = height, TargetWeight = targetWeight, UserId = 1 });

        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        BmiCommandModel[] responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<BmiCommandModel[]>(await response.Content.ReadAsStringAsync())!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            var expected = weights.Select(w =>
            {
                var bmi = decimal.Round(w.Weight / (height / 100m * height / 100m), 2);
                return new BmiCommandModel
                {
                    Bmi = bmi,
                    Date = w.Date.Date,
                    Category = bmi switch
                    {
                        < 18.5m => "Underweight",
                        < 25m => "Normal",
                        < 30m => "Overweight",
                        _ => "Obese"
                    }
                };
            }); 
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().BeEquivalentTo(expected);
        }   
    }
    
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}
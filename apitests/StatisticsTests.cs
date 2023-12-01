using System.Net;

namespace apitests;

public class StatisticsTests
{
    
    private HttpClient _httpClient = null!;
    private const string Url = "http://localhost:5000/api/v1/statistics";
    
    [SetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
        Helper.InsertUser1();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());
    }

    [TestCase(1)]
    [TestCase(0.1)]
    [TestCase(0.5)]
    [TestCase(0.82)]
    [TestCase(0.41)]
    [TestCase(0.99)]
    [TestCase(0.01)]
    public async Task TestCurrentTrendWithTarget(decimal averageLoss)
    {
        await using var conn = Helper.OpenConnection();
        const int inputSize = 10;
        const int startWeight = 100;
        var startDate = DateTime.Now.AddDays(-inputSize).Date;
        for (int i = 0; i < inputSize; i++)
        {
            var weight = new WeightInput
            {
                Weight = startWeight - (i * averageLoss),
                Date = startDate.AddDays(i),
                UserId = 1
            };
            await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);
        }
        
        const int height = 180;
        const decimal targetWeight = 80.0m;
        const int dateDiff = 10;
        DateTime targetDate = DateTime.Now.AddDays(dateDiff).Date;
        await conn.ExecuteAsync(
            "INSERT INTO weight_tracker.user_details (height_cm, target_weight_kg, user_id, target_date) VALUES (@Height, @TargetWeight, @UserId, @TargetDate)",
            new { Height = height, TargetWeight = targetWeight, UserId = 1, TargetDate = targetDate });
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/currentTrend");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        WeightInput[] responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput[]>(await response.Content.ReadAsStringAsync())!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Length.Should().Be(dateDiff + inputSize + 1);
            responseObject.First().Weight.Should().Be(startWeight);
            responseObject.First().Date.Should().Be(startDate);
            responseObject.Last().Date.Should().Be(targetDate);
            responseObject.Last().Weight.Should().Be(startWeight - (dateDiff + inputSize) * averageLoss);
        }
    }
    
    [TestCase(1)]
    [TestCase(0.1)]
    [TestCase(0.5)]
    [TestCase(0.82)]
    [TestCase(0.41)]
    [TestCase(0.99)]
    [TestCase(0.01)]
    public async Task TestCurrentTrendWithoutTarget(decimal averageLoss)
    {
        await using var conn = Helper.OpenConnection();
        const int inputSize = 10;
        const int startWeight = 100;
        var startDate = DateTime.Now.AddDays(-inputSize).Date;
        for (int i = 0; i < inputSize + 1; i++)
        {
            var weight = new WeightInput
            {
                Weight = startWeight - (i * averageLoss),
                Date = startDate.AddDays(i),
                UserId = 1
            };
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
            response = await _httpClient.GetAsync(Url + "/currentTrend");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        WeightInput[] responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput[]>(await response.Content.ReadAsStringAsync())!;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Length.Should().Be( inputSize + 1);
            responseObject.First().Weight.Should().Be(startWeight);
            responseObject.First().Date.Should().Be(startDate);
            responseObject.Last().Date.Should().Be(DateTime.Now.Date);
            responseObject.Last().Weight.Should().Be(startWeight - (inputSize) * averageLoss);
        }
    }
    
    [Test]
    public async Task TestCurrentTrendNoUserDetails()
    {
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/currentTrend");
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
    public async Task TestCurrentTrendNoWeights()
    {
        await using var conn = Helper.OpenConnection();
        const int height = 180;
        const decimal targetWeight = 80.0m;
        await conn.ExecuteAsync(
            "INSERT INTO weight_tracker.user_details (height_cm, target_weight_kg, user_id) VALUES (@Height, @TargetWeight, @UserId)",
            new { Height = height, TargetWeight = targetWeight, UserId = 1 });
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/currentTrend");
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
    public async Task TestCurrentTotalLoss()
    {
        await using var conn = Helper.OpenConnection();
        const int inputSize = 10;
        const int startWeight = 100;
        var startDate = DateTime.Now.AddDays(-inputSize).Date;
        for (int i = 0; i < inputSize + 1; i++)
        {
            var weight = new WeightInput
            {
                Weight = startWeight - i,
                Date = startDate.AddDays(i),
                UserId = 1
            };
            await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);
        }
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/currentTotalLoss");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        decimal? responseObject;
        try 
        {
            responseObject = JsonConvert.DeserializeObject<decimal?>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().Be(inputSize);
        }
    }
    
    [TestCase]
    [TestCase(0.5)]
    [TestCase(0.82)]
    [TestCase(0.41)]
    public async Task TestAverageLoss(decimal averageLoss = 1)
    {
        await using var conn = Helper.OpenConnection();
        const int inputSize = 10;
        const int startWeight = 100;
        var startDate = DateTime.Now.AddDays(-inputSize).Date;
        for (int i = 0; i < inputSize + 1; i++)
        {
            var weight = new WeightInput
            {
                Weight = startWeight - (i * averageLoss),
                Date = startDate.AddDays(i),
                UserId = 1
            };
            await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);
        }
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/averageLoss");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        decimal? responseObject;
        try 
        {
            responseObject = JsonConvert.DeserializeObject<decimal?>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().Be(averageLoss);
        }
    }

    [TestCase]
    [TestCase(1)]
    public async Task TestDaysIn(int inputSize = 10)
    {
        await using var conn = Helper.OpenConnection();
        const int startWeight = 100;
        const decimal averageLoss = 1;
        var startDate = DateTime.Now.AddDays(-inputSize).Date;
        for (int i = 0; i < inputSize + 1; i++)
        {
            var weight = new WeightInput
            {
                Weight = startWeight - (i * averageLoss),
                Date = startDate.AddDays(i),
                UserId = 1
            };
            await conn.ExecuteAsync("INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)", weight);
        }
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/daysIn");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        decimal? responseObject;
        try 
        {
            responseObject = JsonConvert.DeserializeObject<decimal?>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().Be(inputSize);
        }
    }

    [TestCase]
    [TestCase(10)]
    [TestCase(-10)]
    public async Task TestDaysToTarget(int dateDiff = 0)
    {
        await using var conn = Helper.OpenConnection();
        const int height = 180;
        const decimal targetWeight = 80.0m;
        DateTime targetDate = DateTime.Now.AddDays(dateDiff).Date;
        await conn.ExecuteAsync(
            "INSERT INTO weight_tracker.user_details (height_cm, target_weight_kg, user_id, target_date) VALUES (@Height, @TargetWeight, @UserId, @TargetDate)",
            new { Height = height, TargetWeight = targetWeight, UserId = 1, TargetDate = targetDate });
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.GetAsync(Url + "/daysToTarget");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        int? responseObject;
        try 
        {
            responseObject = JsonConvert.DeserializeObject<int?>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            responseObject.Should().Be(dateDiff);
        }
    }
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}
﻿namespace apitests;

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

    [TestCase]
    [TestCase(0.1m)]
    [TestCase(0.5m)]
    [TestCase(0.82m)]
    [TestCase(0.41m)]
    [TestCase(0.99m)]
    [TestCase(0.01m)]
    public async Task TestCurrentTrendWithTarget(decimal averageLoss = 1m)
    {
        await using var conn = Helper.OpenConnection();
        const int inputSize = 50;
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
    
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
        Helper.InsertUser1();
        _httpClient.Dispose();
    }
}
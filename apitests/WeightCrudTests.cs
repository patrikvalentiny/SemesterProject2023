using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bogus;
using Dapper;
using infrastructure.DataModels;
using Newtonsoft.Json;
using service.Models;

namespace apitests;

public class WeightCrudTests
{
    
    private HttpClient _httpClient;
    private Faker<WeightInputCommandModel> _weightFaker;
    private const string Url = "http://localhost:5000/api/v1/weight";
    [SetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());

        _weightFaker = new Faker<WeightInputCommandModel>()
            .RuleFor(w => w.Weight, f => Math.Round(f.Random.Decimal(50, 200), 2))
            .RuleFor(w => w.Date, f => f.Date.Past().Date);

        const string sql = "INSERT INTO weight_tracker.users (id, username, email) VALUES (1, 'test', 'test@test.test')";
        using var conn = Helper.OpenConnection();
        conn.Execute(sql);
    }

    [Test]
    public async Task TestAddWeight()
    {
        
        var weight = _weightFaker.Generate();
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(Url, weight);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        WeightInput? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            weight.Weight.Should().Be(responseObject!.Weight);
            weight.Date.Date.Should().Be(responseObject.Date);
            responseObject.UserId.Should().Be(Helper.UserId);
        }
    }

    [Test]
    public async Task GetAllWeightsTest()
    {
        var weights = new List<WeightInputCommandModel>();
        const string sql = "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        for (var i = 0; i < 10; i++)
        {
            var weight = _weightFaker.Generate();
            weights.Add(weight);
            await conn.ExecuteAsync(sql, new {weight.Weight, weight.Date, Helper.UserId});
        }
        
        
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
        
        WeightInput[]? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput[]>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            weights.Should().BeEquivalentTo(responseObject!, options => options.Excluding(o => o.UserId));
            weights.Count.Should().Be(responseObject!.Length);
            responseObject.All(w => w.UserId == Helper.UserId).Should().BeTrue();
        }
    }

    [Test]
    public async Task TestUpdateWeight()
    {
        var weight = _weightFaker.Generate();
        
        const string sql = "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, new {weight.Weight, weight.Date, Helper.UserId});
        
        var initWeight = weight.Weight;
        weight.Weight = Math.Round(_weightFaker.Generate().Weight, 2);
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PutAsJsonAsync(Url, weight);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        WeightInput? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            weight.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o!.UserId));
            weight.Weight.Should().NotBe(initWeight);
            Helper.UserId.Should().Be(responseObject!.UserId);
        }
    }

    [Test]
    public async Task GetLatestWeightTest()
    {
        var weights = new List<WeightInputCommandModel>();
        const string sql = "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        for (var i = 0; i < 10; i++)
        {
            var weight = _weightFaker.Generate();
            weights.Add(weight);
            await conn.ExecuteAsync(sql, new {weight.Weight, weight.Date, Helper.UserId});
        }

        weights.Sort((w1, w2) => w1.Date.CompareTo(w2.Date));
        var latestWeight = weights.Last();
        
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
        
        WeightInput? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            latestWeight.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o!.UserId));
            Helper.UserId.Should().Be(responseObject!.UserId);
        }
    }
    
    [Test]
    public async Task DeleteWeightTest()
    {
        var weight = _weightFaker.Generate();
        
        const string sql = "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, new {weight.Weight, weight.Date, Helper.UserId});
        
        HttpResponseMessage response;
        try
        {
            response = await _httpClient.DeleteAsync(Url + $"/{weight.Date:yyyy-MM-dd}");
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        WeightInput? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightInput>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
        
        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            weight.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o!.UserId));
            Helper.UserId.Should().Be(responseObject!.UserId);
        }
    }
    
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}
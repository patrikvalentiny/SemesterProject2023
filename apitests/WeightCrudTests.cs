using api.Dtos;

namespace apitests;

public class WeightCrudTests
{
    private const string Url = "http://localhost:5000/api/v1/weight";

    private HttpClient _httpClient = null!;
    private Faker<WeightInputCommandModel> _weightFaker = null!;

    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
        await Helper.InsertUser1();

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Helper.GetToken());

        _weightFaker = new Faker<WeightInputCommandModel>()
            .RuleFor(w => w.Weight, f => Math.Round(f.Random.Decimal(50, 200), 2))
            .RuleFor(w => w.Date, f => f.Date.Past().Date);
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
        }
    }

    [Test]
    public async Task TestAddWeightToSameDay()
    {
        var weight = _weightFaker.Generate();

        const string sql =
            "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, new { weight.Weight, weight.Date, Helper.UserId });

        weight.Weight = _weightFaker.Generate().Weight;

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
        }
    }

    [Test]
    public async Task GetAllWeightsTest()
    {
        var weights = new List<WeightInputCommandModel>();
        const string sql =
            "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        for (var i = 0; i < 10; i++)
        {
            WeightInputCommandModel weight;
            do
            {
                weight = _weightFaker.Generate();
            } while (weights.Exists(w => w.Date == weight.Date));

            weights.Add(weight);
            await conn.ExecuteAsync(sql, new { weight.Weight, weight.Date, Helper.UserId });
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
            weights.Should().BeEquivalentTo(responseObject!,
                options => options.Excluding(o => o.UserId).Excluding(o => o.Difference));
            weights.Count.Should().Be(responseObject!.Length);
            responseObject.Should().BeInAscendingOrder(w => w.Date);
        }
    }

    [Test]
    public async Task TestUpdateWeight()
    {
        var weight = _weightFaker.Generate();

        const string sql =
            "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, new { weight.Weight, weight.Date, Helper.UserId });

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
            weight.Should().BeEquivalentTo(responseObject,
                options => options.Excluding(o => o!.UserId).Excluding(o => o!.Difference));
            weight.Weight.Should().NotBe(initWeight);
        }
    }

    [Test]
    public async Task DeleteWeightTest()
    {
        var weight = _weightFaker.Generate();

        const string sql =
            "INSERT INTO weight_tracker.weights (weight, date, user_id) VALUES (@Weight, @Date, @UserId)";
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync(sql, new { weight.Weight, weight.Date, Helper.UserId });

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
            weight.Should().BeEquivalentTo(responseObject,
                options => options.Excluding(o => o!.UserId).Excluding(o => o!.Difference));
        }
    }

    [Test]
    public async Task TestMultipleWeightInput()
    {
        var weights = new List<WeightInputCommandModel>();
        for (var i = 0; i < 10; i++)
        {
            WeightInputCommandModel weight;
            do
            {
                weight = _weightFaker.Generate();
            } while (weights.Exists(w => w.Date == weight.Date));

            weights.Add(weight);
        }


        HttpResponseMessage response;
        try
        {
            response = await _httpClient.PostAsJsonAsync(Url + "/multiple", weights);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        WeightDto[]? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<WeightDto[]>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            weights.Should().BeEquivalentTo(responseObject!,
                options => options.Excluding(o => o.Difference));
            weights.Count.Should().Be(responseObject!.Length);
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        await Helper.TriggerRebuild();
        _httpClient.Dispose();
    }
}
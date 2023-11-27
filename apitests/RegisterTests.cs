using System.Net;
using System.Net.Http.Json;
using Bogus;
using Dapper;
using Newtonsoft.Json;
using service.Models;

namespace apitests;

public class RegisterTests
{
    [SetUp]
    public void Setup()
    {
        Helper.TriggerRebuild();
    }

    [Test]
    public async Task TestAllFieldsRegister()
    {
        var httpClient = new HttpClient();
        var userFaker = new Faker<RegisterCommandModel>()
            .RuleFor(u => u.Username, f => f.Person.UserName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Email, f => f.Person.Email);


        const string url = "http://localhost:5000/api/v1/account/register";
        var user = userFaker.Generate();

        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsJsonAsync(url, user);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        TokenUserModel? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<TokenUserModel>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }


        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            user.Should().BeEquivalentTo(responseObject!.User, options => options.Excluding(o => o.Id));
        }
    }


    [Test]
    public async Task TestSameUserRegister()
    {
        var httpClient = new HttpClient();
        var userFaker = new Faker<RegisterCommandModel>()
            .RuleFor(u => u.Username, f => f.Person.UserName)
            .RuleFor(u => u.Password, f => f.Internet.Password())
            .RuleFor(u => u.Email, f => f.Person.Email);

        var user = userFaker.Generate();
        await using var conn = Helper.OpenConnection();
        await conn.ExecuteAsync("INSERT INTO weight_tracker.users (username, email) VALUES (@Username, @Email)", user);


        const string url = "http://localhost:5000/api/v1/account/register";


        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsJsonAsync(url, user);
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }


        using (new AssertionScope())
        {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }
    }

    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
    }
}
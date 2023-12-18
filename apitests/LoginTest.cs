using System.Net;
using System.Security.Cryptography;
using System.Text;
using apitests.Helpers;
using Konscious.Security.Cryptography;

namespace apitests;

public class LoginTest
{
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
    }

    [Test]
    public async Task ValidLoginUserNameTest()
    {
        var httpClient = new HttpClient();
        var userFaker = new Faker<LoginCommandModel>()
            .RuleFor(u => u.Username, f => f.Person.UserName)
            .RuleFor(u => u.Password, f => f.Internet.Password());
        var user = userFaker.Generate();

        await using var conn = Helper.OpenConnection();
        var sql = "insert into weight_tracker.users (username) values (@Username) returning id";
        var id = await conn.QueryFirstAsync<int>(sql, user);
        sql =
            "insert into weight_tracker.passwords (user_id, password_hash, salt, algorithm) values (@Id, @Password, @Salt, 'argon2id')";
        using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(user.Password));
        hashAlgo.Salt = RandomNumberGenerator.GetBytes(128);
        hashAlgo.MemorySize = 12288;
        hashAlgo.Iterations = 3;
        hashAlgo.DegreeOfParallelism = 1;
        await conn.ExecuteAsync(sql, new
        {
            Id = id,
            Password = Convert.ToBase64String(await hashAlgo.GetBytesAsync(256)),
            Salt = Convert.ToBase64String(hashAlgo.Salt)
        });

        const string url = "http://localhost:5000/api/v1/account/login";


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
            response.Headers.Should().ContainKey("Authorization");
            responseObject.Should().NotBeNull();
            responseObject!.User.Username.Should().Be(user.Username);
        }
    }

    [Test]
    public async Task ValidLoginEmail()
    {
        var httpClient = new HttpClient();
        var userFaker = new Faker<LoginCommandModel>()
            .RuleFor(u => u.Username, f => f.Person.Email)
            .RuleFor(u => u.Password, f => f.Internet.Password());
        var user = userFaker.Generate();

        await using (var conn = Helper.OpenConnection())
        {
            var sql = "insert into weight_tracker.users (username, email) values ('test', @Username) returning id";
            var id = await conn.QueryFirstAsync<int>(sql, user);
            sql =
                "insert into weight_tracker.passwords (user_id, password_hash, salt, algorithm) values (@Id, @Password, @Salt, 'argon2id')";
            using var hashAlgo = new Argon2id(Encoding.UTF8.GetBytes(user.Password));
            hashAlgo.Salt = RandomNumberGenerator.GetBytes(128);
            hashAlgo.MemorySize = 12288;
            hashAlgo.Iterations = 3;
            hashAlgo.DegreeOfParallelism = 1;
            await conn.ExecuteAsync(sql, new
            {
                Id = id,
                Password = Convert.ToBase64String(await hashAlgo.GetBytesAsync(256)),
                Salt = Convert.ToBase64String(hashAlgo.Salt)
            });
        }

        const string url = "http://localhost:5000/api/v1/account/login";


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
            response.Headers.Should().ContainKey("Authorization");
            responseObject.Should().NotBeNull();
            responseObject!.User.Email.Should().Be(user.Username);
        }
    }

    [TestCase("", "")]
    public void DataValidationViolation(string username, string password)
    {
        var httpClient = new HttpClient();
        LoginCommandModel user = new()
        {
            Username = username,
            Password = password
        };

        const string url = "http://localhost:5000/api/v1/account/login";

        HttpResponseMessage response;
        try
        {
            response = httpClient.PostAsJsonAsync(url, user).Result;
            TestContext.WriteLine("THE FULL BODY RESPONSE: " + response.Content.ReadAsStringAsync().Result);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeFalse();
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            response.Headers.Should().NotContainKey("Authorization");
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        await Helper.TriggerRebuild();
    }
}
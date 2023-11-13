using System.Net.Http.Json;
using Bogus;
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
            .RuleFor(u => u.Email, f => f.Person.Email)
            .RuleFor(u => u.Firstname, f => f.Person.FirstName)
            .RuleFor(u => u.Lastname, f => f.Person.LastName);
            
            
        
        var url = "http://localhost:5000/api/v1/account/register";
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
    
    [TearDown]
    public void TearDown()
    {
        Helper.TriggerRebuild();
    }
}
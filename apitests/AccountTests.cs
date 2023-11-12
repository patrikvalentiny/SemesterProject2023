using System.Data.Common;
using System.Net.Http.Json;
using Bogus;
using infrastructure.DataModels;
using Newtonsoft.Json;
using service.Models;

namespace apitests;

public class AccountTests
{
    
    [Test]
    public async Task TestAllFieldsRegister()
    {
        Helper.TriggerRebuild();   
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

        User? responseObject;
        try
        {
            responseObject = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
            

        using (new AssertionScope())
        {
            response.IsSuccessStatusCode.Should().BeTrue();
            user.Should().BeEquivalentTo(responseObject, options => options.Excluding(o => o.Id));
        }
    }
}
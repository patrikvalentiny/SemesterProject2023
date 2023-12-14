using apitests;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E2ETests;


[TestFixture]
public class AuthTests : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
        await Helper.InsertUser1();
        
        await Page.GotoAsync("http://localhost:4200");

    }

    [Test]
    public async Task TestNotLoggedInUserCantAccessHome()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));

        await Page.GotoAsync("http://localhost:4200/home");

        await Expect(Page).ToHaveURLAsync("http://localhost:4200/login");
    }
    
    [Test]
    public async Task TestLoggedInUserCantAccessLogin()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));
        
        var usernameInput = Page.GetByTestId("usernameInput");
        await usernameInput.IsVisibleAsync();
        await usernameInput.FillAsync(Helper.User1.Username);
        var passwordInput = Page.GetByTestId("passwordInput");
        await passwordInput.IsVisibleAsync();
        await passwordInput.FillAsync(Helper.UserPassword);
        var loginButton = Page.GetByTestId("loginButton");
        await loginButton.IsEnabledAsync();
        await loginButton.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
        
        await Page.GotoAsync("http://localhost:4200/login");
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await Helper.TriggerRebuild();
        await Page.CloseAsync();
    }
}
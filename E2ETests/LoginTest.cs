using apitests;

namespace E2ETests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class LoginTest :PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();

    }

    [Test]
    public async Task UserCanLogin()
    {
        await Helper.InsertUser1();

        await Page.GotoAsync("http://localhost:4200");

        await Page.WaitForURLAsync(new Regex(".*login"));
        var loginButton = Page.GetByTestId("loginButton");
        await loginButton.IsVisibleAsync();
        await loginButton.IsDisabledAsync();
        
        var usernameInput = Page.GetByTestId("usernameInput");
        await usernameInput.IsVisibleAsync();
        await usernameInput.FillAsync(Helper.User1.Username);
        var passwordInput = Page.GetByTestId("passwordInput");
        await passwordInput.IsVisibleAsync();
        await passwordInput.FillAsync(Helper.UserPassword);
        await loginButton.IsEnabledAsync();
        await loginButton.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Helper.TriggerRebuild();
    }
}
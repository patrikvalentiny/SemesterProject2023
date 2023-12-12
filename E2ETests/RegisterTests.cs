using apitests;

namespace E2ETests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RegisterTests : PageTest
{
    
    [SetUp]
    public async Task Setup()
    {
        await Page.GotoAsync("http://localhost:4200");
        Helper.TriggerRebuild();
    }
    [Test]
    public async Task UserCanNavigateFromLoginToRegister()
    {
        // Expects the URL to contain intro.
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));

        var registerLink = Page.GetByTestId("registerLink");
        await registerLink.IsVisibleAsync();
        await registerLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*register"));
    }
    
    [Test]
    public async Task UserCanNavigateFromRegisterToLogin()
    {
        await Page.GotoAsync("http://localhost:4200/register");
        
        var loginLink = Page.GetByTestId("loginLink");
        await loginLink.IsVisibleAsync();
        await loginLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));
    }
    
    [Test]
    public async Task UserCanRegister()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));

        var registerLink = Page.GetByTestId("registerLink");
        await registerLink.IsVisibleAsync();
        await registerLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*register"));
        
        var registerButton = Page.GetByTestId("registerButton");
        await registerButton.IsVisibleAsync();
        await registerButton.IsDisabledAsync();
        
        var emailInput = Page.GetByTestId("emailInput");
        await emailInput.IsVisibleAsync();
        await emailInput.FillAsync("test@test.test");
        var usernameInput = Page.GetByTestId("usernameInput");
        await usernameInput.IsVisibleAsync();
        await usernameInput.FillAsync("test");
        var passwordInput = Page.GetByTestId("passwordInput");
        await passwordInput.IsVisibleAsync();
        await passwordInput.FillAsync("test");
        var confirmPasswordInput = Page.GetByTestId("confirmPasswordInput");
        await confirmPasswordInput.IsVisibleAsync();
        await confirmPasswordInput.FillAsync("test");
        
        await registerButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*onboarding"));
    }
}
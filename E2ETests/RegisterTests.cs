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

    [TestCase("invalidEmail", "test", "test", "test", TestName = "InvalidEmail")]
    [TestCase("valid@email.dk", "test", "ts", "ts", TestName = "InvalidPasswordLength")]
    public async Task UserCanNotRegisterWithInvalidData(string email, string username, string password, string confirmPassword)
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
        await emailInput.FillAsync(email);
        var usernameInput = Page.GetByTestId("usernameInput");
        await usernameInput.IsVisibleAsync();
        await usernameInput.FillAsync(username);
        var passwordInput = Page.GetByTestId("passwordInput");
        await passwordInput.IsVisibleAsync();
        await passwordInput.FillAsync(password);
        var confirmPasswordInput = Page.GetByTestId("confirmPasswordInput");
        await confirmPasswordInput.IsVisibleAsync();
        await confirmPasswordInput.FillAsync(confirmPassword);
        
        await registerButton.IsDisabledAsync();
    }

    [Test]
    public async Task UserCanRegisterAndOnBoard()
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

        await registerButton.IsEnabledAsync();
        await registerButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*onboarding/weight"));
        
        
        var weightInput = Page.GetByTestId("weightInput");
        await weightInput.IsVisibleAsync();
        await weightInput.IsEditableAsync();
        await weightInput.IsEnabledAsync();
        await weightInput.FillAsync("100");
        var saveButton = Page.GetByTestId("saveButton");
        await saveButton.IsVisibleAsync();
        await saveButton.IsEnabledAsync();
        await saveButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*onboarding/profile"));
        
        var firstnameInput = Page.GetByTestId("firstnameInput");
        await firstnameInput.IsVisibleAsync();
        await firstnameInput.IsEditableAsync();
        await firstnameInput.IsEnabledAsync();
        await firstnameInput.FillAsync("test");
        var lastnameInput = Page.GetByTestId("lastnameInput");
        await lastnameInput.IsVisibleAsync();
        await lastnameInput.IsEditableAsync();
        await lastnameInput.IsEnabledAsync();
        await lastnameInput.FillAsync("test");
        var heightInput = Page.GetByTestId("heightInput");
        await heightInput.IsVisibleAsync();
        await heightInput.IsEditableAsync();
        await heightInput.IsEnabledAsync();
        await heightInput.FillAsync("180");
        var goalWeightInput = Page.GetByTestId("goalWeightInput");
        await goalWeightInput.IsVisibleAsync();
        await goalWeightInput.IsEditableAsync();
        await goalWeightInput.IsEnabledAsync();
        await goalWeightInput.FillAsync("90");
        var targetDateInput = Page.GetByTestId("targetDateInput");
        await targetDateInput.IsVisibleAsync();
        await targetDateInput.IsEditableAsync();
        await targetDateInput.IsEnabledAsync();
        await targetDateInput.FillAsync("2024-12-31");
        var createProfileButton = Page.GetByTestId("createProfileButton");
        await createProfileButton.IsVisibleAsync();
        await createProfileButton.IsEnabledAsync();
        await createProfileButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));
        
    }

    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        Helper.TriggerRebuild();
    }
}
using apitests;

namespace E2ETests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class RegisterTests : PageTest
{
    
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
    }
    [Test]
    public async Task UserCanNavigateFromLoginToRegister()
    {
        await Page.GotoAsync("http://localhost:4200");

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
        await Page.GotoAsync("http://localhost:4200");

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
        await Page.GotoAsync("http://localhost:4200");

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
    public async Task UserPasswordsMustMatch()
    {
        await Page.GotoAsync("http://localhost:4200");

        await Page.GotoAsync("http://localhost:4200/register");
        
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
        await confirmPasswordInput.FillAsync("notTest");
        
        await registerButton.ClickAsync();
        
        await Expect(confirmPasswordInput).ToHaveClassAsync(new Regex(".*input-error"));
        
        await Expect(Page).Not.ToHaveURLAsync(new Regex(".*onboarding"));
    }

    [Test]
    public async Task UserCanRegisterAndOnBoard()
    {
        await Page.GotoAsync("http://localhost:4200");

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
    
    [Test]
    public async Task UserCanRegisterAndOnBoardAndLogout()
    {
        await Page.GotoAsync("http://localhost:4200");

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

        var logoutButton = Page.GetByTestId("logoutButton");
        await logoutButton.IsVisibleAsync();
        await logoutButton.IsEnabledAsync();
        await logoutButton.ClickAsync();

        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));
    }

    
    [Test]
    public async Task UserCanRegisterAndOnBoardAndUserProfileExists()
    {
        await Page.GotoAsync("http://localhost:4200");
        
        const string userFirstname = "Test";
        const string userLastname = "Test";
        const string userHeight = "180";
        const string userWeight = "80";
        const string userTargetWeight = "70";
        const string targetDate = "2024-12-31";
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
        await weightInput.FillAsync(userWeight);
        var saveButton = Page.GetByTestId("saveButton");
        await saveButton.IsVisibleAsync();
        await saveButton.IsEnabledAsync();
        await saveButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*onboarding/profile"));
        
        var firstnameInput = Page.GetByTestId("firstnameInput");
        await firstnameInput.IsVisibleAsync();
        await firstnameInput.IsEditableAsync();
        await firstnameInput.IsEnabledAsync();
        await firstnameInput.FillAsync(userFirstname);
        var lastnameInput = Page.GetByTestId("lastnameInput");
        await lastnameInput.IsVisibleAsync();
        await lastnameInput.IsEditableAsync();
        await lastnameInput.IsEnabledAsync();
        await lastnameInput.FillAsync(userLastname);
        var heightInput = Page.GetByTestId("heightInput");
        await heightInput.IsVisibleAsync();
        await heightInput.IsEditableAsync();
        await heightInput.IsEnabledAsync();
        await heightInput.FillAsync(userHeight);
        var goalWeightInput = Page.GetByTestId("goalWeightInput");
        await goalWeightInput.IsVisibleAsync();
        await goalWeightInput.IsEditableAsync();
        await goalWeightInput.IsEnabledAsync();
        await goalWeightInput.FillAsync(userTargetWeight);
        var targetDateInput = Page.GetByTestId("targetDateInput");
        await targetDateInput.IsVisibleAsync();
        await targetDateInput.IsEditableAsync();
        await targetDateInput.IsEnabledAsync();
        await targetDateInput.FillAsync(targetDate);
        var createProfileButton = Page.GetByTestId("createProfileButton");
        await createProfileButton.IsVisibleAsync();
        await createProfileButton.IsEnabledAsync();
        await createProfileButton.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*home"));

        var profileIcon = Page.GetByTestId("profileIcon");
        await profileIcon.IsVisibleAsync();
        await profileIcon.IsEnabledAsync();
        await profileIcon.ClickAsync();
        
        await Expect(Page).ToHaveURLAsync(new Regex(".*profile"));

        firstnameInput = Page.GetByTestId("firstnameInput");
        await firstnameInput.IsVisibleAsync();
        await firstnameInput.IsEditableAsync();
        await firstnameInput.IsEnabledAsync();
        await Expect(firstnameInput).ToHaveValueAsync(userFirstname);
        lastnameInput = Page.GetByTestId("lastnameInput");
        await lastnameInput.IsVisibleAsync();
        await lastnameInput.IsEditableAsync();
        await lastnameInput.IsEnabledAsync();
        await Expect(lastnameInput).ToHaveValueAsync(userLastname);
        heightInput = Page.GetByTestId("heightInput");
        await heightInput.IsVisibleAsync();
        await heightInput.IsEditableAsync();
        await heightInput.IsEnabledAsync();
        await Expect(heightInput).ToHaveValueAsync(userHeight);
        goalWeightInput = Page.GetByTestId("goalWeightInput");
        await goalWeightInput.IsVisibleAsync();
        await goalWeightInput.IsEditableAsync();
        await goalWeightInput.IsEnabledAsync();
        await Expect(goalWeightInput).ToHaveValueAsync(userTargetWeight);
        targetDateInput = Page.GetByTestId("targetDateInput");
        await targetDateInput.IsVisibleAsync();
        await targetDateInput.IsEditableAsync();
        await targetDateInput.IsEnabledAsync();
        await Expect(targetDateInput).ToHaveValueAsync(targetDate);
        
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await Helper.TriggerRebuild();
        await Page.CloseAsync();
    }
}
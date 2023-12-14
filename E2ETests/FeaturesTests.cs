using apitests;

namespace E2ETests;

[TestFixture]
public class FeaturesTests : PageTest
{
    [SetUp]
    public async Task Setup()
    {
        await Helper.TriggerRebuild();
        await Helper.InsertUser1();

        await Page.GotoAsync("http://localhost:4200");

    }


    [Test]
    public async Task TestDataCopyPaste()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));
        
        // login
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
        
        //login end
        
        var pasteDataLink = Page.GetByTestId("pasteDataLink");
        await pasteDataLink.IsVisibleAsync();
        await pasteDataLink.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*pasteData"));
        
        var dataInputArea = Page.GetByTestId("dataInputArea");
        var uploadDataButton = Page.GetByTestId("uploadDataButton");
        await uploadDataButton.IsVisibleAsync();
        await uploadDataButton.IsDisabledAsync();
        
        await dataInputArea.IsVisibleAsync();
        await dataInputArea.FillAsync(
            "2023/12/06\t103\n2023/12/07\t103.1\n2023/12/08\t103.2\n2023/12/09\t102.7\n2023/12/10\t103.5\n2023/12/11\t103.2\n2023/12/12\t103.2\n2023/12/13\t103.7\n2023/12/14\t102.8");
        var parseButton = Page.GetByTestId("parseButton");
        await parseButton.IsVisibleAsync();
        await parseButton.IsEnabledAsync();
        await parseButton.ClickAsync();
        
        var tableRow = Page.Locator("tbody[data-testid='tableBody'] >  tr:nth-of-type(1)");
        await tableRow.IsVisibleAsync();
        var tableRowHeading = Page.Locator("tbody[data-testid='tableBody'] >  tr:nth-of-type(1) > th");
        await tableRowHeading.IsVisibleAsync();
        await Expect(tableRowHeading).ToHaveTextAsync("2023-12-06");
        var tableRowWeight = Page.Locator("tbody[data-testid='tableBody'] >  tr:nth-of-type(1) > td");
        await tableRowWeight.IsVisibleAsync();
        await Expect(tableRowWeight).ToHaveTextAsync("103kg");
        await uploadDataButton.IsEnabledAsync();
        await uploadDataButton.ClickAsync();
        await Expect(Page).ToHaveURLAsync(new Regex(".*/home"));
    }
    
    [TearDown]
    public async Task TearDown()
    {
        await Page.CloseAsync();
        await Helper.TriggerRebuild();
    }
}
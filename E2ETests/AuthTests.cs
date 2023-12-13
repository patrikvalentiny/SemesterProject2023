using apitests;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace E2ETests;

[Parallelizable(ParallelScope.Self)]
[TestFixture]
public class AuthTests : PageTest
{
    [SetUp]
    public async Task SetUp()
    {
        await Page.GotoAsync("http://localhost:4200");
        Helper.TriggerRebuild();
        await Helper.InsertUser1();
        
    }

    [Test]
    public async Task TestNotLoggedInUserCantAccessHome()
    {
        await Expect(Page).ToHaveURLAsync(new Regex(".*login"));

        await Page.GotoAsync("http://localhost:4200/home");

        await Expect(Page).ToHaveURLAsync("http://localhost:4200/login");
    }
}
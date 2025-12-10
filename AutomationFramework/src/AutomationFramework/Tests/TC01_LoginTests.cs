using AutomationFramework.Core;
using AutomationFramework.Data.Models;
using NUnit.Framework;

namespace AutomationFramework.Tests
{
    [TestFixture]
    [Category("TC01")]
    public class TC01_LoginTests : BaseTest
    {
        [Test]
        public void Validate_Login_Functionality()
        {
            Assert.Multiple(() =>
            {
                Assert.NotNull(LoginPage, "LoginPage should be initialized.");
                Assert.NotNull(DataProvider, "DataProvider should be initialized.");
            });

            LoginData data = DataProvider!.GetLoginData();

            // Preconditions: User is on the Login page (BaseTest navigates to BaseUrl)
            LoginPage!.EnterUsername(data.Username);
            LoginPage.EnterPassword(data.Password);
            LoginPage.ClickLogin();

            // Verify successful navigation to Dashboard and displayed username
            bool isDashboard = LoginPage.IsDashboardDisplayed(data.DisplayName);
            Assert.IsTrue(isDashboard, "User should be logged in and Dashboard is displayed.");
        }
    }
}
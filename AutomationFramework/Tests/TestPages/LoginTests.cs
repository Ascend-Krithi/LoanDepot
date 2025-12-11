using Microsoft.VisualStudio.TestTools.UnitTesting;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.Encryption;
using AutomationFramework.Core.Configuration;
using System;

namespace AutomationFramework.Tests.TestPages
{
    [TestClass]
    public class LoginTests : BaseTest
    {
        [TestMethod]
        public void ValidLoginTest()
        {
            var loginPage = new LoginPage(Driver);
            var config = ConfigManager.Settings;
            var username = EncryptionManager.Decrypt(config.EncryptedUsername);
            var password = EncryptionManager.Decrypt(config.EncryptedPassword);

            var stepStart = DateTime.Now;
            loginPage.EnterUsername(username);
            Report.AddStep("Enter Username", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            loginPage.EnterPassword(password);
            Report.AddStep("Enter Password", DateTime.Now - stepStart);

            stepStart = DateTime.Now;
            loginPage.ClickLogin();
            Report.AddStep("Click Login", DateTime.Now - stepStart);

            // Add assertion for successful login, e.g., dashboard loaded
        }
    }
}
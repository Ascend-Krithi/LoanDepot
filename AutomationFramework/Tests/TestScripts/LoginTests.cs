using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Encryption;
using AutomationFramework.Core.Utilities;
using AutomationFramework.Tests.TestPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;

namespace AutomationFramework.Tests.TestScripts
{
    [TestClass]
    public class LoginTests : BaseTest
    {
        private LoginTestPage _pages;

        [TestInitialize]
        public void Setup()
        {
            base.TestInitialize();
            _pages = new LoginTestPage(Driver);
        }

        [TestMethod]
        public void ValidLoginTest()
        {
            var config = ConfigManager.Settings;
            var username = EncryptionManager.Decrypt(config.EncryptedUsername);
            var password = EncryptionManager.Decrypt(config.EncryptedPassword);

            var sw = Stopwatch.StartNew();
            _pages.LoginPage.EnterUsername(username);
            HtmlReportManager.LogStep("Enter Username", sw.Elapsed); sw.Restart();

            _pages.LoginPage.EnterPassword(password);
            HtmlReportManager.LogStep("Enter Password", sw.Elapsed); sw.Restart();

            _pages.LoginPage.ClickLogin();
            HtmlReportManager.LogStep("Click Login", sw.Elapsed); sw.Restart();

            _pages.DashboardPage.DismissChatPopupIfPresent();
            HtmlReportManager.LogStep("Dismiss Chat Popup", sw.Elapsed); sw.Restart();

            var welcome = _pages.DashboardPage.GetWelcomeMessage();
            HtmlReportManager.LogStep("Get Welcome Message", sw.Elapsed); sw.Restart();

            Assert.IsTrue(welcome.Contains("Welcome"), "Welcome message not found.");
        }

        [TestMethod]
        public void InvalidLoginTest()
        {
            var sw = Stopwatch.StartNew();
            _pages.LoginPage.EnterUsername("invalid");
            HtmlReportManager.LogStep("Enter Invalid Username", sw.Elapsed); sw.Restart();

            _pages.LoginPage.EnterPassword("invalid");
            HtmlReportManager.LogStep("Enter Invalid Password", sw.Elapsed); sw.Restart();

            _pages.LoginPage.ClickLogin();
            HtmlReportManager.LogStep("Click Login", sw.Elapsed); sw.Restart();

            var error = _pages.LoginPage.GetErrorMessage();
            HtmlReportManager.LogStep("Get Error Message", sw.Elapsed); sw.Restart();

            Assert.IsTrue(error.Contains("Invalid"), "Error message not shown for invalid login.");
        }
    }
}
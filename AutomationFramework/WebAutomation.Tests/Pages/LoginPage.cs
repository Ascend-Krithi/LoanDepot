using OpenQA.Selenium;
using WebAutomation.Core.Configuration;
using WebAutomation.Core.Security;
using WebAutomation.Core.Utilities;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public void NavigateToLogin()
        {
            _driver.Navigate().GoToUrl(ConfigManager.Settings.BaseUrl);
        }

        public bool IsPageReady()
        {
            return _wait.UntilPresent(By.CssSelector("form"), 10);
        }

        public void Login()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            _wait.UntilVisible(By.Id("email")).SendKeys(username);
            _wait.UntilVisible(By.Id("password")).SendKeys(password);
            _wait.UntilClickable(By.CssSelector("button[type='submit']")).Click();
        }
    }
}
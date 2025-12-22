using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;
using WebAutomation.Core.Locators;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators/Locators.txt");
        }

        public void LoginWithDefaultCredentials()
        {
            var (username, password) = CredentialProvider.GetDefaultCredentials();
            Driver.FindElement(_locators.GetBy("Login.Username")).SendKeys(username);
            Driver.FindElement(_locators.GetBy("Login.Password")).SendKeys(password);
            Driver.FindElement(_locators.GetBy("Login.Submit.Button")).Click();
        }
    }
}
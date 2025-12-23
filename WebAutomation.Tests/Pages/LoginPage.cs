using OpenQA.Selenium;
using WebAutomation.Core.Locators;
using WebAutomation.Core.Pages;
using WebAutomation.Core.Security;

namespace WebAutomation.Tests.Pages
{
    public class LoginPage : BasePage
    {
        private readonly LocatorRepository _locators;

        public LoginPage(IWebDriver driver) : base(driver)
        {
            _locators = new LocatorRepository("Locators.json");
        }

        public void WaitForPageReady()
        {
            Wait.UntilVisible(_locators.GetBy("Login.PageReady"));
        }

        public void Login(string username, string password)
        {
            Wait.UntilVisible(_locators.GetBy("Login.Username")).Clear();
            Driver.FindElement(_locators.GetBy("Login.Username")).SendKeys(username);

            Wait.UntilVisible(_locators.GetBy("Login.Password")).Clear();
            Driver.FindElement(_locators.GetBy("Login.Password")).SendKeys(password);

            Driver.FindElement(_locators.GetBy("Login.Submit.Button")).Click();
        }
    }
}
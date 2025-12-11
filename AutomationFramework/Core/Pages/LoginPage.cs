using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage
    {
        private readonly SelfHealingWebDriver _driver;

        public LoginPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void EnterUsername(string username)
        {
            var input = _driver.FindElementByKey(LoginPageLocators.UsernameInputKey);
            input.Clear();
            input.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var input = _driver.FindElementByKey(LoginPageLocators.PasswordInputKey);
            input.Clear();
            input.SendKeys(password);
        }

        public void ClickLogin()
        {
            _driver.FindElementByKey(LoginPageLocators.LoginButtonKey).Click();
        }
    }
}
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
            var element = _driver.FindElementByKey(LoginPageLocators.UsernameInputKey);
            element.Clear();
            element.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var element = _driver.FindElementByKey(LoginPageLocators.PasswordInputKey);
            element.Clear();
            element.SendKeys(password);
        }

        public void ClickLogin()
        {
            var element = _driver.FindElementByKey(LoginPageLocators.LoginButtonKey);
            element.Click();
        }
    }
}
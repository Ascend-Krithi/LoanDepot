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
            var el = _driver.FindElementByKey(LoginPageLocators.UsernameInputKey);
            el.Clear();
            el.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var el = _driver.FindElementByKey(LoginPageLocators.PasswordInputKey);
            el.Clear();
            el.SendKeys(password);
        }

        public void ClickLogin()
        {
            _driver.FindElementByKey(LoginPageLocators.LoginButtonKey).Click();
        }
    }
}
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;

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
            var element = _driver.FindElementByKey(nameof(LoginPageLocators.UsernameInputLocator));
            element.Clear();
            element.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var element = _driver.FindElementByKey(nameof(LoginPageLocators.PasswordInputLocator));
            element.Clear();
            element.SendKeys(password);
        }

        public void ClickLogin()
        {
            var element = _driver.FindElementByKey(nameof(LoginPageLocators.LoginButtonLocator));
            element.Click();
        }
    }
}
using AutomationFramework.Core.SelfHealing;

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
            var element = _driver.FindElementByKey(Locators.LoginPageLocators.UsernameKey);
            element.Clear();
            element.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var element = _driver.FindElementByKey(Locators.LoginPageLocators.PasswordKey);
            element.Clear();
            element.SendKeys(password);
        }

        public void ClickLogin()
        {
            var element = _driver.FindElementByKey(Locators.LoginPageLocators.LoginButtonKey);
            element.Click();
        }
    }
}
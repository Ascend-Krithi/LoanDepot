using System;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage
    {
        private readonly IWebDriver _driver;
        private readonly SelfHealingWebDriver _sh;

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
            _sh = new SelfHealingWebDriver(driver);
        }

        public void Navigate()
        {
            _driver.Navigate().GoToUrl(ConfigManager.BaseUrl);
        }

        public void EnterUsername(string username)
        {
            _sh.Type(username, LoginPageLocators.UsernameById, LoginPageLocators.UsernameByXpath, LoginPageLocators.UsernameByCss);
        }

        public void EnterPassword(string password)
        {
            _sh.Type(password, LoginPageLocators.PasswordById, LoginPageLocators.PasswordByXpath, LoginPageLocators.PasswordByCss);
        }

        public void ClickLogin()
        {
            _sh.Click(null, LoginPageLocators.LoginButtonByXpath, LoginPageLocators.LoginButtonByCss);
        }
    }
}

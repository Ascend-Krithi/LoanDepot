using System;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using AutomationFramework.Core.Utilities;

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
            var el = _driver.FindElementWithFallback(LoginPageLocators.Username);
            el.Clear();
            el.SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            var el = _driver.FindElementWithFallback(LoginPageLocators.Password);
            el.Clear();
            el.SendKeys(password);
        }

        public void ClickLogin()
        {
            _driver.FindElementWithFallback(LoginPageLocators.LoginButton).Click();
        }

        public void Login(string username, string password)
        {
            EnterUsername(username);
            EnterPassword(password);
            ClickLogin();
        }
    }
}
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class LoginPage : BasePage
    {
        public LoginPage(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement PageReady() => FindElement("Login.PageReady", By.CssSelector("form"));
        public IWebElement Username() => FindElement("Login.Username", By.Id("email"));
        public IWebElement Password() => FindElement("Login.Password", By.Id("password"));
        public IWebElement Submit() => FindElement("Login.Submit", By.CssSelector("button[type='submit']"));
    }
}
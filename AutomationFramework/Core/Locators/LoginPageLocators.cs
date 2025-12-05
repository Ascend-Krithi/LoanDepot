using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Locators
{
    public static class LoginPageLocators
    {
        // Provided template locators (update dynamically if needed)
        public static IEnumerable<By> Username => new List<By>
        {
            By.Id("username"),
            By.XPath("//input[@name='username' or @placeholder='Username']"),
            By.CssSelector("input[type='text'].login-field")
        };

        public static IEnumerable<By> Password => new List<By>
        {
            By.Id("password"),
            By.XPath("//input[@name='password']"),
            By.CssSelector("input[type='password']")
        };

        public static IEnumerable<By> LoginButton => new List<By>
        {
            By.XPath("//button[normalize-space()='Log In' or @type='submit']"),
            By.CssSelector(".btn-primary.login-btn")
        };
    }
}
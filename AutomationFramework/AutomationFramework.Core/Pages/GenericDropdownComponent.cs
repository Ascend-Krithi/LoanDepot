using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent
    {
        private readonly IWebDriver _driver;
        private readonly By _dropdown;

        public GenericDropdownComponent(IWebDriver driver, By dropdown)
        {
            _driver = driver;
            _dropdown = dropdown;
        }

        public void SelectByText(string text)
        {
            var select = new SelectElement(_driver.FindElement(_dropdown));
            select.SelectByText(text);
        }

        public void SelectByValue(string value)
        {
            var select = new SelectElement(_driver.FindElement(_dropdown));
            select.SelectByValue(value);
        }

        public string SelectedOption()
        {
            var select = new SelectElement(_driver.FindElement(_dropdown));
            return select.SelectedOption.Text;
        }
    }
}
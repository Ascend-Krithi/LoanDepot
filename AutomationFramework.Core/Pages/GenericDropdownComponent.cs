using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        private readonly IWebElement _selectElement;

        public GenericDropdownComponent(SelfHealingWebDriver driver, By locator) : base(driver)
        {
            _selectElement = FindElement("GenericDropdown.SelectElement", locator);
        }

        public void SelectByText(string text)
        {
            var select = new SelectElement(_selectElement);
            select.SelectByText(text);
        }

        public void SelectByValue(string value)
        {
            var select = new SelectElement(_selectElement);
            select.SelectByValue(value);
        }
    }
}
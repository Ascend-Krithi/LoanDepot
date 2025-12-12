// AutomationFramework.Core/Pages/GenericDropdownComponent.cs
using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        private readonly By _dropdownLocator;
        private readonly string _logicalKey;

        public GenericDropdownComponent(SelfHealingWebDriver driver, By dropdownLocator, string logicalKey) : base(driver)
        {
            _dropdownLocator = dropdownLocator;
            _logicalKey = logicalKey;
        }

        private IWebElement DropdownElement => FindElement(_logicalKey, _dropdownLocator);

        public void SelectByText(string text)
        {
            var selectElement = new SelectElement(DropdownElement);
            selectElement.SelectByText(text);
        }

        public void SelectByValue(string value)
        {
            var selectElement = new SelectElement(DropdownElement);
            selectElement.SelectByValue(value);
        }
    }
}
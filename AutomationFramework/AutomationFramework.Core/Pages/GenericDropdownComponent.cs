using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent
    {
        private readonly IWebElement _dropdownTrigger;
        private readonly SelfHealingWebDriver _driver;

        public GenericDropdownComponent(SelfHealingWebDriver driver, IWebElement dropdownTrigger)
        {
            _driver = driver;
            _dropdownTrigger = dropdownTrigger;
        }

        public void SelectByText(string text)
        {
            _dropdownTrigger.Click();
            // This assumes dropdown options appear as siblings or in a related container
            var option = _driver.FindElement(By.XPath($"//div[contains(@class, 'dropdown-menu')]//a[text()='{text}']"));
            option.Click();
        }

        public void SelectByIndex(int index)
        {
            _dropdownTrigger.Click();
            var options = _driver.FindElements(By.CssSelector(".dropdown-menu a, .dropdown-menu li"));
            if (index < options.Count)
            {
                options[index].Click();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(index), "Index is out of range for dropdown options.");
            }
        }
    }
}
// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    /// <summary>
    /// A helper class to interact with custom dropdowns (e.g., those made with divs and uls).
    /// This is for non-<select> elements.
    /// </summary>
    public class GenericDropdownComponent
    {
        private readonly IWebDriver _driver;
        private readonly WaitHelper _wait;
        private readonly IWebElement _dropdownTrigger;
        private readonly By _optionsLocator;

        /// <summary>
        /// Initializes a new instance of the GenericDropdownComponent.
        /// </summary>
        /// <param name="driver">The WebDriver instance.</param>
        /// <param name="triggerLocator">The locator for the element that opens the dropdown.</param>
        /// <param name="optionsLocator">The locator for the dropdown option elements (e.g., By.CssSelector("ul > li")).</param>
        public GenericDropdownComponent(IWebDriver driver, By triggerLocator, By optionsLocator)
        {
            _driver = driver;
            _wait = new WaitHelper(driver);
            _dropdownTrigger = _wait.WaitForElementClickable(triggerLocator);
            _optionsLocator = optionsLocator;
        }

        /// <summary>
        /// Selects an option from the dropdown by its visible text.
        /// </summary>
        /// <param name="optionText">The text of the option to select (case-sensitive).</param>
        public void SelectByText(string optionText)
        {
            _dropdownTrigger.Click();
            var options = _wait.WaitForElementVisible(_optionsLocator).FindElements(By.XPath(".//*")); // Find all descendants
            var optionToSelect = options.FirstOrDefault(o => o.Text.Trim() == optionText && o.Displayed);

            if (optionToSelect == null)
            {
                throw new NoSuchElementException($"Option with text '{optionText}' not found or not visible in the dropdown.");
            }
            optionToSelect.Click();
        }

        /// <summary>
        /// Selects an option from the dropdown by its index.
        /// </summary>
        /// <param name="index">The zero-based index of the option to select.</param>
        public void SelectByIndex(int index)
        {
            _dropdownTrigger.Click();
            var options = _driver.FindElements(_optionsLocator);
            if (index < 0 || index >= options.Count)
            {
                throw new System.ArgumentOutOfRangeException(nameof(index), "Index is out of range for the dropdown options.");
            }
            options[index].Click();
        }
    }
}
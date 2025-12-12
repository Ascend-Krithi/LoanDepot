using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent
    {
        private readonly IWebElement _dropdownElement;
        private readonly SelectElement _selectElement;

        public GenericDropdownComponent(IWebElement dropdownElement)
        {
            _dropdownElement = dropdownElement;
            // Check if it's a standard <select> element
            if (dropdownElement.TagName.ToLower() == "select")
            {
                _selectElement = new SelectElement(_dropdownElement);
            }
        }

        public void SelectByText(string text)
        {
            if (_selectElement != null)
            {
                _selectElement.SelectByText(text);
            }
            else
            {
                // Handle custom dropdowns (e.g., div/ul/li based)
                _dropdownElement.Click(); // Open the dropdown
                var option = _dropdownElement.FindElement(By.XPath($"//li[normalize-space()='{text}'] | //div[normalize-space()='{text}']"));
                option.Click();
            }
        }

        public void SelectByValue(string value)
        {
            if (_selectElement != null)
            {
                _selectElement.SelectByValue(value);
            }
            else
            {
                throw new NotSupportedException("SelectByValue is not supported for custom dropdowns in this generic component.");
            }
        }

        public string GetSelectedOptionText()
        {
            if (_selectElement != null)
            {
                return _selectElement.SelectedOption.Text;
            }
            else
            {
                // For custom dropdowns, the selected value might be in the main element's text
                return _dropdownElement.Text;
            }
        }
    }
}
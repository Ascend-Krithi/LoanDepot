using System.Linq;
using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        public const string DropdownKey = "GenericDropdown.Dropdown";
        public const string OptionKey = "GenericDropdown.Option";

        private readonly By dropdown = By.CssSelector("select, .dropdown, [role='listbox']");
        private readonly By option = By.CssSelector("option, [role='option'], .dropdown-item");

        public GenericDropdownComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Dropdown => FindElement(DropdownKey, dropdown);

        public IWebElement GetOptionByText(string text)
        {
            var options = Dropdown.FindElements(option);
            return options.FirstOrDefault(o => o.Text.Trim().Equals(text.Trim(), System.StringComparison.OrdinalIgnoreCase));
        }

        public void SelectByText(string text)
        {
            var opt = GetOptionByText(text);
            if (opt != null)
                JsClick(opt);
        }
    }
}
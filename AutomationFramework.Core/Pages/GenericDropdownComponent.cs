using OpenQA.Selenium;
using System.Linq;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        public const string DropdownKey = "GenericDropdown.Dropdown";
        public const string OptionKey = "GenericDropdown.Option";

        private readonly By _dropdown = By.CssSelector("select, .dropdown");
        private readonly By _option = By.CssSelector("option, .dropdown-item");

        public GenericDropdownComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Dropdown => FindElement(DropdownKey, _dropdown);

        public IWebElement GetOptionByText(string text)
        {
            var options = Dropdown.FindElements(_option);
            return options.FirstOrDefault(o => o.Text.Trim() == text);
        }

        public void SelectByText(string text)
        {
            var option = GetOptionByText(text);
            if (option != null)
                JsClick(option);
        }
    }
}
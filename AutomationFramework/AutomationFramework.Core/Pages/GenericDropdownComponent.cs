using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        public const string DropdownKey = "GenericDropdown.Dropdown";
        public const string OptionKey = "GenericDropdown.Option";

        private readonly By dropdown = By.CssSelector("select, .dropdown");
        private readonly By option = By.CssSelector("option, .dropdown-item");

        public GenericDropdownComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Dropdown => FindElement(DropdownKey, dropdown);
        public IReadOnlyCollection<IWebElement> Options => Driver.InnerDriver.FindElements(option);
    }
}
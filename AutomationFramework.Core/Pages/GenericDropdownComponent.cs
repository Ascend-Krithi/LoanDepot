using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public class GenericDropdownComponent : BasePage
    {
        public const string DropdownKey = "GenericDropdown.Dropdown";
        public const string OptionKey = "GenericDropdown.Option";

        private readonly By _dropdown = By.CssSelector("select,.dropdown,[role='listbox']");
        private readonly By _option = By.CssSelector("option,[role='option']");

        public GenericDropdownComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement Dropdown => FindElement(DropdownKey, _dropdown);

        public IList<IWebElement> Options
        {
            get
            {
                var dropdown = Dropdown;
                return dropdown.FindElements(_option);
            }
        }
    }
}
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent : BasePage
    {
        private readonly IWebElement _modalContainer;

        public GenericModalComponent(IWebDriver driver, By modalLocator) : base(driver)
        {
            _modalContainer = Wait.WaitForElementToBeVisible(modalLocator);
        }

        // Override GetElement to search within the modal context
        protected new IWebElement GetElement(string logicalName)
        {
            if (!Locators.ContainsKey(logicalName))
            {
                throw new KeyNotFoundException($"Locator with logical name '{logicalName}' not found in the modal component.");
            }
            return _modalContainer.FindElement(Locators[logicalName]);
        }

        public void ClickButton(string buttonText)
        {
            var buttonLocator = By.XPath($".//button[normalize-space()='{buttonText}']");
            _modalContainer.FindElement(buttonLocator).Click();
        }

        public string GetModalTitle()
        {
            var titleLocator = By.CssSelector(".modal-title, h2, h3"); // Common title selectors
            return _modalContainer.FindElement(titleLocator).Text;
        }

        public void CloseModal()
        {
            var closeButtonLocator = By.CssSelector("button.close, [aria-label='Close']");
            _modalContainer.FindElement(closeButtonLocator).Click();
        }
    }
}
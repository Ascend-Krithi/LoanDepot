using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent : BasePage
    {
        // Logical Keys
        private const string ModalContainerKey = "GenericModal.Container";
        private const string CloseButtonKey = "GenericModal.CloseButton";

        // Locators
        private readonly By _modalContainer = By.CssSelector(".modal-content, [role='dialog']");
        private readonly By _closeButton = By.CssSelector(".modal-header .close, button[aria-label*='close']");

        public GenericModalComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement ModalContainer => FindElement(ModalContainerKey, _modalContainer);
        public IWebElement CloseButton => FindElement(CloseButtonKey, _closeButton);

        public void Close()
        {
            CloseButton.Click();
        }
    }
}
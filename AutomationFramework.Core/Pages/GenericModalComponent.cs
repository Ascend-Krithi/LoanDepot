using OpenQA.Selenium;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent : BasePage
    {
        public const string ModalContainerKey = "GenericModal.ModalContainer";
        public const string CloseButtonKey = "GenericModal.CloseButton";

        private readonly By _modalContainer = By.CssSelector("[role='dialog'],.modal,[aria-modal='true']");
        private readonly By _closeButton = By.CssSelector("[aria-label*='close'],.close,button.close");

        public GenericModalComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement ModalContainer => FindElement(ModalContainerKey, _modalContainer);
        public IWebElement CloseButton => FindElement(CloseButtonKey, _closeButton);
    }
}
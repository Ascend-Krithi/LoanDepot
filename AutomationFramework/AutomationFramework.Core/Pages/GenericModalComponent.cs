using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent : BasePage
    {
        public const string ModalContainerKey = "GenericModal.ModalContainer";
        public const string CloseButtonKey = "GenericModal.CloseButton";

        private readonly By modalContainer = By.CssSelector("[role='dialog'], .modal");
        private readonly By closeButton = By.CssSelector("[data-dismiss], .close, [aria-label='Close']");

        public GenericModalComponent(SelfHealingWebDriver driver) : base(driver) { }

        public IWebElement ModalContainer => FindElement(ModalContainerKey, modalContainer);
        public IWebElement CloseButton => FindElement(CloseButtonKey, closeButton);
    }
}
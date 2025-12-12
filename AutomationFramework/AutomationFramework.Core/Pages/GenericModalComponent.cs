using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class GenericModalComponent : BasePage
    {
        private readonly IWebElement _modalContainer;

        // This component is initialized with a reference to the modal's root element
        public GenericModalComponent(SelfHealingWebDriver driver, IWebElement modalContainer) : base(driver)
        {
            _modalContainer = modalContainer;
        }

        // Find elements relative to the modal container
        private IWebElement Title => _modalContainer.FindElement(By.CssSelector(".modal-title"));
        private IWebElement Body => _modalContainer.FindElement(By.CssSelector(".modal-body"));
        private IWebElement ConfirmButton => _modalContainer.FindElement(By.CssSelector(".btn-primary, .confirm-button"));
        private IWebElement CancelButton => _modalContainer.FindElement(By.CssSelector(".btn-secondary, .cancel-button"));

        public string GetTitle() => Title.Text;
        public string GetBodyText() => Body.Text;
        public void ClickConfirm() => ConfirmButton.Click();
        public void ClickCancel() => CancelButton.Click();
    }
}
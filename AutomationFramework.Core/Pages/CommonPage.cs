using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class CommonPage : BasePage
    {
        public CommonPage(SelfHealingWebDriver driver) : base(driver) { }

        public void WaitForLoader()
        {
            Driver.WaitForElementToDisappear("PageLoader");
        }

        public string GetBreadcrumb()
        {
            return Driver.GetText("Breadcrumb");
        }

        public void CloseModal()
        {
            Driver.Click("ModalCloseButton");
        }

        public void ConfirmModal()
        {
            Driver.Click("ConfirmButton");
        }

        public void CancelModal()
        {
            Driver.Click("CancelButton");
        }

        public void ClickPrimaryButton()
        {
            Driver.Click("PrimaryButton");
        }

        public void ClickSecondaryButton()
        {
            Driver.Click("SecondaryButton");
        }

        public string GetToastSuccess()
        {
            return Driver.GetText("ToastSuccess");
        }

        public string GetToastError()
        {
            return Driver.GetText("ToastError");
        }

        public void GoToNextPage()
        {
            Driver.Click("PaginationNext");
        }

        public void GoToPreviousPage()
        {
            Driver.Click("PaginationPrev");
        }
    }
}
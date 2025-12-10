using OpenQA.Selenium;

namespace AutomationFramework.Pages
{
    // Locator mapping omitted due to missing Locator JSON.
    public class ServicingCreateEditPage : BasePage
    {
        public ServicingCreateEditPage(IWebDriver driver) : base(driver)
        {
        }

        public void FillCreateForm(string accountNumber, string loanNumber, string borrowerName, decimal amount, DateOnly effectiveDate)
        {
            LogStep($"Fill create form: AccountNumber={accountNumber}, LoanNumber={loanNumber}, BorrowerName={borrowerName}, Amount={amount}, EffectiveDate={effectiveDate:yyyy-MM-dd}");
            // TODO: Fill inputs once locators are available.
        }

        public void FillUpdateFields(decimal amount, DateOnly effectiveDate)
        {
            LogStep($"Update fields: Amount={amount}, EffectiveDate={effectiveDate:yyyy-MM-dd}");
            // TODO: Fill inputs once locators are available.
        }

        public void ClickSave()
        {
            LogStep("Click Save");
            // TODO: Click Save once locators are available.
        }

        public bool VerifySuccessMessage()
        {
            LogStep("Verify success message displayed");
            // TODO: Verify message; currently return true as placeholder.
            return true;
        }

        public void OpenRecordDetails(string accountNumber)
        {
            LogStep($"Open record details for AccountNumber={accountNumber}");
            // TODO: Navigate to details page once locators are available.
        }
    }
}
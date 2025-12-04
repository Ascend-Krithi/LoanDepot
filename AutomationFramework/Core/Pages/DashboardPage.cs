using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Locators;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage
    {
        private readonly SelfHealingWebDriver _driver;

        public DashboardPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectLoanByName(string loanName)
        {
            var loanList = _driver.FindElementByKey(nameof(DashboardPageLocators.LoanListLocator));
            var loanItem = loanList.FindElements(By.TagName("li"))
                .FirstOrDefault(li => li.Text.Contains(loanName));
            if (loanItem != null)
                loanItem.Click();
            else
                throw new Exception($"Loan '{loanName}' not found in list.");
        }

        public void DismissChatPopupIfPresent()
        {
            try
            {
                var popup = _driver.FindElementByKey(nameof(DashboardPageLocators.ChatPopupLocator));
                if (popup.Displayed)
                {
                    var dismissBtn = _driver.FindElementByKey(nameof(DashboardPageLocators.DismissChatButtonLocator));
                    dismissBtn.Click();
                }
            }
            catch { /* Popup not present, ignore */ }
        }
    }
}
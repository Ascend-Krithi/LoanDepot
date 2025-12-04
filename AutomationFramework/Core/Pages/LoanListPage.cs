using AutomationFramework.Core.SelfHealing;
using OpenQA.Selenium;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class LoanListPage
    {
        private readonly SelfHealingWebDriver _driver;
        public LoanListPage(SelfHealingWebDriver driver)
        {
            _driver = driver;
        }

        public void SelectLoanByName(string loanName)
        {
            var rows = _driver.FindElements(Locators.LoanListPageLocators.LoanRowLocator);
            foreach (var row in rows)
            {
                if (row.Text.Contains(loanName))
                {
                    row.Click();
                    break;
                }
            }
        }

        public void DismissPopup()
        {
            var closeBtn = _driver.FindElementByKey(Locators.LoanListPageLocators.PopupCloseKey);
            closeBtn.Click();
        }
    }
}
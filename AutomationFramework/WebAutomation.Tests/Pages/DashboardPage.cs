using OpenQA.Selenium;
using WebAutomation.Core.Utilities;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage
    {
        private readonly IWebDriver _driver;
        private readonly SmartWait _wait;

        public DashboardPage(IWebDriver driver)
        {
            _driver = driver;
            _wait = new SmartWait(driver);
        }

        public bool IsPageReady()
        {
            return _wait.UntilPresent(By.CssSelector("header"), 10);
        }

        public void DismissContactUpdatePopup()
        {
            if (_wait.UntilPresent(By.CssSelector("mat-dialog-container"), 3))
            {
                if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Update Later']"), 2))
                {
                    _wait.UntilClickable(By.XPath("//button[normalize-space()='Update Later']")).Click();
                }
                else if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Continue']"), 2))
                {
                    _wait.UntilClickable(By.XPath("//button[normalize-space()='Continue']")).Click();
                }
            }
        }

        public void DismissChatbotIframe()
        {
            try
            {
                var iframe = _driver.FindElement(By.Id("servisbot-messenger-iframe-roundel"));
                ((IJavaScriptExecutor)_driver).ExecuteScript("arguments[0].style.display='none';", iframe);
            }
            catch { /* Ignore if not present */ }
        }

        public void SelectLoanAccount(string loanNumber)
        {
            _wait.UntilClickable(By.CssSelector("button[logaction='Open Loan Selection Window']")).Click();
            _wait.UntilClickable(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]")).Click();
            Thread.Sleep(1000); // Allow loan details to load
        }

        public bool IsLoanDetailsLoaded(string loanNumber)
        {
            return _wait.UntilPresent(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]"), 10);
        }

        public void ClickMakePayment()
        {
            _wait.UntilClickable(By.CssSelector("p.make-payment")).Click();
        }

        public void ContinueScheduledPaymentPopupIfPresent()
        {
            if (_wait.UntilPresent(By.XPath("//button[normalize-space()='Continue']"), 3))
            {
                _wait.UntilClickable(By.XPath("//button[normalize-space()='Continue']")).Click();
            }
        }
    }
}
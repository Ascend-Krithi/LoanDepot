using OpenQA.Selenium;
using WebAutomation.Core.Pages;
using System.Threading;

namespace WebAutomation.Tests.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(IWebDriver driver) : base(driver) { }

        public void WaitForDashboardReady()
        {
            Wait.UntilVisible(By.CssSelector("header"));
        }

        public void DismissPopupsIfPresent()
        {
            // Contact Update popup
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Update Later']"));
            // Chatbot iframe
            try
            {
                var iframe = Driver.FindElements(By.Id("servisbot-messenger-iframe-roundel"));
                if (iframe.Count > 0)
                {
                    ((IJavaScriptExecutor)Driver).ExecuteScript("arguments[0].style.display='none';", iframe[0]);
                }
            }
            catch { }
        }

        public void SelectLoanAccount(string loanNumber)
        {
            Wait.UntilClickable(By.CssSelector("button[logaction='Open Loan Selection Window']")).Click();
            Wait.UntilClickable(By.XPath($"//p[contains(normalize-space(.),'Account - {loanNumber}')]")).Click();
        }

        public void ClickMakePayment()
        {
            Wait.UntilClickable(By.CssSelector("p.make-payment")).Click();
        }

        public void HandleScheduledPaymentPopupIfPresent()
        {
            Popup.HandleIfPresent(By.XPath("//button[normalize-space()='Continue']"));
        }
    }
}
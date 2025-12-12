using OpenQA.Selenium;

namespace AutomationFramework.Core.Pages
{
    public class MFAVerificationPageTemplate : BasePage
    {
        private readonly By emailDropdown = By.Id("emailInput");
        private readonly By receiveCodeButton = By.Id("emailCodeBtn");

        public void SelectEmailOption()
        {
            var dropdown = FindElement("MFAPage.EmailDropdown", emailDropdown);
            dropdown.Click();
            // Select first option logic here (could use SelectElement if <select>)
        }

        public void ClickReceiveCodeViaEmail()
        {
            var button = FindElement("MFAPage.ReceiveCodeButton", receiveCodeButton);
            button.Click();
        }
    }
}
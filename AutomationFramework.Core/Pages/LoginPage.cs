using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    // Automation for Page: LoginPage
    public class LoginPage : BasePage
    {
        public LoginPage(SelfHealingWebDriver driver) : base(driver) { }

        public void EnterUsername(string username)
        {
            BeforeCriticalAction();
            Driver.FindElement("UsernameInput").SendKeys(username);
        }

        public void EnterPassword(string password)
        {
            BeforeCriticalAction();
            Driver.FindElement("PasswordInput").SendKeys(password);
        }

        public void ClickLogin()
        {
            BeforeCriticalAction();
            Driver.FindElement("LoginButton").Click();
        }
    }
}
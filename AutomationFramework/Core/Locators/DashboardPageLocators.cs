using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class DashboardPageLocators
    {
        public const string LoanList = "DashboardPage.LoanList";
        public static readonly By LoanListLocator = By.XPath("//div[@class='loan-list']");
        public static readonly By[] LoanListAlternatives = {
            By.CssSelector(".loan-list"),
            By.XPath("//ul[contains(@class,'loan-list')]")
        };

        public const string ChatPopup = "DashboardPage.ChatPopup";
        public static readonly By ChatPopupLocator = By.XPath("//div[@id='chat-popup']");
        public static readonly By[] ChatPopupAlternatives = {
            By.CssSelector("#chat-popup"),
            By.XPath("//div[contains(@class,'chat-popup')]")
        };

        public const string DismissChatButton = "DashboardPage.DismissChatButton";
        public static readonly By DismissChatButtonLocator = By.XPath("//button[@id='dismiss-chat']");
        public static readonly By[] DismissChatButtonAlternatives = {
            By.CssSelector("button.dismiss-chat"),
            By.XPath("//button[contains(text(),'Dismiss')]")
        };
    }
}
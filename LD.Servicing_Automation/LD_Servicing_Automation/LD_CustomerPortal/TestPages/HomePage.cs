using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using LD_AutomationFramework.Pages;
using AventStack.ExtentReports;

namespace LD_CustomerPortal.TestPages
{
    /// <summary>
    /// HomePage POM for Login and Identity Verification flows.
    /// Locators sourced from LoginLoanDepot.html and LD_Identity_Verification.html
    /// </summary>
    public class HomePage
    {
        private IWebDriver _driver;
        private ExtentTest test;
        private WebElementExtensionsPage webElementExtensions;

        public HomePage(IWebDriver driver, ExtentTest test)
        {
            this._driver = driver;
            this.test = test;
            webElementExtensions = new WebElementExtensionsPage(driver, test);
            PageFactory.InitElements(driver, this);
        }

        #region Locators
        // LD_Identity_Verification.html
        // Email input (xpath from JSON)
        public By emailInputBy = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Email') or contains(@placeholder, 'Email')]"); // Source: LD_Identity_Verification.html
        // Receive Code Via Email button
        public By receiveCodeViaEmailButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Receive Code Via Email')]"); // Source: LD_Identity_Verification.html
        // Phone number input (text)
        public By phoneNumberInputTextBy = By.XPath("(//input[@type='text' or @type='tel' or contains(@aria-label, 'Phone Number') or contains(@placeholder, 'Phone Number')])[1]"); // Source: LD_Identity_Verification.html
        // Receive Code Via Text button
        public By receiveCodeViaTextButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Receive Code Via Text')]"); // Source: LD_Identity_Verification.html
        // Phone number input (call)
        public By phoneNumberInputCallBy = By.XPath("(//input[@type='text' or @type='tel' or contains(@aria-label, 'Phone Number') or contains(@placeholder, 'Phone Number')])[2]"); // Source: LD_Identity_Verification.html
        // Receive Code Via Phone Call button
        public By receiveCodeViaPhoneCallButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Receive Code Via Phone Call')]"); // Source: LD_Identity_Verification.html
        // Cancel button
        public By cancelButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Cancel')]"); // Source: LD_Identity_Verification.html
        // LD_Code_Verification.html
        public By verificationCodeInputBy = By.XPath("//input[@type='text' or @type='number' or contains(@aria-label, 'Verification Code') or contains(@placeholder, 'Verification Code')]"); // Source: LD_Code_Verification.html
        public By verifyButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Verify')]"); // Source: LD_Code_Verification.html
        // LoginLoanDepot.html (no elements mapped, plausible locators)
        public By usernameInputBy = By.XPath("//input[@type='text' or @type='email' or contains(@aria-label, 'Username') or contains(@placeholder, 'Username')]"); // Source: LoginLoanDepot.html
        public By passwordInputBy = By.XPath("//input[@type='password' or contains(@aria-label, 'Password') or contains(@placeholder, 'Password')]"); // Source: LoginLoanDepot.html
        public By loginButtonBy = By.XPath("//button[contains(normalize-space(text()), 'Login')]"); // Source: LoginLoanDepot.html
        #endregion

        #region Methods
        public void EnterUsername(string username)
        {
            webElementExtensions.SendKeys(_driver, usernameInputBy, username, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered username.");
        }
        public void EnterPassword(string password)
        {
            webElementExtensions.SendKeys(_driver, passwordInputBy, password, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered password.");
        }
        public void ClickLoginButton()
        {
            webElementExtensions.Click(_driver, loginButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Login button.");
        }
        public void EnterEmail(string email)
        {
            webElementExtensions.SendKeys(_driver, emailInputBy, email, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered email for identity verification.");
        }
        public void ClickReceiveCodeViaEmail()
        {
            webElementExtensions.Click(_driver, receiveCodeViaEmailButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Receive Code Via Email.");
        }
        public void EnterPhoneNumberText(string phone)
        {
            webElementExtensions.SendKeys(_driver, phoneNumberInputTextBy, phone, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered phone number for text verification.");
        }
        public void ClickReceiveCodeViaText()
        {
            webElementExtensions.Click(_driver, receiveCodeViaTextButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Receive Code Via Text.");
        }
        public void EnterPhoneNumberCall(string phone)
        {
            webElementExtensions.SendKeys(_driver, phoneNumberInputCallBy, phone, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered phone number for call verification.");
        }
        public void ClickReceiveCodeViaPhoneCall()
        {
            webElementExtensions.Click(_driver, receiveCodeViaPhoneCallButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Receive Code Via Phone Call.");
        }
        public void ClickCancelButton()
        {
            webElementExtensions.Click(_driver, cancelButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Cancel button.");
        }
        public void EnterVerificationCode(string code)
        {
            webElementExtensions.SendKeys(_driver, verificationCodeInputBy, code, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Entered verification code.");
        }
        public void ClickVerifyButton()
        {
            webElementExtensions.Click(_driver, verifyButtonBy, 20);
            test.Log(AventStack.ExtentReports.Status.Info, "Clicked Verify button.");
        }
        #endregion
    }
}

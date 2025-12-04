using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using AutomationFramework.Core.Locators;

namespace AutomationFramework.Core.SelfHealing
{
    public class SelfHealingWebDriver : IWebDriver
    {
        private readonly IWebDriver _driver;
        private readonly Dictionary<string, (By, By[])> _locatorRepo;
        public event Action<string, By, By> OnHealed;

        public SelfHealingWebDriver(IWebDriver driver)
        {
            _driver = driver;
            _locatorRepo = new Dictionary<string, (By, By[])>();

            // Register all locators here
            RegisterLocators();
        }

        private void RegisterLocators()
        {
            // LoginPage
            _locatorRepo[LoginPageLocators.UsernameInputKey] = (LoginPageLocators.UsernameInput, LoginPageLocators.UsernameInputAlternatives);
            _locatorRepo[LoginPageLocators.PasswordInputKey] = (LoginPageLocators.PasswordInput, LoginPageLocators.PasswordInputAlternatives);
            _locatorRepo[LoginPageLocators.LoginButtonKey] = (LoginPageLocators.LoginButton, LoginPageLocators.LoginButtonAlternatives);

            // DashboardPage
            _locatorRepo[DashboardPageLocators.LoanDropdownKey] = (DashboardPageLocators.LoanDropdown, DashboardPageLocators.LoanDropdownAlternatives);
            _locatorRepo[DashboardPageLocators.LoanListKey] = (DashboardPageLocators.LoanList, DashboardPageLocators.LoanListAlternatives);
            _locatorRepo[DashboardPageLocators.PopupCloseKey] = (DashboardPageLocators.PopupClose, DashboardPageLocators.PopupCloseAlternatives);
            _locatorRepo[DashboardPageLocators.ChatPopupKey] = (DashboardPageLocators.ChatPopup, DashboardPageLocators.ChatPopupAlternatives);
            _locatorRepo[DashboardPageLocators.DatePickerInputKey] = (DashboardPageLocators.DatePickerInput, DashboardPageLocators.DatePickerInputAlternatives);
            _locatorRepo[DashboardPageLocators.MessageKey] = (DashboardPageLocators.Message, DashboardPageLocators.MessageAlternatives);
        }

        public IWebElement FindElementByKey(string key)
        {
            if (!_locatorRepo.ContainsKey(key))
                throw new Exception($"Locator key '{key}' not found in repository.");

            var (primary, alternatives) = _locatorRepo[key];
            try
            {
                return _driver.FindElement(primary);
            }
            catch (NoSuchElementException)
            {
                foreach (var alt in alternatives)
                {
                    try
                    {
                        var el = _driver.FindElement(alt);
                        OnHealed?.Invoke(key, primary, alt);
                        return el;
                    }
                    catch { }
                }
                throw;
            }
        }

        // IWebDriver implementation (delegated)
        public string Url { get => _driver.Url; set => _driver.Url = value; }
        public string Title => _driver.Title;
        public string PageSource => _driver.PageSource;
        public string CurrentWindowHandle => _driver.CurrentWindowHandle;
        public ReadOnlyCollection<string> WindowHandles => _driver.WindowHandles;

        public void Close() => _driver.Close();
        public void Dispose() => _driver.Dispose();
        public IWebElement FindElement(By by) => _driver.FindElement(by);
        public ReadOnlyCollection<IWebElement> FindElements(By by) => _driver.FindElements(by);
        public IOptions Manage() => _driver.Manage();
        public INavigation Navigate() => _driver.Navigate();
        public void Quit() => _driver.Quit();
        public ITargetLocator SwitchTo() => _driver.SwitchTo();
    }
}
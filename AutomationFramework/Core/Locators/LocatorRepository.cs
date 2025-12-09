using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Locators
{
    public class LocatorRepository : ILocatorRepository
    {
        private readonly Dictionary<string, By> _locators = new()
        {
            { LoginPageLocators.UsernameInputKey, LoginPageLocators.UsernameInput },
            { LoginPageLocators.PasswordInputKey, LoginPageLocators.PasswordInput },
            { LoginPageLocators.LoginButtonKey, LoginPageLocators.LoginButton },
            { DashboardPageLocators.LoanDropdownKey, DashboardPageLocators.LoanDropdown },
            { DashboardPageLocators.LoanListKey, DashboardPageLocators.LoanList },
            { DashboardPageLocators.ChatPopupKey, DashboardPageLocators.ChatPopup },
            { DashboardPageLocators.DatePickerKey, DashboardPageLocators.DatePicker }
        };

        private readonly Dictionary<string, By[]> _alternatives = new()
        {
            { LoginPageLocators.UsernameInputKey, LoginPageLocators.UsernameInputAlternatives },
            { LoginPageLocators.PasswordInputKey, LoginPageLocators.PasswordInputAlternatives },
            { LoginPageLocators.LoginButtonKey, LoginPageLocators.LoginButtonAlternatives },
            { DashboardPageLocators.LoanDropdownKey, DashboardPageLocators.LoanDropdownAlternatives },
            { DashboardPageLocators.LoanListKey, DashboardPageLocators.LoanListAlternatives },
            { DashboardPageLocators.ChatPopupKey, DashboardPageLocators.ChatPopupAlternatives },
            { DashboardPageLocators.DatePickerKey, DashboardPageLocators.DatePickerAlternatives }
        };

        public By GetLocator(string key) => _locators[key];
        public By[] GetAlternatives(string key) => _alternatives.ContainsKey(key) ? _alternatives[key] : new By[0];
    }
}
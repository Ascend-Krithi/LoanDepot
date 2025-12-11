using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace AutomationFramework.Core.Locators
{
    public static class LocatorRepository
    {
        private static readonly Dictionary<string, (By primary, By[] alternatives)> _locators =
            new Dictionary<string, (By, By[])>
            {
                // LoginPage
                { LoginPageLocators.UsernameInputKey, (LoginPageLocators.UsernameInput, LoginPageLocators.UsernameInputAlternatives) },
                { LoginPageLocators.PasswordInputKey, (LoginPageLocators.PasswordInput, LoginPageLocators.PasswordInputAlternatives) },
                { LoginPageLocators.LoginButtonKey, (LoginPageLocators.LoginButton, LoginPageLocators.LoginButtonAlternatives) },

                // DashboardPage
                { DashboardPageLocators.LoanDropdownKey, (DashboardPageLocators.LoanDropdown, DashboardPageLocators.LoanDropdownAlternatives) },
                { DashboardPageLocators.LoanListKey, (DashboardPageLocators.LoanList, DashboardPageLocators.LoanListAlternatives) },
                { DashboardPageLocators.PopupCloseKey, (DashboardPageLocators.PopupClose, DashboardPageLocators.PopupCloseAlternatives) },
                { DashboardPageLocators.ChatPopupKey, (DashboardPageLocators.ChatPopup, DashboardPageLocators.ChatPopupAlternatives) },
                { DashboardPageLocators.DatePickerInputKey, (DashboardPageLocators.DatePickerInput, DashboardPageLocators.DatePickerInputAlternatives) },
                { DashboardPageLocators.MessageBannerKey, (DashboardPageLocators.MessageBanner, DashboardPageLocators.MessageBannerAlternatives) },

                // PaymentPage
                { PaymentPageLocators.AmountInputKey, (PaymentPageLocators.AmountInput, PaymentPageLocators.AmountInputAlternatives) },
                { PaymentPageLocators.PayButtonKey, (PaymentPageLocators.PayButton, PaymentPageLocators.PayButtonAlternatives) }
            };

        public static (By primary, By[] alternatives) GetLocators(string key)
        {
            if (!_locators.ContainsKey(key))
                throw new ArgumentException($"Locator key not found: {key}");
            return _locators[key];
        }
    }
}
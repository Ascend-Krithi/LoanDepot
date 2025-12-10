using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class AccountDetailsPage : BasePage
    {
        public AccountDetailsPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetAccountHeader()
        {
            return Driver.GetText("AccountHeader");
        }

        public string GetAccountNumber()
        {
            return Driver.GetText("AccountNumberLabel");
        }

        public string GetAccountStatus()
        {
            return Driver.GetText("AccountStatusBadge");
        }

        public void EditAccount()
        {
            Driver.Click("EditAccountButton");
        }

        public void SaveAccount()
        {
            Driver.Click("SaveAccountButton");
        }

        public void CancelEdit()
        {
            Driver.Click("CancelEditButton");
        }

        public string GetPrimaryContactName()
        {
            return Driver.GetText("PrimaryContactName");
        }

        public string GetPrimaryContactPhone()
        {
            return Driver.GetText("PrimaryContactPhone");
        }

        public string GetPrimaryContactEmail()
        {
            return Driver.GetText("PrimaryContactEmail");
        }

        public void EnterAddress(string line1, string line2, string city, string state, string zip, string country)
        {
            Driver.SendKeys("AddressLine1Input", line1);
            Driver.SendKeys("AddressLine2Input", line2);
            Driver.SendKeys("CityInput", city);
            Driver.SelectDropdownByText("StateDropdown", state);
            Driver.SendKeys("ZipInput", zip);
            Driver.SelectDropdownByText("CountryDropdown", country);
        }

        public void SelectPreferredLanguage(string language)
        {
            Driver.SelectDropdownByText("PreferredLanguageDropdown", language);
        }

        public void EnterAccountNotes(string notes)
        {
            Driver.SendKeys("AccountNotesTextarea", notes);
        }

        public string GetSaveToast()
        {
            return Driver.GetText("AccountSaveToast");
        }
    }
}
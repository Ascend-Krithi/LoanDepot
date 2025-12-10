using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class DashboardPage : BasePage
    {
        public DashboardPage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetHeaderTitle()
        {
            HandleUniversalPopups();
            return Driver.GetText("HeaderTitle");
        }

        public void Search(string query)
        {
            Driver.SendKeys("SearchInput", query);
            Driver.Click("SearchButton");
        }

        public void OpenNotifications()
        {
            Driver.Click("NotificationsIcon");
        }

        public void OpenUserMenu()
        {
            Driver.Click("UserMenuButton");
        }

        public void Logout()
        {
            Driver.Click("LogoutLink");
        }

        public void ClickCreateCase()
        {
            Driver.Click("CreateCaseButton");
        }

        public void OpenRecentCaseById(string caseId)
        {
            Driver.ClickDynamic("CaseRowById", caseId);
        }

        public void FilterCases(string filter)
        {
            Driver.SelectDropdownByText("CaseFilterDropdown", filter);
        }
    }
}
using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class ServicingHomePage : BasePage
    {
        public ServicingHomePage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetServicingHeader()
        {
            return Driver.GetText("ServicingHeader");
        }

        public void ClickCreateNewCase()
        {
            Driver.Click("CreateNewCaseButton");
        }

        public void SearchCase(string caseNumber)
        {
            Driver.SendKeys("CaseSearchInput", caseNumber);
            Driver.Click("CaseSearchButton");
        }

        public void OpenCaseByNumber(string caseNumber)
        {
            Driver.ClickDynamic("CaseRowByNumber", caseNumber);
        }

        public void OpenSelectedCase()
        {
            Driver.Click("OpenSelectedCaseButton");
        }

        public void FilterStatus(string status)
        {
            Driver.SelectDropdownByText("FilterStatusDropdown", status);
        }

        public void FilterOwner(string owner)
        {
            Driver.SelectDropdownByText("FilterOwnerDropdown", owner);
        }

        public void ClearFilters()
        {
            Driver.Click("ClearFiltersButton");
        }
    }
}
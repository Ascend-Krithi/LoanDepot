using AutomationFramework.Core.Base;
using AutomationFramework.Core.SelfHealing;

namespace AutomationFramework.Core.Pages
{
    public class ServicingCasePage : BasePage
    {
        public ServicingCasePage(SelfHealingWebDriver driver) : base(driver) { }

        public string GetCaseHeader()
        {
            return Driver.GetText("CaseHeader");
        }

        public string GetCaseNumber()
        {
            return Driver.GetText("CaseNumberLabel");
        }

        public string GetCaseStatus()
        {
            return Driver.GetText("CaseStatusBadge");
        }

        public string GetOwnerName()
        {
            return Driver.GetText("OwnerNameLabel");
        }

        public void AssignOwner(string owner)
        {
            Driver.Click("AssignOwnerButton");
            Driver.SelectDropdownByText("OwnerDropdown", owner);
        }

        public void EnterNotes(string notes)
        {
            Driver.SendKeys("NotesTextarea", notes);
        }

        public void AddNote()
        {
            Driver.Click("AddNoteButton");
        }

        public string GetNotesTimeline()
        {
            return Driver.GetText("NotesTimeline");
        }

        public void OpenAttachmentsTab()
        {
            Driver.Click("AttachmentsTab");
        }

        public void AddAttachment(string filePath)
        {
            Driver.SendKeys("AttachmentInput", filePath);
            Driver.Click("AddAttachmentButton");
        }

        public void SaveCase()
        {
            Driver.Click("SaveCaseButton");
        }

        public void CloseCase()
        {
            Driver.Click("CloseCaseButton");
        }

        public void ReopenCase()
        {
            Driver.Click("ReopenCaseButton");
        }

        public string GetSaveToast()
        {
            return Driver.GetText("CaseSaveToast");
        }

        public string GetErrorToast()
        {
            return Driver.GetText("CaseErrorToast");
        }
    }
}
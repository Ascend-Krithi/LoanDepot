using AutomationFramework.Core;
using AutomationFramework.Data.Models;
using NUnit.Framework;

namespace AutomationFramework.Tests
{
    [TestFixture]
    [Category("TC02")]
    public class TC02_ServicingCreateTests : BaseTest
    {
        [Test]
        public void Create_New_Loan_Servicing_Record()
        {
            Assert.Multiple(() =>
            {
                Assert.NotNull(DashboardPage, "DashboardPage should be initialized.");
                Assert.NotNull(ServicingListingPage, "ServicingListingPage should be initialized.");
                Assert.NotNull(ServicingCreateEditPage, "ServicingCreateEditPage should be initialized.");
                Assert.NotNull(DataProvider, "DataProvider should be initialized.");
                Assert.NotNull(Flow, "ScreenFlow should be loaded.");
            });

            // Preconditions: User is logged in and on Dashboard
            // From Screen Flow: 2. Dashboard -> 3. Servicing Listing
            DashboardPage!.NavigateToServicingModule();

            // Steps: Click "Create New"
            ServicingListingPage!.ClickCreateNew();

            // Fill servicing form
            ServicingCreateData data = DataProvider!.GetServicingCreateData();
            ServicingCreateEditPage!.FillCreateForm(
                data.AccountNumber,
                data.LoanNumber,
                data.BorrowerName,
                data.Amount,
                data.EffectiveDate
            );

            // Save
            ServicingCreateEditPage.ClickSave();

            // Verify success message
            Assert.IsTrue(ServicingCreateEditPage.VerifySuccessMessage(), "Success message should be displayed after create.");

            // Verify record appears in listing
            Assert.IsTrue(
                ServicingListingPage.VerifyRecordInListing(data.AccountNumber, data.LoanNumber),
                "New servicing record should be visible in the listing with matching AccountNumber and LoanNumber."
            );
        }
    }
}
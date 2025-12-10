using AutomationFramework.Core;
using AutomationFramework.Data.Models;
using NUnit.Framework;

namespace AutomationFramework.Tests
{
    [TestFixture]
    [Category("TC03")]
    public class TC03_ServicingUpdateTests : BaseTest
    {
        [Test]
        public void Update_Existing_Loan_Servicing_Record()
        {
            Assert.Multiple(() =>
            {
                Assert.NotNull(ServicingListingPage, "ServicingListingPage should be initialized.");
                Assert.NotNull(ServicingCreateEditPage, "ServicingCreateEditPage should be initialized.");
                Assert.NotNull(DataProvider, "DataProvider should be initialized.");
            });

            // Preconditions: User is logged in and in Servicing listing
            // Steps: Search for existing record using AccountNumber
            ServicingUpdateData update = DataProvider!.GetServicingUpdateData();

            ServicingListingPage!.SearchByAccountNumber(update.AccountNumber);
            ServicingCreateEditPage!.OpenRecordDetails(update.AccountNumber);

            // Update fields
            ServicingCreateEditPage.FillUpdateFields(update.Amount, update.EffectiveDate);

            // Save
            ServicingCreateEditPage.ClickSave();

            // Verify success message
            Assert.IsTrue(ServicingCreateEditPage.VerifySuccessMessage(), "Success message should be displayed after update.");

            // Verify listing shows updated fields
            Assert.IsTrue(
                ServicingListingPage.VerifyUpdatedRecord(update.AccountNumber, update.Amount, update.EffectiveDate),
                "Listing should reflect updated Amount and EffectiveDate."
            );
        }
    }
}
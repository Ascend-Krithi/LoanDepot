// Automation for TestCaseID: TC03
using TechTalk.SpecFlow;
using AutomationFramework.Core.Pages;
using AutomationFramework.Core.SelfHealing;
using AutomationFramework.Core.Utilities;
using NUnit.Framework;

namespace AutomationFramework.Tests.StepDefinitions
{
    [Binding]
    public class TC03_Payment_ProcessingStepDefinitions
    {
        private readonly PaymentPage _paymentPage;

        public TC03_Payment_ProcessingStepDefinitions(SelfHealingWebDriver driver)
        {
            _paymentPage = new PaymentPage(driver);
        }

        [Given(@"I am on the Payment Page")]
        public void GivenIAmOnThePaymentPage()
        {
            // Navigation handled in Hooks
        }

        [When(@"I enter payment details")]
        public void WhenIEnterPaymentDetails()
        {
            var data = TestDataReader.GetTestData("TC03");
            _paymentPage.EnterAmount(data["Amount"].ToString());
        }

        [When(@"I submit the payment")]
        public void WhenISubmitThePayment()
        {
            _paymentPage.ClickSubmit();
        }

        [Then(@"I should see a payment success message")]
        public void ThenIShouldSeeAPaymentSuccessMessage()
        {
            Assert.IsTrue(_paymentPage.IsPaymentSuccess());
        }
    }
}
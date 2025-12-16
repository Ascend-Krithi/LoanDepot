namespace AutomationFramework.Core.Models
{
    public class PaymentScenario
    {
        public string TestCaseId { get; set; }
        public string Scenario { get; set; }
        public string LoanNumber { get; set; }
        public string PaymentDate { get; set; }
        public string State { get; set; }
        public bool ExpectedLateFee { get; set; }
    }
}
namespace AutomationFramework.Core.Models
{
    public class PaymentScenario
    {
        public string LoanAccount { get; set; }
        public string PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string PaymentType { get; set; }
    }
}
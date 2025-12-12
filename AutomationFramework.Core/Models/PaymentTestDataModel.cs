namespace AutomationFramework.Core.Models
{
    public class PaymentTestDataModel
    {
        public string TestCaseId { get; set; }
        public string Scenario { get; set; }
        public string LoanNumber { get; set; }
        public string PaymentDate { get; set; }
        public string State { get; set; }
        public bool ExpectedLateFee { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string OTP { get; set; }
    }
}
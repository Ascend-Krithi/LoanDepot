using System;

namespace AutomationFramework.Core.Models
{
    public class LateFeeValidationModel
    {
        public string TestCaseId { get; set; }
        public string Scenario { get; set; }
        public string LoanNumber { get; set; }
        public DateTime PaymentDate { get; set; }
        public string State { get; set; }
        public bool ExpectedLateFee { get; set; }
    }
}
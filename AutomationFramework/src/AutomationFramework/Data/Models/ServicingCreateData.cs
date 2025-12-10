namespace AutomationFramework.Data.Models
{
    public class ServicingCreateData
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string LoanNumber { get; set; } = string.Empty;
        public string BorrowerName { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly EffectiveDate { get; set; }
    }
}
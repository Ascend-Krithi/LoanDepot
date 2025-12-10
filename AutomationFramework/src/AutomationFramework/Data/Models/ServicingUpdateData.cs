namespace AutomationFramework.Data.Models
{
    public class ServicingUpdateData
    {
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Amount { get; set; }
        public DateOnly EffectiveDate { get; set; }
    }
}
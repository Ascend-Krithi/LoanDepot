using System.Collections.Generic;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        // Placeholder returning dummy data; replace with real Excel reading if needed
        public static Dictionary<string, string> GetLoanData()
        {
            return new Dictionary<string, string>
            {
                {"BorrowerName", "John Doe"},
                {"LoanAmount", "250000"},
                {"Term", "360"}
            };
        }

        public static Dictionary<string, string> GetPaymentData()
        {
            return new Dictionary<string, string>
            {
                {"Amount", "1500"},
                {"Method", "ACH"}
            };
        }
    }
}

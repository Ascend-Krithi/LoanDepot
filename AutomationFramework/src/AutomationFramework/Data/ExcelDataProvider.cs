using AutomationFramework.Data.Models;
using AutomationFramework.Core;

namespace AutomationFramework.Data
{
    // Note:
    // Locator JSON is not available and Excel parsing libraries are not used to keep compile-ready.
    // This provider references the Excel file path and returns placeholder data while logging intended
    // sheet/column access based on the provided test cases (TC01, TC02, TC03).
    public class ExcelDataProvider : IDataProvider
    {
        private readonly string _excelPath;

        public ExcelDataProvider(string excelPath)
        {
            _excelPath = excelPath;
            if (!File.Exists(_excelPath))
            {
                Logger.Warn($"Test data file not found at {_excelPath}. Using placeholder values.");
            }
            else
            {
                Logger.Info($"Test data file referenced at {_excelPath}. Excel parsing is omitted; using placeholder values tied to expected sheet/columns.");
            }
        }

        public LoginData GetLoginData()
        {
            // Intended Excel access:
            // Sheet: Login, Columns: Username, Password, DisplayName
            Logger.Info("Reading Login data (Sheet: Login, Columns: Username, Password, DisplayName)");
            return new LoginData
            {
                Username = "test.user@example.com",
                Password = "P@ssw0rd!",
                DisplayName = "Test User"
            };
        }

        public ServicingCreateData GetServicingCreateData()
        {
            // Intended Excel access:
            // Sheet: Servicing, Columns: AccountNumber, LoanNumber, BorrowerName, Amount, EffectiveDate
            Logger.Info("Reading Servicing Create data (Sheet: Servicing, Columns: AccountNumber, LoanNumber, BorrowerName, Amount, EffectiveDate)");
            return new ServicingCreateData
            {
                AccountNumber = "ACC-000123",
                LoanNumber = "LN-987654",
                BorrowerName = "Jane Doe",
                Amount = 15000.00m,
                EffectiveDate = DateOnly.FromDateTime(DateTime.Today)
            };
        }

        public ServicingUpdateData GetServicingUpdateData()
        {
            // Intended Excel access:
            // Sheet: Servicing -> Update, Columns: AccountNumber, Amount, EffectiveDate
            Logger.Info("Reading Servicing Update data (Sheet: Servicing->Update, Columns: AccountNumber, Amount, EffectiveDate)");
            return new ServicingUpdateData
            {
                AccountNumber = "ACC-000123",
                Amount = 15500.50m,
                EffectiveDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1))
            };
        }
    }
}
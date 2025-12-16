using System;
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;
using AutomationFramework.Core.Models;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static List<PaymentScenario> ReadPaymentScenarios(string fileName, string sheetName)
        {
            var result = new List<PaymentScenario>();
            var folder = ConfigManager.Settings.TestDataFolder;
            var path = Path.Combine(folder, fileName);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null) return result;

                var header = sheet.GetRow(0);
                var colMap = new Dictionary<string, int>();
                for (int j = 0; j < header.LastCellNum; j++)
                {
                    colMap[header.GetCell(j)?.ToString() ?? $"Col{j}"] = j;
                }

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    var scenario = new PaymentScenario
                    {
                        TestCaseId = row.GetCell(colMap["TestCaseId"])?.ToString(),
                        Scenario = row.GetCell(colMap["Scenario"])?.ToString(),
                        LoanNumber = row.GetCell(colMap["LoanNumber"])?.ToString(),
                        PaymentDate = row.GetCell(colMap["PaymentDate"])?.ToString(),
                        State = row.GetCell(colMap["State"])?.ToString(),
                        ExpectedLateFee = bool.TryParse(row.GetCell(colMap["ExpectedLateFee"])?.ToString(), out var lateFee) ? lateFee : false
                    };
                    result.Add(scenario);
                }
            }
            return result;
        }

        public static PaymentScenario GetScenarioByTestCaseId(string fileName, string sheetName, string testCaseId)
        {
            var scenarios = ReadPaymentScenarios(fileName, sheetName);
            return scenarios.Find(s => s.TestCaseId == testCaseId);
        }
    }
}
using System.Collections.Generic;
using System.IO;
using OfficeOpenXml;

namespace AutomationFramework.Core.Utilities
{
    public static class TestDataReader
    {
        static TestDataReader()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }

        public static IDictionary<string, string> ReadRowByTestCaseId(string filePath, string sheetName, string testCaseId)
        {
            using var package = new ExcelPackage(new FileInfo(filePath));
            var sheet = package.Workbook.Worksheets[sheetName];
            var headerMap = new Dictionary<int, string>();
            int colCount = sheet.Dimension.Columns;
            int rowCount = sheet.Dimension.Rows;

            for (int c = 1; c <= colCount; c++)
            {
                var header = sheet.Cells[1, c].Text?.Trim();
                if (!string.IsNullOrWhiteSpace(header))
                    headerMap[c] = header;
            }

            int keyColIndex = -1;
            foreach (var kv in headerMap)
            {
                if (kv.Value.Equals("TestCaseId", System.StringComparison.OrdinalIgnoreCase))
                {
                    keyColIndex = kv.Key;
                    break;
                }
            }
            if (keyColIndex == -1)
                throw new System.InvalidOperationException($"Key column 'TestCaseId' not found in sheet '{sheetName}'.");

            for (int r = 2; r <= rowCount; r++)
            {
                var cellKey = sheet.Cells[r, keyColIndex].Text?.Trim();
                if (cellKey != null && cellKey.Equals(testCaseId, System.StringComparison.OrdinalIgnoreCase))
                {
                    var dict = new Dictionary<string, string>(System.StringComparer.OrdinalIgnoreCase);
                    foreach (var kv in headerMap)
                    {
                        var val = sheet.Cells[r, kv.Key].Text?.Trim() ?? string.Empty;
                        dict[kv.Value] = val;
                    }
                    return dict;
                }
            }
            throw new System.InvalidOperationException($"Row with key '{testCaseId}' not found in sheet '{sheetName}'.");
        }
    }
}
using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AutomationFramework.Core.Utilities
{
    public class ExcelReader
    {
        private readonly string _filePath;
        private readonly IWorkbook _workbook;

        public ExcelReader(string filePath)
        {
            _filePath = filePath;
            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                _workbook = new XSSFWorkbook(fs);
            }
        }

        public Dictionary<string, string> GetRowByTestCaseId(string sheetName, string testCaseId)
        {
            var sheet = _workbook.GetSheet(sheetName);
            var headerRow = sheet.GetRow(0);
            int colCount = headerRow.LastCellNum;
            var result = new Dictionary<string, string>();

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.GetCell(0).ToString() == testCaseId)
                {
                    for (int j = 0; j < colCount; j++)
                    {
                        var key = headerRow.GetCell(j).ToString();
                        var value = row.GetCell(j)?.ToString() ?? "";
                        result[key] = value;
                    }
                    break;
                }
            }
            return result;
        }
    }
}
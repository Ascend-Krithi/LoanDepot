using System.Collections.Generic;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace AutomationFramework.Core.Utilities
{
    public class ExcelReader
    {
        private readonly string _filePath;
        private IWorkbook _workbook;

        public ExcelReader(string filePath)
        {
            _filePath = filePath;
            using var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            _workbook = new XSSFWorkbook(fs);
        }

        public Dictionary<string, string> GetRowByTestCaseId(string sheetName, string testCaseId)
        {
            var sheet = _workbook.GetSheet(sheetName);
            var headerRow = sheet.GetRow(0);
            var headers = new List<string>();
            for (int i = 0; i < headerRow.LastCellNum; i++)
                headers.Add(headerRow.GetCell(i).StringCellValue);

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                if (row.GetCell(0).StringCellValue == testCaseId)
                {
                    var dict = new Dictionary<string, string>();
                    for (int j = 0; j < headers.Count; j++)
                        dict[headers[j]] = row.GetCell(j)?.ToString();
                    return dict;
                }
            }
            return null;
        }
    }
}
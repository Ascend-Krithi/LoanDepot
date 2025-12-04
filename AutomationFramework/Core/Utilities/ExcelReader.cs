using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public class ExcelReader
    {
        private readonly string _filePath;

        public ExcelReader(string filePath)
        {
            _filePath = filePath;
        }

        public Dictionary<string, string> GetRowByTestCaseId(string sheetName, string testCaseId)
        {
            using (var fs = new FileStream(_filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                var headerRow = sheet.GetRow(0);
                int colCount = headerRow.LastCellNum;
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.GetCell(0).ToString() == testCaseId)
                    {
                        var dict = new Dictionary<string, string>();
                        for (int j = 0; j < colCount; j++)
                        {
                            dict[headerRow.GetCell(j).ToString()] = row.GetCell(j)?.ToString();
                        }
                        return dict;
                    }
                }
            }
            return null;
        }
    }
}
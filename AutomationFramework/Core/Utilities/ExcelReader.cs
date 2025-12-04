using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static Dictionary<string, string> GetTestData(string filePath, string sheetName, string testCaseId)
        {
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                var headerRow = sheet.GetRow(0);
                int cellCount = headerRow.LastCellNum;
                Dictionary<int, string> headers = new();
                for (int i = 0; i < cellCount; i++)
                    headers[i] = headerRow.GetCell(i).StringCellValue;

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    var row = sheet.GetRow(i);
                    if (row == null) continue;
                    if (row.GetCell(0).StringCellValue == testCaseId)
                    {
                        var data = new Dictionary<string, string>();
                        for (int j = 0; j < cellCount; j++)
                            data[headers[j]] = row.GetCell(j)?.ToString();
                        return data;
                    }
                }
            }
            return null;
        }
    }
}
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static List<Dictionary<string, string>> ReadSheet(string filePath, string sheetName)
        {
            var result = new List<Dictionary<string, string>>();
            using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fs);
            var sheet = workbook.GetSheet(sheetName);
            if (sheet == null) return result;

            var headerRow = sheet.GetRow(0);
            var headers = new List<string>();
            for (int i = 0; i < headerRow.LastCellNum; i++)
            {
                headers.Add(headerRow.GetCell(i).ToString());
            }

            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                var row = sheet.GetRow(i);
                if (row == null) continue;
                var dict = new Dictionary<string, string>();
                for (int j = 0; j < headers.Count; j++)
                {
                    var cell = row.GetCell(j);
                    dict[headers[j]] = cell?.ToString() ?? string.Empty;
                }
                result.Add(dict);
            }
            return result;
        }
    }
}
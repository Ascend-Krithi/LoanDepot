using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            string folder = ConfigManager.Settings.TestDataFolder ?? "TestData";
            string path = Path.IsPathRooted(filePathOrName) ? filePathOrName : Path.Combine(folder, filePathOrName);

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                var row = sheet.GetRow(rowIndex);
                var cell = row.GetCell(colIndex);
                return cell?.ToString() ?? string.Empty;
            }
        }
    }
}
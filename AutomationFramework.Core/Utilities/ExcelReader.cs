using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static string ReadCell(string fileName, string sheetName, int row, int col)
        {
            string folder = ConfigManager.Settings.TestDataFolder;
            string path = Path.Combine(folder, fileName);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            IWorkbook workbook = new XSSFWorkbook(fs);
            var sheet = workbook.GetSheet(sheetName);
            var cell = sheet.GetRow(row)?.GetCell(col);
            return cell?.ToString();
        }
    }
}
using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        private static string ResolvePath(string filePathOrName)
        {
            if (File.Exists(filePathOrName))
                return filePathOrName;

            var folder = ConfigManager.Settings.TestDataFolder;
            if (string.IsNullOrWhiteSpace(folder))
                folder = Path.Combine(AppContext.BaseDirectory, "TestData");
            var path = Path.Combine(folder, filePathOrName);
            if (!File.Exists(path))
                throw new FileNotFoundException($"Excel file not found: {path}");
            return path;
        }

        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var path = ResolvePath(filePathOrName);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                    throw new ArgumentException($"Sheet '{sheetName}' not found in {filePathOrName}.");
                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    return string.Empty;
                var cell = row.GetCell(colIndex);
                return cell?.ToString() ?? string.Empty;
            }
        }
    }
}
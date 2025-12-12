using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        private static string ResolveFilePath(string filePathOrName)
        {
            if (File.Exists(filePathOrName))
                return filePathOrName;

            var folder = ConfigManager.Settings.TestDataFolder;
            if (string.IsNullOrWhiteSpace(folder))
                folder = Path.Combine(AppContext.BaseDirectory, "TestData");

            var fullPath = Path.Combine(folder, filePathOrName);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Excel file not found: {fullPath}");

            return fullPath;
        }

        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var path = ResolveFilePath(filePathOrName);
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                    throw new ArgumentException($"Sheet '{sheetName}' not found in file '{filePathOrName}'.");

                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    return null;

                var cell = row.GetCell(colIndex);
                if (cell == null)
                    return null;

                return cell.ToString();
            }
        }

        public static DateTime? GetCellAsDateTime(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var value = GetCell(filePathOrName, sheetName, rowIndex, colIndex);
            if (DateTime.TryParse(value, out var dt))
                return dt;
            return null;
        }
    }
}
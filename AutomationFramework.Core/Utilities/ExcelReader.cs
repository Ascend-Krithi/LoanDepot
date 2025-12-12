using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        private static string GetFullPath(string filePathOrName)
        {
            if (Path.IsPathRooted(filePathOrName))
                return filePathOrName;

            var folder = ConfigManager.Settings.TestDataFolder;
            if (string.IsNullOrWhiteSpace(folder))
                folder = Path.Combine(AppContext.BaseDirectory, "TestData");

            return Path.Combine(folder, filePathOrName);
        }

        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var fullPath = GetFullPath(filePathOrName);
            using (var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                    throw new ArgumentException($"Sheet '{sheetName}' not found in '{filePathOrName}'.");

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
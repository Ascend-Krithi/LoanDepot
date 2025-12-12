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
            if (File.Exists(filePathOrName))
                return filePathOrName;

            var folder = ConfigManager.Settings.TestDataFolder ?? "TestData";
            var baseDir = AppContext.BaseDirectory;
            var fullPath = Path.Combine(baseDir, folder, filePathOrName);
            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Excel file not found: {fullPath}");
            return fullPath;
        }

        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var path = GetFullPath(filePathOrName);
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IWorkbook workbook = new XSSFWorkbook(fs);
            var sheet = workbook.GetSheet(sheetName) ?? throw new ArgumentException($"Sheet not found: {sheetName}");
            var row = sheet.GetRow(rowIndex) ?? throw new ArgumentException($"Row not found: {rowIndex}");
            var cell = row.GetCell(colIndex) ?? throw new ArgumentException($"Cell not found: {colIndex}");
            return cell.ToString();
        }
    }
}
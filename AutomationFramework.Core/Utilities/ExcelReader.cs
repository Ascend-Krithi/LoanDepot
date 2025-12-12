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
            else if (!Path.IsPathRooted(folder))
                folder = Path.Combine(AppContext.BaseDirectory, folder);

            var fullPath = Path.Combine(folder, filePathOrName);
            return fullPath;
        }

        public static string GetCell(string filePathOrName, string sheetName, int rowIndex, int colIndex)
        {
            var filePath = ResolveFilePath(filePathOrName);
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                IWorkbook workbook = new XSSFWorkbook(fs);
                var sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                    throw new ArgumentException($"Sheet '{sheetName}' not found in file '{filePath}'.");

                var row = sheet.GetRow(rowIndex);
                if (row == null)
                    return null;

                var cell = row.GetCell(colIndex);
                if (cell == null)
                    return null;

                return cell.ToString();
            }
        }
    }
}
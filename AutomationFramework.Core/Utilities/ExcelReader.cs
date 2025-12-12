using AutomationFramework.Core.Configuration;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        private static string GetFullFilePath(string fileName)
        {
            var testDataFolder = ConfigManager.Settings.TestDataFolder ?? "TestData";
            var baseDirectory = AppContext.BaseDirectory;
            var filePath = Path.Combine(baseDirectory, testDataFolder, fileName);

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"Excel file not found at path: {filePath}");
            }
            return filePath;
        }

        public static string GetCell(string fileName, string sheetName, int rowIndex, int colIndex)
        {
            var filePath = GetFullFilePath(fileName);
            IWorkbook workbook;

            using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = new XSSFWorkbook(fileStream);
            }

            var sheet = workbook.GetSheet(sheetName);
            if (sheet == null)
            {
                throw new ArgumentException($"Sheet '{sheetName}' not found in file '{fileName}'.");
            }

            var row = sheet.GetRow(rowIndex);
            var cell = row?.GetCell(colIndex);

            return cell?.ToString() ?? string.Empty;
        }
    }
}
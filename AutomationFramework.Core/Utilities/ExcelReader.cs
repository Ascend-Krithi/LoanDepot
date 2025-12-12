// AutomationFramework.Core/Utilities/ExcelReader.cs
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
            if (Path.IsPathRooted(fileName))
            {
                return fileName;
            }

            var testDataFolder = ConfigManager.Settings.TestDataFolder ?? "TestData";
            var basePath = AppContext.BaseDirectory;
            return Path.Combine(basePath, testDataFolder, fileName);
        }

        public static string GetCell(string fileName, string sheetName, int rowIndex, int colIndex)
        {
            var fullPath = GetFullFilePath(fileName);
            if (!File.Exists(fullPath))
            {
                throw new FileNotFoundException($"Excel file not found at path: {fullPath}");
            }

            using (var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(fileStream);
                ISheet sheet = workbook.GetSheet(sheetName);
                if (sheet == null)
                {
                    throw new ArgumentException($"Sheet '{sheetName}' not found in file '{fileName}'.");
                }

                IRow row = sheet.GetRow(rowIndex);
                if (row == null) return string.Empty;

                ICell cell = row.GetCell(colIndex);
                if (cell == null) return string.Empty;

                return cell.ToString() ?? string.Empty;
            }
        }
    }
}
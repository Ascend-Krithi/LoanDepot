// NuGet Packages: ExcelDataReader, ExcelDataReader.DataSet, System.Text.Encoding.CodePages
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;

namespace AutomationFramework.Core.Utilities
{
    /// <summary>
    /// A utility for reading data from Excel files (.xls and .xlsx).
    /// Requires the System.Text.Encoding.CodePages package for .NET Core/.NET 5+.
    /// </summary>
    public static class ExcelReader
    {
        static ExcelReader()
        {
            // Required for ExcelDataReader on .NET Core/.NET 5+
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Reads data from an Excel sheet and returns it as a list of dictionaries.
        /// Each dictionary represents a row, with column headers as keys.
        /// </summary>
        /// <param name="filePath">The full path to the Excel file.</param>
        /// <param name="sheetName">The name of the sheet to read from.</param>
        /// <returns>A list of dictionaries, where each dictionary is a row of data.</returns>
        public static List<Dictionary<string, string>> ReadSheet(string filePath, string sheetName)
        {
            var data = new List<Dictionary<string, string>>();

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true // Use the first row as column headers
                        }
                    });

                    if (!result.Tables.Contains(sheetName))
                    {
                        throw new ArgumentException($"Sheet '{sheetName}' not found in the Excel file.");
                    }

                    DataTable table = result.Tables[sheetName];
                    if (table == null) return data;

                    foreach (DataRow row in table.Rows)
                    {
                        var rowData = new Dictionary<string, string>();
                        foreach (DataColumn col in table.Columns)
                        {
                            rowData[col.ColumnName] = row[col].ToString();
                        }
                        data.Add(rowData);
                    }
                }
            }
            return data;
        }
    }
}
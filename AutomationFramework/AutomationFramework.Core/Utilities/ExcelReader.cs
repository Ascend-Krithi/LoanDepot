using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;

namespace AutomationFramework.Core.Utilities
{
    public class ExcelReader
    {
        public static IEnumerable<object[]> ReadData(string filePath, string sheetName)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (_) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });

                    DataTable table = result.Tables[sheetName];
                    if (table == null)
                    {
                        throw new KeyNotFoundException($"Sheet '{sheetName}' not found in the Excel file.");
                    }

                    foreach (DataRow row in table.Rows)
                    {
                        yield return new object[] { row };
                    }
                }
            }
        }
    }
}
using System.Data;
using System.IO;
using ExcelDataReader;
using System.Collections.Generic;

namespace AutomationFramework.Core.Utilities
{
    public static class TestDataReader
    {
        public static Dictionary<string, string> GetTestData(string testCaseId, string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[0];
                var data = new Dictionary<string, string>();
                for (int i = 1; i < table.Rows.Count; i++)
                {
                    if (table.Rows[i][0].ToString() == testCaseId)
                    {
                        for (int j = 0; j < table.Columns.Count; j++)
                        {
                            data[table.Columns[j].ColumnName] = table.Rows[i][j].ToString();
                        }
                        break;
                    }
                }
                return data;
            }
        }
    }
}
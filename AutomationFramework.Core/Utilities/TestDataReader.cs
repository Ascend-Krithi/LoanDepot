using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;

namespace AutomationFramework.Core.Utilities
{
    public class TestDataReader
    {
        private readonly Dictionary<string, Dictionary<string, string>> _testData = new();

        public TestDataReader(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            var table = result.Tables[0];

            var headers = new List<string>();
            for (int i = 0; i < table.Columns.Count; i++)
                headers.Add(table.Rows[0][i].ToString());

            for (int i = 1; i < table.Rows.Count; i++)
            {
                var row = table.Rows[i];
                var tcId = row[0].ToString();
                var dict = new Dictionary<string, string>();
                for (int j = 0; j < headers.Count; j++)
                    dict[headers[j]] = row[j].ToString();
                _testData[tcId] = dict;
            }
        }

        public Dictionary<string, string> GetDataByTestCaseId(string testCaseId)
        {
            return _testData.ContainsKey(testCaseId) ? _testData[testCaseId] : null;
        }
    }
}
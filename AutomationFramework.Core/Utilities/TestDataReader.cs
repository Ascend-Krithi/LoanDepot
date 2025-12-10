using System.Collections.Generic;
using System.Data;
using System.IO;
using ExcelDataReader;

namespace AutomationFramework.Core.Utilities
{
    public class TestDataReader
    {
        private Dictionary<string, Dictionary<string, string>> testData;

        public TestDataReader(string filePath)
        {
            testData = new Dictionary<string, Dictionary<string, string>>();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                var result = reader.AsDataSet();
                var table = result.Tables[0];

                var columns = new List<string>();
                for (int i = 0; i < table.Columns.Count; i++)
                    columns.Add(table.Rows[0][i].ToString());

                for (int i = 1; i < table.Rows.Count; i++)
                {
                    var row = table.Rows[i];
                    var rowDict = new Dictionary<string, string>();
                    for (int j = 0; j < columns.Count; j++)
                        rowDict[columns[j]] = row[j].ToString();

                    string testCaseId = rowDict["TestCaseId"];
                    testData[testCaseId] = rowDict;
                }
            }
        }

        public Dictionary<string, string> GetDataByTestCaseId(string testCaseId)
        {
            return testData.ContainsKey(testCaseId) ? testData[testCaseId] : null;
        }
    }
}
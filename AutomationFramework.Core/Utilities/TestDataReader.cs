using System.Collections.Generic;
using System.Data;
using System.IO;
using ClosedXML.Excel;

namespace AutomationFramework.Core.Utilities
{
    public class TestDataReader
    {
        private readonly Dictionary<string, Dictionary<string, string>> _testData = new();

        public TestDataReader(string filePath)
        {
            using (var workbook = new XLWorkbook(filePath))
            {
                var worksheet = workbook.Worksheet(1);
                var rows = worksheet.RangeUsed().RowsUsed();
                var headers = new List<string>();

                foreach (var cell in rows.First().Cells())
                {
                    headers.Add(cell.Value.ToString());
                }

                foreach (var row in rows.Skip(1))
                {
                    var testCaseId = row.Cell(1).Value.ToString();
                    var data = new Dictionary<string, string>();
                    for (int i = 0; i < headers.Count; i++)
                    {
                        data[headers[i]] = row.Cell(i + 1).Value.ToString();
                    }
                    _testData[testCaseId] = data;
                }
            }
        }

        public Dictionary<string, string> GetDataByTestCaseId(string testCaseId)
        {
            return _testData.ContainsKey(testCaseId) ? _testData[testCaseId] : null;
        }
    }
}
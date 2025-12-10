using System.Data;
using System.IO;
using ExcelDataReader;

namespace AutomationFramework.Core.Utilities
{
    public static class TestDataReader
    {
        public static DataRow GetTestData(string testCaseId)
        {
            using (var stream = File.Open("AutomationFramework.Tests/TestData/testdata.xlsx", FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    var result = reader.AsDataSet();
                    var table = result.Tables[0];
                    foreach (DataRow row in table.Rows)
                    {
                        if (row[0].ToString() == testCaseId)
                            return row;
                    }
                }
            }
            return null;
        }
    }
}
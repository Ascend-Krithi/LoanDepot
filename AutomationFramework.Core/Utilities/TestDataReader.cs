using System.Data;
using System.IO;
using ExcelDataReader;

namespace AutomationFramework.Core.Utilities
{
    public static class TestDataReader
    {
        public static DataTable ReadTestData(string filePath)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
            using var reader = ExcelReaderFactory.CreateReader(stream);
            var result = reader.AsDataSet();
            return result.Tables[0];
        }
    }
}
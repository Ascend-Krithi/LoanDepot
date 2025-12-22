using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace WebAutomation.Core.Utilities;

public sealed class ExcelReader
{
    public static Dictionary<string, string> GetRow(
        string filePath,
        string sheetName,
        string keyColumn,
        string keyValue)
    {
        using var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        var workbook = new XSSFWorkbook(fs);

        var sheet = workbook.GetSheet(sheetName)
            ?? throw new InvalidOperationException($"Sheet '{sheetName}' not found.");

        // Read header row
        var headerRow = sheet.GetRow(0)
            ?? throw new InvalidOperationException("Header row is missing.");

        var headers = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        for (int i = 0; i < headerRow.LastCellNum; i++)
        {
            var headerName = headerRow.GetCell(i)?.ToString()?.Trim();
            if (!string.IsNullOrEmpty(headerName))
            {
                headers[headerName] = i;
            }
        }

        if (!headers.ContainsKey(keyColumn))
            throw new InvalidOperationException(
                $"Key column '{keyColumn}' not found in Excel header.");

        // Find matching row
        for (int r = 1; r <= sheet.LastRowNum; r++)
        {
            var row = sheet.GetRow(r);
            if (row == null) continue;

            var cellValue = row.GetCell(headers[keyColumn])?.ToString()?.Trim();
            if (string.Equals(cellValue, keyValue, StringComparison.OrdinalIgnoreCase))
            {
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

                foreach (var header in headers)
                {
                    var value = row.GetCell(header.Value)?.ToString()?.Trim() ?? "";
                    result[header.Key] = value;
                }

                return result;
            }
        }

        throw new InvalidOperationException(
            $"Test case '{keyValue}' not found in sheet '{sheetName}'.");
    }
}
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data;

namespace AutomationFramework.Core.Utilities
{
    public static class ExcelReader
    {
        public static DataTable ReadSheet(string filePath, string sheetName)
        {
            var table = new DataTable(sheetName);
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                IWorkbook workbook = new XSSFWorkbook(stream);
                ISheet sheet = workbook.GetSheet(sheetName);

                if (sheet == null)
                {
                    throw new ArgumentException($"Sheet '{sheetName}' not found in '{filePath}'.");
                }

                IRow headerRow = sheet.GetRow(0);
                if (headerRow == null) return table;

                // Create columns
                foreach (ICell headerCell in headerRow)
                {
                    table.Columns.Add(headerCell.ToString());
                }

                // Read data rows
                for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
                {
                    IRow? row = sheet.GetRow(i);
                    if (row == null) continue;

                    DataRow dataRow = table.NewRow();
                    for (int j = 0; j < headerRow.LastCellNum; j++)
                    {
                        ICell? cell = row.GetCell(j);
                        dataRow[j] = cell?.ToString() ?? string.Empty;
                    }
                    table.Rows.Add(dataRow);
                }
            }
            return table;
        }
    }
}
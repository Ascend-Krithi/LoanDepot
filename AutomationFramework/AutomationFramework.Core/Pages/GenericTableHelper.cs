using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper
    {
        private readonly IWebElement _tableElement;
        private List<string> _headers;

        public GenericTableHelper(IWebElement tableElement)
        {
            _tableElement = tableElement;
            _headers = _tableElement.FindElements(By.TagName("th"))
                                    .Select(header => header.Text.Trim())
                                    .ToList();
        }

        public int GetRowCount()
        {
            return _tableElement.FindElements(By.CssSelector("tbody tr")).Count;
        }

        public int GetColumnCount()
        {
            return _headers.Count;
        }

        public string GetCellValue(int row, int column)
        {
            var rows = _tableElement.FindElements(By.CssSelector("tbody tr"));
            if (row >= rows.Count)
                throw new ArgumentOutOfRangeException(nameof(row), "Row index is out of bounds.");

            var cells = rows[row].FindElements(By.TagName("td"));
            if (column >= cells.Count)
                throw new ArgumentOutOfRangeException(nameof(column), "Column index is out of bounds.");

            return cells[column].Text;
        }

        public string GetCellValue(int row, string columnName)
        {
            int columnIndex = _headers.IndexOf(columnName);
            if (columnIndex == -1)
                throw new KeyNotFoundException($"Column with name '{columnName}' not found.");

            return GetCellValue(row, columnIndex);
        }

        public IWebElement GetCellElement(int row, int column)
        {
            var rows = _tableElement.FindElements(By.CssSelector("tbody tr"));
            var cells = rows[row].FindElements(By.TagName("td"));
            return cells[column];
        }

        public Dictionary<string, string> GetRowData(int rowIndex)
        {
            var rowData = new Dictionary<string, string>();
            var rows = _tableElement.FindElements(By.CssSelector("tbody tr"));
            var cells = rows[rowIndex].FindElements(By.TagName("td"));

            for (int i = 0; i < _headers.Count; i++)
            {
                rowData[_headers[i]] = cells[i].Text;
            }
            return rowData;
        }
    }
}
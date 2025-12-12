using OpenQA.Selenium;
using System.Collections.Generic;

namespace AutomationFramework.Core.Pages
{
    public class GenericTableHelper
    {
        private readonly IWebDriver _driver;
        private readonly By _table;

        public GenericTableHelper(IWebDriver driver, By table)
        {
            _driver = driver;
            _table = table;
        }

        public List<Dictionary<string, string>> GetTableData()
        {
            var tableElement = _driver.FindElement(_table);
            var headers = tableElement.FindElements(By.TagName("th"));
            var rows = tableElement.FindElements(By.TagName("tr"));
            var data = new List<Dictionary<string, string>>();

            for (int i = 1; i < rows.Count; i++)
            {
                var cells = rows[i].FindElements(By.TagName("td"));
                var rowDict = new Dictionary<string, string>();
                for (int j = 0; j < headers.Count && j < cells.Count; j++)
                {
                    rowDict[headers[j].Text] = cells[j].Text;
                }
                data.Add(rowDict);
            }
            return data;
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;

namespace AutomationFramework.Core.Utilities
{
    // Lightweight CSV-style reader for demo purposes. Place CSV-like content inside .xlsx placeholder files.
    public static class ExcelReader
    {
        public static IEnumerable<Dictionary<string, string>> Read(string filePath)
        {
            var result = new List<Dictionary<string, string>>();
            if (!File.Exists(filePath)) return result;
            var lines = File.ReadAllLines(filePath);
            if (lines.Length == 0) return result;
            var headers = lines[0].Split(',');
            for (int i = 1; i < lines.Length; i++)
            {
                if (string.IsNullOrWhiteSpace(lines[i])) continue;
                var vals = lines[i].Split;
                var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                for (int h = 0; h < headers.Length && h < vals.Length; h++)
                {
                    dict[headers[h].Trim()] = vals[h].Trim();
                }
                result.Add(dict);
            }
            return result;
        }
    }
}
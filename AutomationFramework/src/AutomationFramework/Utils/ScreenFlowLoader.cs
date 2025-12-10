using AutomationFramework.Core;

namespace AutomationFramework.Utils
{
    public class ScreenFlow
    {
        public IReadOnlyList<string> Steps { get; init; } = Array.Empty<string>();

        public int IndexOf(string stepName)
        {
            for (var i = 0; i < Steps.Count; i++)
            {
                if (string.Equals(Steps[i], stepName, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
            return -1;
        }

        public override string ToString() => string.Join(" -> ", Steps);
    }

    public static class ScreenFlowLoader
    {
        // Parses a simple Screen Flow.txt with lines like:
        // Screen Flow:
        // 1. Login
        // 2. Dashboard
        // 3. Servicing Listing
        // 4. Servicing Create/Edit
        public static ScreenFlow Load(string path)
        {
            if (!File.Exists(path))
            {
                Logger.Warn($"Screen Flow file not found at {path}. Using default order.");
                return new ScreenFlow
                {
                    Steps = new List<string>
                    {
                        "Login",
                        "Dashboard",
                        "Servicing Listing",
                        "Servicing Create/Edit"
                    }
                };
            }

            var lines = File.ReadAllLines(path);
            var steps = new List<string>();

            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (string.IsNullOrWhiteSpace(line)) continue;
                if (line.StartsWith("Screen Flow", StringComparison.OrdinalIgnoreCase)) continue;

                // Remove leading numbering "1. " etc.
                var normalized = line;
                var dotIdx = normalized.IndexOf('.');
                if (dotIdx >= 0 && dotIdx <= 2)
                {
                    normalized = normalized[(dotIdx + 1)..].Trim();
                }
                steps.Add(normalized);
            }

            var flow = new ScreenFlow { Steps = steps };
            Logger.Info($"Loaded Screen Flow: {flow}");
            return flow;
        }
    }
}
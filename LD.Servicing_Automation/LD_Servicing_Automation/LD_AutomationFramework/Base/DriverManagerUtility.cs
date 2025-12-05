using System.Collections.Generic;
using System.Threading.Tasks;
using WebDriverManager;
using WebDriverManager.DriverConfigs;
using WebDriverManager.DriverConfigs.Impl;
using WebDriverManager.Helpers;

namespace LD_AutomationFramework
{
    public class DriverManagerUtility
    {
        private static readonly object Object = new object();

        /// <summary>
        /// Method to download Chrome driver as per version
        /// </summary>
        /// <param name="version">VersionResolveStrategy.MatchingBrowser</param>
        /// <param name="arc">Architecture.X32</param>
        public void DownloadChromeDriver(string version, Architecture arc)
        {
            lock (Object)
            {
                new DriverManager().SetUpDriver(new ChromeConfig(), version, arc);
            }
        }

        /// <summary>
        /// Method to download Edge driver as per version
        /// </summary>
        /// <param name="version">VersionResolveStrategy.MatchingBrowser</param>
        /// <param name="arc">Architecture.X32</param>
        public void DownloadEdgeDriver(string version, Architecture arc)
        {
            lock (Object)
            {
                new DriverManager().SetUpDriver(new EdgeConfig(), version, arc);
            }
        }
    }
}

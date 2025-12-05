using LD_AutomationFramework.Config;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace LD_CustomerPortal.Config
{
    [TestClass]
    public class ConfigurationSetup
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [AssemblyInitialize]
        public static void OneTimeSetup(TestContext context)
        {
            ConfigReader.ConfigValueMappings();
        }
    }
}

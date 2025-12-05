using System;
using TechTalk.SpecFlow;
using AutomationFramework.Core.Configuration;

namespace AutomationFramework.Tests.Hooks
{
    [Binding]
    public class TestHooks
    {
        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            ConfigManager.Initialize();
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            // Placeholder for report finalization
        }
    }
}
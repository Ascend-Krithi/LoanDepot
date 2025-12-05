using AventStack.ExtentReports;
using LD_AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Threading;

namespace LD_AutomationFramework.Base
{
    public class ReportLogger
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IWebDriver _driver = null;

        public ReportLogger(IWebDriver _driver)
        {
            this._driver = _driver;
        }

        /// <summary>
        /// Method to write to extent report
        /// </summary>
        /// <param name="IsPass">true/false</param>
        /// <param name="extentTest">Extent test variable</param>
        /// <param name="message">Pass or fail message</param>
        /// <param name="isScreenshotRequiredForPass">true/false</param>
        public void ReportWriter(bool IsPass, ExtentTest extentTest, string message, bool isScreenshotRequiredForPass)
        {
            bool unknownExc = false;
            string screenShotName = string.Empty, imagePath = string.Empty, reportImagePath = string.Empty;
            Screenshot screenshot = null;
            Bitmap cp = null;

            try
            {
                string appPath = UtilAdditions.GetRootDirectory();
                if (_driver != null && (!IsPass || isScreenshotRequiredForPass))
                {
                    try
                    {
                        screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Exception occurred while taking screenshot: " + ex.Message);
                        unknownExc = true;
                        log.Error("Unknown exception identified while taking screenshot: " + ex.Message);
                        extentTest.Log(Status.Info, "Unknown exception identified while taking screenshot. Please check logs for more details");
                    }

                    if (!unknownExc)
                    {
                        //To save screenshot in folder - Currently not being used but kept for reference
                        //screenShotName = DateTime.Now.ToString("dd_MMMM_hh_mm_ss_fffff_tt");
                        //imagePath = Path.Combine(appPath, ConfigSettings.TestReportFolder, ConfigSettings.CurrentReportFolder, ConfigSettings.ScreenshotFolder, screenShotName + ".jpg");
                        //reportImagePath = @".\" + ConfigSettings.ScreenshotFolder + @"\" + screenShotName + ".jpg";

                        if (screenshot != null)
                        {
                            string screenshotAsBase64String = screenshot.AsBase64EncodedString;
                            if (isScreenshotRequiredForPass)
                                extentTest.Log(Status.Pass, message + "\nScreenshot: ", MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshotAsBase64String).Build());
                            else
                                extentTest.Log(Status.Fail, message + "\nScreenshot: ", MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshotAsBase64String).Build());
                            //To add screenshots at the top of extent reports - Currently not being used but kept for reference
                            //extentTest.AddScreenCaptureFromBase64String(screenshotAsBase64String);
                        }
                        else
                        {
                            extentTest.Log(Status.Fail, message + " : Could not save the screenshot since it's null.");
                        }
                    }
                    else
                    {
                        extentTest.Log(Status.Fail, message);
                    }
                }
                else
                {
                    extentTest.Log(Status.Pass, message);
                }
            }
            catch (Exception e)
            {
                log.Error("Failed while reporting in Extent Test Report: " + e.Message);
                extentTest.Log(Status.Fail, "Failed while reporting in Extent Test Report. Please check logs for more information: '" + message + "'.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (cp != null)
                    cp.Dispose();
            }
        }

        /// <summary>
        /// Method to take screenshot and add it to extent report with message
        /// </summary>
        /// <param name="extentTest">Extent report variable</param>
        /// <param name="message">The description of the screenshot</param>
        /// <param name="isFullPage">If true, capture full page screenshot, otherwise capture normal viewport screenshot</param>
        public void TakeScreenshot(ExtentTest extentTest, string message, bool isFullPage = false)
        {
            bool unknownExc = false;
            string screenShotName = string.Empty, imagePath = string.Empty, reportImagePath = string.Empty;
            Screenshot screenshot = null;
            Bitmap cp = null;

            try
            {
                string appPath = UtilAdditions.GetRootDirectory();

                try
                {
                    if (isFullPage)  // Check if full page screenshot is requested
                    {
                        screenshot = CaptureFullPageScreenshot();  // Capture full page screenshot
                    }
                    else
                    {
                        screenshot = ((ITakesScreenshot)_driver).GetScreenshot();  // Capture normal screenshot (viewport size)
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Exception occurred while taking screenshot: " + ex.Message);
                    unknownExc = true;
                    log.Error("Unknown exception identified while taking screenshot: " + ex.Message);
                    extentTest.Log(Status.Info, "Unknown exception identified while taking screenshot. Please check logs for more details");
                }

                if (!unknownExc)
                {
                    // Code to save screenshot in folder - Currently not being used but kept for reference
                    // screenShotName = DateTime.Now.ToString("dd_MMMM_hh_mm_ss_fffff_tt");
                    // imagePath = Path.Combine(appPath, ConfigSettings.TestReportFolder, ConfigSettings.CurrentReportFolder, ConfigSettings.ScreenshotFolder, screenShotName + ".jpg");
                    // reportImagePath = @".\" + ConfigSettings.ScreenshotFolder + @"\" + screenShotName + ".jpg";

                    if (screenshot != null)
                    {
                        string screenshotAsBase64String = screenshot.AsBase64EncodedString;  // Accessing the property directly
                        extentTest.Log(Status.Info, "\nScreenshot: ", MediaEntityBuilder.CreateScreenCaptureFromBase64String(screenshotAsBase64String, message).Build());
                        // Code to add screenshot preview in extent report - Currently not being used but kept for reference
                        // extentTest.AddScreenCaptureFromBase64String(screenshotAsBase64String);
                    }
                    else
                    {
                        extentTest.Log(Status.Fail, message + " : Could not save the screenshot since it's null.");
                    }
                }
                else
                {
                    extentTest.Log(Status.Fail, message);
                }
            }
            catch (Exception e)
            {
                log.Error("Failed while reporting in Extent Test Report: " + e.Message);
                extentTest.Log(Status.Fail, "Failed while reporting in Extent Test Report. Please check logs for more information: '" + message + "'.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                if (cp != null)
                    cp.Dispose();
            }
        }

        /// <summary>
        /// Method to capture full page screenshot by scrolling and stitching screenshots.
        /// </summary>
        private Screenshot CaptureFullPageScreenshot()
        {
            // Cast the driver to IJavaScriptExecutor to access ExecuteScript
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)_driver;

            // Get the total page height and viewport height using ExecuteScript
            var pageHeight = (long)jsExecutor.ExecuteScript("return document.body.scrollHeight");
            var viewportHeight = (long)jsExecutor.ExecuteScript("return window.innerHeight");

            // Capture full page screenshot by scrolling and stitching
            Bitmap fullScreenshot = new Bitmap((int)_driver.Manage().Window.Size.Width, (int)pageHeight);

            using (Graphics g = Graphics.FromImage(fullScreenshot))
            {
                int scrollPosition = 0;

                while (scrollPosition < pageHeight)
                {
                    // Scroll to the current position and capture screenshot
                    jsExecutor.ExecuteScript($"window.scrollTo(0, {scrollPosition});");
                    Thread.Sleep(1000); // Ensure page is loaded before capturing

                    Screenshot screenshot = ((ITakesScreenshot)_driver).GetScreenshot();
                    string screenshotAsBase64String = screenshot.AsBase64EncodedString;  // Accessing the property correctly

                    // Convert the base64 string into an image and add to the full screenshot
                    var currentScreenshot = new Bitmap(new MemoryStream(Convert.FromBase64String(screenshotAsBase64String)));

                    // Draw the screenshot to the full screenshot bitmap
                    g.DrawImage(currentScreenshot, 0, scrollPosition);

                    // Increment the scroll position for next screenshot
                    scrollPosition += (int)viewportHeight;

                    // Optionally add more delay if necessary
                    Thread.Sleep(1000);
                }
            }

            // Return the full-page screenshot as a Screenshot object
            using (MemoryStream memoryStream = new MemoryStream())
            {
                fullScreenshot.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                string fullScreenshotAsBase64 = Convert.ToBase64String(memoryStream.ToArray());
                return new Screenshot(fullScreenshotAsBase64);
            }
        }

    }
}

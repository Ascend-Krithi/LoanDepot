// NuGet Packages: Selenium.WebDriver
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using AutomationFramework.Core.Utilities;

namespace AutomationFramework.Core.SelfHealing
{
    /// <summary>
    /// Analyzes the DOM to find the best match for an element whose original locator has failed.
    /// It uses a scoring algorithm based on a previously captured LocatorSnapshot.
    /// </summary>
    public class DomAnalyzer
    {
        private readonly IWebDriver _driver;

        public DomAnalyzer(IWebDriver driver)
        {
            _driver = driver;
        }

        /// <summary>
        /// Attempts to find a new locator for an element that could not be found with its original locator.
        /// </summary>
        /// <param name="snapshot">The last known good state of the element.</param>
        /// <returns>A new 'By' locator if a suitable match is found; otherwise, null.</returns>
        public By FindNewLocator(LocatorSnapshot snapshot)
        {
            Logger.Log($"Self-healing: Attempting to find new locator for original: {snapshot.OriginalLocator}");

            // Find potential candidates based on tag name
            var candidates = _driver.FindElements(By.TagName(snapshot.TagName));
            if (!candidates.Any())
            {
                Logger.Log($"Self-healing: No elements found with tag '{snapshot.TagName}'. Healing failed.");
                return null;
            }

            var bestMatch = candidates
                .Select(candidate => new
                {
                    Element = candidate,
                    Score = CalculateMatchScore(candidate, snapshot)
                })
                .Where(x => x.Score > 0.5) // Set a minimum score threshold
                .OrderByDescending(x => x.Score)
                .FirstOrDefault();

            if (bestMatch != null)
            {
                // Generate a new, reliable locator for the best match (preferring unique attributes)
                var newLocator = GenerateStableLocator(bestMatch.Element);
                Logger.Log($"Self-healing: Found a likely match with score {bestMatch.Score:P2}. New locator: {newLocator}");
                return newLocator;
            }

            Logger.Log("Self-healing: No suitable replacement element found.");
            return null;
        }

        private double CalculateMatchScore(IWebElement candidate, LocatorSnapshot snapshot)
        {
            double totalScore = 0;
            int attributeCount = 0;

            // Score based on attributes
            foreach (var attribute in snapshot.Attributes)
            {
                var candidateAttrValue = candidate.GetAttribute(attribute.Key);
                if (!string.IsNullOrEmpty(candidateAttrValue))
                {
                    totalScore += (candidateAttrValue == attribute.Value) ? 1.0 : 0.5; // Exact match is better
                }
                attributeCount++;
            }

            // Score based on text content
            if (!string.IsNullOrEmpty(snapshot.Text))
            {
                totalScore += (candidate.Text == snapshot.Text) ? 1.5 : 0; // Text is a strong indicator
                attributeCount++;
            }

            return attributeCount > 0 ? totalScore / attributeCount : 0;
        }

        private By GenerateStableLocator(IWebElement element)
        {
            // Prioritize unique and stable attributes for the new locator
            var id = element.GetAttribute("id");
            if (!string.IsNullOrEmpty(id))
            {
                return By.Id(id);
            }

            var dataTestId = element.GetAttribute("data-testid");
            if (!string.IsNullOrEmpty(dataTestId))
            {
                return By.CssSelector($"[{dataTestId}='{element.GetAttribute(dataTestId)}']");
            }

            var name = element.GetAttribute("name");
            if (!string.IsNullOrEmpty(name))
            {
                return By.Name(name);
            }

            // Fallback to a more complex CSS selector if no simple unique attribute is found
            var tagName = element.TagName;
            var className = element.GetAttribute("class")?.Trim().Replace(" ", ".");
            if (!string.IsNullOrEmpty(className))
            {
                return By.CssSelector($"{tagName}.{className}");
            }

            // As a last resort, use text if it's unique enough
            if (!string.IsNullOrEmpty(element.Text))
            {
                return By.XPath($"//{tagName}[normalize-space()='{element.Text}']");
            }

            // If all else fails, we can't generate a stable locator
            return null;
        }
    }
}
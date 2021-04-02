using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace FileManagement.SeleniumXunit.Utilities
{
    internal static class WebDriverExtensions
    {
        //you should set default time in config
        private const int DefaultWaitTime = 10;
        private static readonly TimeSpan DefaultWaitTimeSpan = TimeSpan.FromSeconds(DefaultWaitTime);

        public static IWait<IWebDriver> Wait(this IWebDriver driver) => Wait(driver, DefaultWaitTimeSpan);
        public static IWait<IWebDriver> Wait(this IWebDriver driver, int waitTime) => Wait(driver, TimeSpan.FromSeconds(waitTime));
        public static IWait<IWebDriver> Wait(this IWebDriver driver, TimeSpan waitTimeSpan) => new WebDriverWait(driver, waitTimeSpan);

        private static IWebElement WaitUntilFindElementForPageLoadCheck(this IWebDriver driver) => driver.WaitUntilFindElement(By.XPath("html"));


        public static IWebElement WaitUntilFindElement(this IWebDriver driver, By locator)
        {
            driver.Wait().Until(condition => condition.FindElement(locator));
            return driver.FindElement(locator);
        }
        
        public static IWebElement WaitUntilFindElementCustomCondition(this IWebDriver driver, By locator, Func<IWebDriver,IWebElement> condition)
        {
            driver.Wait().Until(condition);
            return driver.FindElement(locator);
        }

        public static IWebElement WaitUntilInitialPageLoad(this IWebDriver driver, string titleOnNewPage)
        {
            driver.Wait().Until(c => c.Title == titleOnNewPage);
            return driver.WaitUntilFindElementForPageLoadCheck();
        }

        public static IWebElement WaitUntilPageLoad(this IWebDriver driver, string titleOnNewPage)
        {
            driver.Wait().Until(c=>c.Title==titleOnNewPage);
            return driver.WaitUntilFindElementForPageLoadCheck();
        }

        public static void ScrollIntoView(this IWebDriver driver, IWebElement element)
        {
            ScrollIntoView((IJavaScriptExecutor)driver, element);
        }

        private static void ScrollIntoView(IJavaScriptExecutor driver, IWebElement element)
        {
            driver.ExecuteScript("arguments[0].scrollIntoView(true);", element);
        }

        public static void Quit (this IWebDriver driver)
        {
            driver.Quit();
        }
    }
}

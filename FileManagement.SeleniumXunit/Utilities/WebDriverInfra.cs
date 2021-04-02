using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Opera;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagement.SeleniumXunit.Utilities
{
    internal static class WebDriverInfra
    {
        public static IWebDriver CreateBrowser(BrowserType browserType)
        {
            //you should add these drivers to the PATH if you want to use this method also you can add driver path string to this method.
            switch (browserType)
            {
                case BrowserType.Chrome:
                    return new ChromeDriver();
                case BrowserType.Firefox:
                    //firefox works very slow when finding elements. If you make this setup it will work faster.
                    FirefoxDriverService service = FirefoxDriverService.CreateDefaultService();
                    service.Host = "::1";
                    return new FirefoxDriver(service);
                case BrowserType.Edge:
                    return new EdgeDriver();
                case BrowserType.Opera:
                    return new OperaDriver();
                default:
                    throw new ArgumentOutOfRangeException(nameof(browserType),browserType,null);
            }
        }
    }
}

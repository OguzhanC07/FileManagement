using FileManagement.SeleniumXunit.GenerateData;
using FileManagement.SeleniumXunit.Models;
using FileManagement.SeleniumXunit.Utilities;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FileManagement.SeleniumXunit
{

    public class AuthTest
    {
        private const string TestUrl = "http://localhost:3000/";

        [Theory]
        [MemberData(nameof(BrowserSuccessData))]
        public void LoginTest_WithDifferentBrowsers(BrowserType browserType, User user)
        {
            using var driver = WebDriverInfra.CreateBrowser(browserType);
            driver.Navigate().GoToUrl(TestUrl);
            var loadingCheck = driver.WaitUntilFindElement(By.XPath("//div[@class='ui centered one column grid container']"));

            driver.FindElement(By.Id("username")).SendKeys(user.Username);
            driver.FindElement(By.Id("password")).SendKeys(user.Password);
            driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            var element = driver.WaitUntilFindElement(By.XPath("//div//div[@class='links-con']"));
            Assert.NotNull(element);
        }

        [Theory]
        [MemberData(nameof(BrowserSuccessData))]
        public void LogOutTest_ShouldEnterLoginScreen_IfLogoutDone(BrowserType browserType,User user)
        {
            using var driver = WebDriverInfra.CreateBrowser(browserType);
            driver.Navigate().GoToUrl(TestUrl);
            var loadingCheck = driver.WaitUntilFindElement(By.XPath("//div[@class='ui centered one column grid container']"));

            driver.FindElement(By.Id("username")).SendKeys(user.Username);
            driver.FindElement(By.Id("password")).SendKeys(user.Password);
            driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            var element = driver.WaitUntilFindElement(By.XPath("//div[@class='links-con']//button[@class='nav']"));
            Assert.NotNull(element);
            element.Click();

            var secondElement = driver.WaitUntilFindElement(By.XPath("//div[@class='ui centered one column grid container']"));
            Assert.NotNull(secondElement);
        }

        [Theory]
        [MemberData(nameof(BrowserFailData))]
        public void LoginTest_ShouldSeeAnError_WhenAttempToLoginWithWrongCredentials(BrowserType browserType, User user)
        {
            //setup process
            using var driver = WebDriverInfra.CreateBrowser(browserType);
            driver.Navigate().GoToUrl(TestUrl);
            var loadingCheck = driver.WaitUntilFindElement(By.XPath("//div[@class='ui centered one column grid container']"));

            //login process
            driver.FindElement(By.Id("username")).SendKeys(user.Username);
            driver.FindElement(By.Id("password")).SendKeys(user.Password);
            driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            //Assert process
            var element = driver.WaitUntilFindElement(By.XPath("//p"));
            Assert.NotNull(element);
        }

        public static IEnumerable<object[]> BrowserSuccessData
        {
            get
            {
                var user = new User { Username = "test", Password = "1234", IsValid = true };
                var data = new List<ITheoryDatum>
                {
                    TheoryDatum.Factory(BrowserType.Chrome,user),
                    TheoryDatum.Factory(BrowserType.Edge,user),
                    TheoryDatum.Factory(BrowserType.Opera,sut: user)
                };

                return data.ConvertAll(d => d.ToParameterArray());
            }
        }

        public static IEnumerable<object[]> BrowserFailData
        {
            get
            {
                var user = new User();
                var data = new List<ITheoryDatum>
                {
                    TheoryDatum.Factory(BrowserType.Chrome,new User{ Username = "Test", Password = "1234", IsValid = false }),
                    TheoryDatum.Factory(BrowserType.Edge,new User{ Username = "", Password = "", IsValid = false }),
                    TheoryDatum.Factory(BrowserType.Opera,new User{ Username = "userisnotexist", Password = "1234", IsValid = false })
                };

                return data.ConvertAll(d => d.ToParameterArray());
            }
        }

    }
}

using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;

namespace FileManagement.SeleniumTest
{
    public class AuthTest
    {
        public IWebDriver Driver;
        [SetUp]
        public void Setup()
        {
            //if you want to use like that you should chrome driver to PATH
            Driver = new ChromeDriver();
        }

        [Test]
        public void LoginTest_ShouldEnterMainScreen_IfAuthProcessDone()
        {
            Driver.Navigate().GoToUrl("http://localhost:3000/");
            //Driver.FindElement(By.XPath("//form[@class='ui inverted form']//"));
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement firstResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='ui centered two column grid container']")));

            Driver.FindElement(By.Id("username")).SendKeys("test");
            Driver.FindElement(By.Id("password")).SendKeys("1234");
            Driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            try
            {
                IWebElement secondResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='grid']//div[@class='menu']")));
                if (secondResult != null)
                {
                    Assert.Pass();
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Login process failed");
            }
        }

        [TearDown]
        public void CloseDriver()
        {
            Driver.Close();
        }
    }
}
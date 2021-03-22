using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagement.SeleniumTest
{
    public class FolderTest
    {
        private IWebDriver Driver;
        
        [SetUp]
        public void Setup()
        {
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("http://localhost:3000/");
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement firstResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='ui centered two column grid container']")));

            Driver.FindElement(By.Id("username")).SendKeys("test");
            Driver.FindElement(By.Id("password")).SendKeys("1234");
            Driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            try
            {
                IWebElement secondResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='grid']//div[@class='menu']")));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Login process failed");
            }
        }

        [Test, Order(1)]
        public void AddNewFolder()
        {
            Driver.FindElement(By.XPath("//div//button[@class='ui primary button']")).Click();
            Driver.FindElement(By.XPath("//div[@class='ui page modals dimmer transition visible active']//form//input")).SendKeys("testfolder11");
            Driver.FindElement(By.XPath("//form/button")).Click();

            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("Folder added succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("Folder adding process failed\n", exc.Message);
            }
        }

        [Test, Order(2)]
        public void EditFolderName()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(e => e.FindElement(By.XPath("//table")));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Folder list are not loaded");
            }

            Driver.FindElement(By.XPath("//table//tbody//tr[1]//td[4]//i[@class='edit icon']")).Click();
            var input = Driver.FindElement(By.XPath("//div[@class='ui page modals dimmer transition visible active']//form//input"));
            input.Clear();
            input.SendKeys("testedited");
            Driver.FindElement(By.XPath("//form/button")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("Folder edited succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("Folder editing process failed\n", exc.Message);
            }
        }

        [Test, Order(3)]
        //Ordering tests are not true. Every test should work independently
        public void DeleteFolder()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            try
            {
                wait.Until(e => e.FindElement(By.XPath("//table")));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Folder list are not loaded");
            }

            Driver.FindElement(By.XPath("//table//tbody//tr[1]//td[4]//i[@class='delete icon']")).Click();
            Driver.FindElement(By.XPath("//button[@class='ui green inverted button']")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("Folder deleted succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("Folder deleted process failed\n", exc.Message);
            }
        }

        [TearDown]
        public void CloseDriver()
        {
            Driver.Close();
        }
    }
}

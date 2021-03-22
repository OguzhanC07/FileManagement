using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace FileManagement.SeleniumTest
{
    public class FileTest
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

            try
            {
                wait.Until(e => e.FindElement(By.XPath("//table")));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Folder list are not loaded");
            }

            var tableRow = Driver.FindElement(By.XPath("//table//tbody//tr[1]"));
            new Actions(Driver).DoubleClick(tableRow).Perform();
            wait.Until(e => e.FindElement(By.XPath("//div[@tabindex='0']")));
        }

        [Test, Order(1)]
        public void UploadFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            //Find a way for upload files with a good file path
            Driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(@"C:\Users\lenovoE50\Desktop\selenium\NUnitSelenium\TestFiles\dummy.pdf");
            Driver.FindElement(By.XPath("//button[@class='ui green inverted button']")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("File edited succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("File uploading process failed\n", exc.Message);
            }
        }

        [Test, Order(2)]
        public void EditFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(e => e.FindElement(By.XPath("//table")));
            Driver.FindElement(By.XPath("//tr[@class='file'][1]//i[@class='edit icon']")).Click();
            var nameInput = Driver.FindElement(By.XPath("//form//input"));
            nameInput.Clear();
            nameInput.SendKeys("editedFileName");
            Driver.FindElement(By.XPath("//form//button")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("File edited succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("File uploading process failed\n", exc.Message);
            }

        }

        [Test, Order(3)]
        public void DeleteFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(e => e.FindElement(By.XPath("//table")));

            Driver.FindElement(By.XPath("//tr[@class='file'][1]//i[@class='delete icon']")).Click();
            Driver.FindElement(By.XPath("//button[@class='ui green inverted button']")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//body[@class='']")));
                if (passResult != null)
                {
                    Assert.Pass("File edited succesfully");
                }
            }
            catch (WebDriverTimeoutException exc)
            {
                Assert.Fail("File uploading process failed\n", exc.Message);
            }

        }


        [TearDown]
        public void CloseDriver()
        {
            Driver.Close();
        }
    }
}

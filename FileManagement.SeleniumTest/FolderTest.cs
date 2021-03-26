using FileManagement.ApiSdk.Services;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FileManagement.SeleniumTest
{
    public class FolderTest
    {
        private IWebDriver Driver;
        [SetUp]
        public async Task Setup()
        {
            IFolderService folderService = new FolderService("test","1234");
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("http://localhost:3000/");
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement firstResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='ui centered two column grid container']")));
            
            if (!string.IsNullOrEmpty(await folderService.AddAsync(null, "testfolder")))
            {
                Assert.Fail("Folder didn't added");
            }


            Driver.FindElement(By.Id("username")).SendKeys("test");
            Driver.FindElement(By.Id("password")).SendKeys("1234");
            Driver.FindElement(By.XPath("//button[@class='ui primary button']")).Click();

            try
            {
                IWebElement secondResult = wait.Until(e => e.FindElement(By.XPath("//div//div[@class='links-con']")));
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Login process failed");
            }
        }

        [Test]
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

        [Test]
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

        [Test]
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
        public async Task CloseDriver()
        {
            var element = Driver.FindElement(By.XPath("//tbody//tr[1]"));
            var id = element.GetAttribute("id");
            IFolderService folderService = new FolderService("test","1234");
            if (!await folderService.RemoveAsync(int.Parse(id)))
            {
                Assert.Fail("Folder didn't remove");
            }
            Driver.Close();
        }
    }
}

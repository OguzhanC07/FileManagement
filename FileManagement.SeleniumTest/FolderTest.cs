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
        WebDriverWait wait;

        [SetUp]
        public async Task Setup()
        {
            IFolderService folderService = new FolderService("test", "1234");
            Driver = new ChromeDriver();
            Driver.Navigate().GoToUrl("http://localhost:3000/");
            wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            IWebElement firstResult = wait.Until(e => e.FindElement(By.XPath("//div[@class='ui centered one column grid container']")));

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
        public void AddNewFolder_ShouldReturnError_IfFolderNameCharactersMoreThan50()
        {
            Driver.FindElement(By.XPath("//div//button[@class='ui primary button']")).Click();
            Driver.FindElement(By.XPath("//div[@class='ui page modals dimmer transition visible active']//form//input")).SendKeys("testfolder11testfolder11testfolder11testfolder11testfolder11testfolder11");
            Driver.FindElement(By.XPath("//form/button")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//p[@class='errorMessage']")));
                if (passResult != null)
                {
                    Assert.Pass("Folder didn't added and give an error as expected");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Folder added successfully but expected give an error");
            }
        }

        [Test]
        public void EditFolderName()
        {
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
        public void EditFolderName_ShouldReturnError_WhenFolderNameMoreThan50Characters()
        {
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
            input.SendKeys("testeditedtesteditedtesteditedtesteditedtesteditedtesteditedtesteditedtesteditedtestedited");
            Driver.FindElement(By.XPath("//form/button")).Click();

            try
            {
                IWebElement passResult = wait.Until(e => e.FindElement(By.XPath("//p[@class='errorMessage']")));
                if (passResult != null)
                {
                    Assert.Pass("Folder didn't edited and give an error as expected");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("Folder edited process done but expected result was fail.");
            }
        }

        [Test]
        public void DeleteFolder()
        {
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
            var element = Driver.FindElement(By.XPath("//tbody//td[@class='folder'][1]"));
            var id = element.GetAttribute("id");
            IFolderService folderService = new FolderService("test", "1234");
            if (!await folderService.RemoveAsync(int.Parse(id)))
            {
                Assert.Fail("Folder didn't remove");
            }
            Driver.Close();
        }
    }
}

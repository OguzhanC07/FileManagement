using FileManagement.ApiSdk.Services;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using System;
using System.IO;
using System.Threading.Tasks;

namespace FileManagement.SeleniumTest
{
    public class FileTest
    {
        private IWebDriver Driver;

        [SetUp]
        public async Task Setup()
        {
            Driver = new ChromeDriver();
            IFileService fileService = new FileService("test","1234");
            IFolderService folderService = new FolderService("test", "1234");

            if (!string.IsNullOrEmpty(await folderService.AddAsync(null, "test")))
            {
                Assert.Fail("Folder didn't added");
            }

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

            var element = Driver.FindElement(By.XPath("//tbody//tr[1]"));

            var id = element.GetAttribute("id");

            string[] filePaths = { AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\bin" }, StringSplitOptions.None)[0], "Files", "dummy.pdf" };
            string path = Path.Combine(filePaths);

            if (!string.IsNullOrEmpty(await fileService.UploadFile(int.Parse(id), path)))
            {
                Assert.Fail("File didn't upload");
            }

            var tableRow = Driver.FindElement(By.XPath("//table//tbody//tr[1]"));
            new Actions(Driver).DoubleClick(tableRow).Perform();
            wait.Until(e => e.FindElement(By.XPath("//div[@tabindex='0']")));
        }

        [Test]
        public void UploadFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));

            string[] filePaths = { AppDomain.CurrentDomain.BaseDirectory.Split(new string[] { "\\bin" }, StringSplitOptions.None)[0], "Files", "dummy.pdf" };
            string exampleFile = Path.Combine(filePaths);
            Driver.FindElement(By.XPath("//input[@type='file']")).SendKeys(exampleFile);
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

        [Test]
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

        [Test]
        public void PreviewFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(e => e.FindElement(By.XPath("//table")));
            Driver.FindElement(By.XPath("//tr[@class='file'][1]//i[@class='external square alternate icon']")).Click();
            try
            {
                wait.Until(e => e.FindElement(By.XPath("//div[contains(@style,'display: flex !important')]")));
                Driver.FindElement(By.XPath("//i[@class='close icon']")).Click();
                Assert.Pass("File preview successfully");
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Fail("File preview process failed");
            }

        }

        [Test]
        public void DownloadFile()
        {
            WebDriverWait wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10));
            wait.Until(e => e.FindElement(By.XPath("//table")));
            Driver.FindElement(By.XPath("//tr[@class='file'][1]//i[@class='download icon']")).Click();

            try
            {
                var error = wait.Until(e => e.FindElement(By.XPath("//div[@class='Toastify']//div")));
                if (error!=null)
                {
                    Assert.Fail("File download process failed");
                }
            }
            catch (WebDriverTimeoutException)
            {
                Assert.Pass("File downloaded successfuly");
            }
        }

        [Test]
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
            var element = Driver.FindElement(By.XPath("//div[@class='ui breadcrumb']//div[@class='section']"));
            var id = element.GetAttribute("id");
            IFolderService folderService = new FolderService("test","1234");
            folderService.RemoveAsync(int.Parse(id));
            Driver.Close();
        }
    }
}

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.IO;
using System.Threading;

namespace BGAppworks.testSuite
{
    public class TestSuite
    {
        private IWebDriver? driver;

        [OneTimeSetUp]
        public void SetUp()
        {
            // Get the bin directory (e.g. bin\Debug\net7.0)
            string projectDirectory = TestContext.CurrentContext.TestDirectory;

            // Navigate to the ChromeDriver folder
            string driverFolder = Path.Combine(projectDirectory, "..", "..", "..", "Driver", "chromedriver-win64");
            driverFolder = Path.GetFullPath(driverFolder);

            // Optional: Check for portable Chrome binary
            string chromeBinaryPath = Path.Combine(projectDirectory, "..", "..", "..", "Browser", "chrome-win64", "chrome.exe");
            chromeBinaryPath = Path.GetFullPath(chromeBinaryPath);

            ChromeOptions options = new ChromeOptions();

            // Set portable Chrome binary if it exists
            if (File.Exists(chromeBinaryPath))
            {
                options.BinaryLocation = chromeBinaryPath;
            }

            // Initialize the driver using the correct folder path
            driver = new ChromeDriver(driverFolder, options);

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void YourTestMethod()
        {
            driver.Navigate().GoToUrl("http://10.10.89.155:81/home/Enterprise_Build/app/start/web/");
            Thread.Sleep(30000); // Replace with proper waits in real tests
            
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver?.Quit();
        }
    }
}

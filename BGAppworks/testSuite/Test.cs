using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using System;
using System.IO;

namespace BGAppworks.testSuite
{
    public class TestSuite
    {
        private IWebDriver? driver;
        private WebDriverWait? wait;

        private const string Username = "BWSTG\\Admin_MutiA";
        private const string Password = "BG@Capricorn***1";
        private const string BaseUrl = "http://10.10.88.47:8080/home/Enterprise/app/start/web/pages/90B11C6A8EBB11E6F0E862F2E28B7B87";

        public static string BaseUrl1 => BaseUrl;

        [OneTimeSetUp]
        public void SetUp()
        {
            driver = InitializeWebDriver();
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();

            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
        }


        private IWebDriver InitializeWebDriver()
        {
            string projectDirectory = TestContext.CurrentContext.TestDirectory;
            string driverFolder = Path.Combine(projectDirectory, "..", "..", "..", "Driver", "chromedriver-win64");
            driverFolder = Path.GetFullPath(driverFolder);

            string chromeBinaryPath = Path.Combine(projectDirectory, "..", "..", "..", "Browser", "chrome-win64", "chrome.exe");
            chromeBinaryPath = Path.GetFullPath(chromeBinaryPath);

            ChromeOptions options = new ChromeOptions();
            if (File.Exists(chromeBinaryPath))
            {
                options.BinaryLocation = chromeBinaryPath;
            }

            return new ChromeDriver(driverFolder, options);
        }

        [Test]
        public void LoginTest()
        {
            NavigateToLoginPage();
            PerformLogin(Username, Password);
            AssertLoginSuccess();
        }

        private void NavigateToLoginPage()
        {
            driver!.Navigate().GoToUrl(BaseUrl1);
            wait!.Until(d => d.FindElement(By.Id("otds_username")));
        }

        private void PerformLogin(string username, string password)
        {
            var usernameField = driver!.FindElement(By.Id("otds_username"));
            usernameField.Clear();
            usernameField.SendKeys(username);

            var passwordField = driver.FindElement(By.Id("otds_password"));
            passwordField.Clear();
            passwordField.SendKeys(password);

            var loginButton = driver.FindElement(By.Id("loginbutton"));
            loginButton.Click();
        }
        
        

        private void AssertLoginSuccess()
        {
            wait!.Until(d => d.Url.Contains("dashboard") || d.Title.Contains("Home"));
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver?.Quit();
        }
    }
}

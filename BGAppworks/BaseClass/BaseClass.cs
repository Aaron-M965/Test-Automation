using BGAppworks.Utilities;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using NUnit.Framework.Interfaces;
using System.Reflection;
using log4net;
using log4net.Config;
using AventStack.ExtentReports.Model;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using WindowsInput;
using Microsoft.AspNetCore.Identity;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using SeleniumExtras.PageObjects;

namespace BGAppworks.testSuite
{

    public class BaseClass
    {
        public ReadConfig readConfig;
        public static IWebDriver driver;
        public static ExtentReports extent;
        public static ExtentTest testlog;
        //public static ExtentSparkReporter spark;
        //string dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
        //public log4net.ILog Log = log4net.LogManager.GetLogger(typeof(BaseClass));

        public BaseClass()
        {
            BasicConfigurator.Configure();
        }

        [OneTimeSetUp]
        public void SetUp()
        {
            string projectBaseDirectory = TestContext.CurrentContext.TestDirectory;
            string configFilePath = System.IO.Path.Combine(projectBaseDirectory, "ConfigurationFiles", "config.properties");
            readConfig = new ReadConfig(configFilePath);

            // Setting the environment variable for webdriver.chrome.driver
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", readConfig.getChromeForTestingDriverPath(configFilePath));

            // Additional Chrome options if needed
            ChromeOptions co = new ChromeOptions();

            // string browserPath = System.IO.Path.Combine(projectDirectory, "Browser", "chrome-win64", "chrome.exe");
            co.BinaryLocation = readConfig.getChromeEXEPath(configFilePath);
            // Set ACCEPT_SSL_CERTS capability to true
            co.AcceptInsecureCertificates = true;
            co.AddArgument("--ignore-ssl-errors=yes");
            co.AddArgument("--ignore-certificate-errors");
            // Disable notifications
            co.AddArgument("--disable-notifications");
            // Enable incognito mode
            //co.AddArgument("--incognito");


            // Initialize the ChromeDriver
            driver = new ChromeDriver(co);

            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            //driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();

            extent = ExtentManager.Instance;

            //Test URL
            string url = readConfig.getAppWorksEnvURL();
        
            //driver.Url = "http://www.google.com";
            driver.Url = url;
            Console.WriteLine("URL Provided");

        }

        [TearDown]
        public void LogOutAfterEachTest()
        {
            LoggingTestStatusExtentReport();
            for (int attempt = 1; attempt <= 3;)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    Actions actions = new Actions(driver);

                    wait.Until(ExpectedConditions.InvisibilityOfElementLocated(By.CssSelector(".inside-loader")));
                    wait.Until(ExpectedConditions.ElementToBeClickable(By.CssSelector("img[alt='User options']")));
                    IWebElement UserOptions = driver.FindElement(By.CssSelector("img[alt='User options']"));
                    actions.MoveToElement(UserOptions).Click().Perform();

                    if (driver.FindElement(By.CssSelector(".user-option")).Displayed)
                    {
                        IWebElement LogOut = driver.FindElement(By.CssSelector(".focusable.au-target[href='#']"));
                        SafeClick(LogOut);
                        break;
                    }
                }
                catch (StaleElementReferenceException)
                {
                    attempt++;
                    Console.WriteLine($"Stale element attempt {attempt}. Retrying...");
                }
            }
        }
        

        [OneTimeTearDown]
        public void TearDown()
        {
            extent.Flush();
            // Quit the driver at the end of the test
            driver.Quit();
            driver.Dispose();
        }

        public static void StartExtentTest(string testsToStart)
        {
            testlog = extent.CreateTest(testsToStart);
        }

        public static void LoggingTestStatusExtentReport()
        {
            var status = TestContext.CurrentContext.Result.Outcome.Status;
            var stacktrace = string.Empty + TestContext.CurrentContext.Result.StackTrace + string.Empty;
            var errorMessage = TestContext.CurrentContext.Result.Message;
            Status logstatus;

            switch (status)
            {
                case TestStatus.Failed:
                    logstatus = Status.Fail;
                    testlog.Log(Status.Fail, "Test steps NOT Completed for Test case " + TestContext.CurrentContext.Test.Name + " ");
                    testlog.Log(Status.Fail, "Test ended with " + Status.Fail + " – " + errorMessage);
                    string failedScreenshotPath = CaptureScreen(driver, TestContext.CurrentContext.Test.MethodName);
                    testlog.Log(Status.Fail, "SCREENSHOT TAKEN").AddScreenCaptureFromPath(failedScreenshotPath);
                    break;
                case TestStatus.Skipped:
                    logstatus = Status.Skip;
                    testlog.Log(Status.Skip, "Test ended with " + Status.Skip);
                    break;
                default:
                    logstatus = Status.Pass;
                    testlog.Log(Status.Pass, "Test Case " + TestContext.CurrentContext.Test.Name+" passed.");
                    string passScreenshotPath = CaptureScreen(driver, TestContext.CurrentContext.Test.MethodName);
                    testlog.Log(Status.Pass, "SCREENSHOT TAKEN").AddScreenCaptureFromPath(passScreenshotPath);
                    break;
            }
            
        }

        public void SafeClick(IWebElement element)
        {
            // Retry mechanism to handle stale element
            for (int attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                    wait.Until(ExpectedConditions.ElementToBeClickable(element));
                    element.Click();
                    break; // If click is successful, exit the loop
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Stale element attempt {attempt}. Retrying...");
                }
            }
        }

        public static string CaptureScreen(IWebDriver driver, string tname)
        {
            try
            {
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                
                string basePath = Assembly.GetCallingAssembly().CodeBase;
                string actualPath = basePath.Substring(0, basePath.LastIndexOf("bin"));
                string projectPath = new Uri(actualPath).LocalPath;
                string screenshotPath = projectPath + "Screenshots\\"+ DateTime.Now.ToString("yyyy-MM-dd")+"\\";
                System.IO.Directory.CreateDirectory(screenshotPath);
                string dateTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                string fileName = dateTimeStamp + " " + tname + ".png";
                string screenshotFilePath=System.IO.Path.Combine(screenshotPath, fileName);
                ss.SaveAsFile(screenshotFilePath);
                return screenshotFilePath;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                throw;
            }
	    }
    }
}

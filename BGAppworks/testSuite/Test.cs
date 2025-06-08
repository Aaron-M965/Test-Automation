using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BOPCUSAppWorks.testSuite
{
    public class Test
    {
        public static IWebDriver driver;
        [OneTimeSetUp]
        public void InvokeBrowser() {
            System.Environment.SetEnvironmentVariable("webdriver.chrome.driver", @".\\Driver\\chromedriver-win64\\chromedriver.exe");
            ChromeOptions co = new ChromeOptions();
            co.BinaryLocation = @".\\Browser\\chrome-win64\\chrome.exe";
            driver = new ChromeDriver(co);
            driver.Manage().Cookies.DeleteAllCookies();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.Manage().Window.Maximize();
        }

        [Test]
        public void TestSample()
        {
            driver.Url = "http://google.com";
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            driver.Quit();
        }
            







    }

        
    
}

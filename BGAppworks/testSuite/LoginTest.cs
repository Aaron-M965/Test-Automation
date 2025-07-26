// using NUnit.Framework;
// using OpenQA.Selenium;
// using OpenQA.Selenium.Chrome;
// using OpenQA.Selenium.Support.UI;

// namespace YourNamespace
// {
//     public class LoginTest
//     {
//         private IWebDriver? driver;
//         private WebDriverWait? wait;
//         private string BaseUrl = "https://example.com";
//         private string Username = "your_username";
//         private string Password = "your_password";

//         [SetUp]
//         public void SetUp()
//         {
//             var options = new ChromeOptions();
//             driver = new ChromeDriver("path_to_driver_folder", options);
//             wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
//         }

//         [Test]
//         public void PerformLoginTest()
//         {
//             NavigateToLoginPage();
//             PerformLogin(Username, Password);
//             AssertLoginSuccess();
//         }

//         private void NavigateToLoginPage()
//         {
//             driver!.Navigate().GoToUrl(BaseUrl);
//             wait!.Until(d => d.FindElement(By.Id("otds_username")));
//         }

//         private void PerformLogin(string username, string password)
//         {
//             var usernameField = driver!.FindElement(By.Id("otds_username"));
//             usernameField.Clear();
//             usernameField.SendKeys(username);

//             var passwordField = driver.FindElement(By.Id("otds_password"));
//             passwordField.Clear();
//             passwordField.SendKeys(password);

//             var loginButton = driver.FindElement(By.Id("loginbutton"));
//             loginButton.Click();
//         }

//         private void AssertLoginSuccess()
//         {
//             wait!.Until(d => d.FindElement(By.Id("success_message")));
//             Assert.IsTrue(driver!.FindElement(By.Id("success_message")).Displayed);
//         }

//         [TearDown]
//         public void TearDown()
//         {
//             driver?.Quit();
//         }
//     }
// }
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace EndToEndPeckFlow.StepDefinitions
{
    [Binding]
    public class GetCryptoCurrenciesPriceSteps
    {
        private IWebDriver _driver;

        [BeforeScenario]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _driver.Manage().Window.Maximize();
        }

        [AfterScenario]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Given(@"the project get the price base on USD and changes this by EUR  and etc")]
        public void GivenTheProjectASampleKeyOfCryptoCurrencyLikeBTC()
        {
            _driver.Navigate().GoToUrl("https://localhost:7288/Crypto/Index"); // Update with your app URL
        }

        [When(@"there is no exactly time or codition")]
        public void WhenIEnterTheCryptocurrencyCode(string cryptoCode)
        {
            var codeInput = _driver.FindElement(By.Id("code"));
            codeInput.SendKeys(cryptoCode);

            var submitButton = _driver.FindElement(By.CssSelector("form button[type='submit']"));
            submitButton.Click();
        }

        [Then(@"I should see the Currencies based on some Fiat Currencies")]
        public void ThenIShouldSeeTheCurrenciesBasedOnSomeFiatCurrencies()
        {
            var tableRows = _driver.FindElements(By.CssSelector("table tbody tr"));
            Assert.IsTrue(tableRows.Count > 0, "The quotes table should have at least one row.");

            foreach (var row in tableRows)
            {
                var cells = row.FindElements(By.TagName("td"));
                Assert.IsTrue(cells.Count == 2, "Each row should have two columns: Currency and Rate.");
            }
        }
    }
}

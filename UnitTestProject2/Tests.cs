using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;
using OpenQA.Selenium.Remote;

namespace CokieQAA
{
    [TestFixture]
    public class Tests
    {
        const string PATH_COOKIE_FILE = "cookies.txt";
        const string URL_AUTORIZATION = @"https://sso.teachable.com/secure/673/users/sign_in?reset_purchase_session=1";

        private IWebDriver driver;

        [SetUp]
        public void BeforeTest()
        {
            driver = new ChromeDriver();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
        }

        [TearDown]
        public void AfterTest()
        {
            driver.Quit();
        }


        [Test]
        public void Autoriztion_01_Selenium()
        {
            string email = "dddd73@yandex.ru";
            string password = "Wizard73";

            driver.Navigate().GoToUrl(URL_AUTORIZATION);
            var page = new PageHelper(driver);
            Assert.True(page.Login(email, password));

            var workInFile = new FileHelper();
            workInFile.WriteCookiesToFile(PATH_COOKIE_FILE, driver);
            page.DeleteAllCokie();
        }

        [Test]
        public void Autoriztion_02_Cookies()
        {
            driver.Navigate().GoToUrl(URL_AUTORIZATION);

            var page = new PageHelper(driver);
            page.AddCokie(PATH_COOKIE_FILE, driver);
            page.Refresh();
            Assert.True(page.CheckLogin());
        }

    }
}


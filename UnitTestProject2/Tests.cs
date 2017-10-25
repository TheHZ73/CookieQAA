using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Collections.ObjectModel;
using System.IO;

namespace UnitTestProject1
{
    [TestClass]
    public class Tests
    {
        string EDIT_PASSWORD_ID = "user_password";
        string EDIT_USER_EMAIL_ID = "user_email";
        string PATH_COOKIE_FILE = "cookies.txt";
        string URL_AUTORIZATION = @"https://sso.teachable.com/secure/673/users/sign_in?reset_purchase_session=1";
        string ELEMENT_FOR_CHECK_DOWLOAD_ID = "navbar";

        [TestMethod]
        public void Autoriztion_01_Selenium()
        {
            string email = "dddd73@yandex.ru";
            string password = "Wizard73";

            TimeSpan timeSpan = new TimeSpan(10);
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Manage().Timeouts().ImplicitlyWait(timeSpan);
                driver.Navigate().GoToUrl(URL_AUTORIZATION);
                IWebElement userEmail = driver.FindElement(By.Id(EDIT_USER_EMAIL_ID));
                IWebElement userPassword = driver.FindElement(By.Id(EDIT_PASSWORD_ID));
                userEmail.SendKeys(email);
                userPassword.SendKeys(password);
                userPassword.SendKeys(Keys.Enter);

                var cookieFirst = driver.Manage().Cookies.AllCookies;
                var workInFile = new WorkWithFile();
                workInFile.WriteCookiesToFile(PATH_COOKIE_FILE, cookieFirst);
                driver.Manage().Cookies.DeleteAllCookies();

                IWebElement forCheck = driver.FindElement(By.Id(ELEMENT_FOR_CHECK_DOWLOAD_ID));
                Assert.IsNotNull(forCheck);
            }
        }

        [TestMethod]
        public void Autoriztion_02_Cookies()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(URL_AUTORIZATION);
                driver.Manage().Cookies.DeleteAllCookies();

                TimeSpan timeSpan = new TimeSpan(10);
                driver.Manage().Timeouts().ImplicitlyWait(timeSpan);

                var workInFile = new WorkWithFile();
                Collection<Cookie> lastVisitCookie = workInFile.readCookiesFromFile(PATH_COOKIE_FILE);
                foreach (Cookie loadedCookie in lastVisitCookie)
                {
                    Cookie forAdd = new Cookie(loadedCookie.Name, loadedCookie.Value);
                    driver.Manage().Cookies.AddCookie(forAdd);
                }

                IWebElement userPassword = driver.FindElement(By.Id(EDIT_PASSWORD_ID));
                userPassword.SendKeys(Keys.Enter);

                IWebElement forCheck = driver.FindElement(By.Id(ELEMENT_FOR_CHECK_DOWLOAD_ID));
                Assert.IsNotNull(forCheck);
            }
        }
    }
}

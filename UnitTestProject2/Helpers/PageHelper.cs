using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System.Collections.ObjectModel;

namespace CokieQAA
{
    public class PageHelper
    {
        private IWebDriver Driver;

        private By User = By.Id("user_email");
        private By Password = By.Id("user_password");
        private By LoginButton = By.CssSelector(@"#new_user input.login-button");
        private By ElementCheckLogin = By.Id("navbar");

        public PageHelper(IWebDriver driver)
        {
            Driver = driver;
        }

        public bool Login(string userName, string password)
        {
            Driver.FindElement(User).SendKeys(userName);
            Driver.FindElement(Password).SendKeys(password);
            Driver.FindElement(Password).SendKeys(Keys.Enter);
            return CheckLogin();
        }

        public bool CheckLogin()
        {
            return Driver.FindElement(ElementCheckLogin) != null;
        }

        public void Refresh()
        {
            Driver.Navigate().Refresh();
        }

        public void DeleteAllCokie()
        {
            Driver.Manage().Cookies.DeleteAllCookies();
        }

        public void AddCokie(string pathCokie, IWebDriver driver)
        {
            var workInFile = new FileHelper();
            Collection<Cookie> lastVisitCookie = workInFile.ReadCookiesFromFile(pathCokie);
            foreach (Cookie loadedCookie in lastVisitCookie)
            {
                Cookie forAdd = new Cookie(loadedCookie.Name, loadedCookie.Value);
                driver.Manage().Cookies.AddCookie(forAdd);
            }
        }
    }
}

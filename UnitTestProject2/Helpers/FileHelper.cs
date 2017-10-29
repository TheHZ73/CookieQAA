using System;
using System.Collections.ObjectModel;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace CokieQAA
{
    public class FileHelper
    {
        public void WriteCookiesToFile(string nameFile, IWebDriver driver)
        {
            var cookieLastVisit = driver.Manage().Cookies.AllCookies;

            if (File.Exists(nameFile))
            {
                File.Delete(nameFile);
            }

            using (StreamWriter sw = new StreamWriter(nameFile, true, System.Text.Encoding.Default))
            {
                foreach (Cookie cookie in cookieLastVisit)
                {
                    sw.WriteLine((cookie.Name + ";" + cookie.Value + ";" +
                      cookie.Domain + ";" + cookie.Path + ";" + cookie.Expiry +
                         ";" + cookie.Secure));
                }
            }
        }

        public Collection<Cookie> ReadCookiesFromFile(string nameFileCookie)
        {
            Collection<Cookie> cookieColletion = new Collection<Cookie>();
            using (StreamReader sr = new StreamReader(nameFileCookie, System.Text.Encoding.Default))
            {
                string name, value, domain, path;
                DateTime expire;
                string line;
                string[] array;

                while ((line = sr.ReadLine()) != null)
                {
                    array = line.Split(new char[] { ';' });
                    name = array[0];
                    value = array[1];
                    domain = array[2];
                    path = array[3];
                    expire = array[4].Equals("") ? DateTime.Now.AddDays(14) : Convert.ToDateTime(array[4]);

                    Cookie cookie = new Cookie(name, value, domain, path, expire);
                    cookieColletion.Add(cookie);
                }
            }
            return cookieColletion;
        }
    }
}

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Runtime.Serialization;
using OpenQA.Selenium;

namespace UnitTestProject1
{
    public class WorkWithFile
    {
        public void WriteCookieInFile(string nameFile, ReadOnlyCollection<OpenQA.Selenium.Cookie> cookie, IFormatter formatter)
        {
            using (Stream stream = new FileStream(nameFile, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                formatter.Serialize(stream, cookie);
                stream.Close();
            }
        }

        public void WriteCookiesToFile(string nameFile, ReadOnlyCollection<Cookie> lastVisitCookie)
        {
            if (File.Exists(nameFile))
            {
                File.Delete(nameFile);
            }

            using (StreamWriter sw = new StreamWriter(nameFile, true, System.Text.Encoding.Default))
            {
                foreach (OpenQA.Selenium.Cookie cookie in lastVisitCookie)
                {
                    sw.WriteLine((cookie.Name + ";" + cookie.Value + ";" +
                      cookie.Domain + ";" + cookie.Path + ";" + cookie.Expiry +
                         ";" + cookie.Secure));
                }
            }
        }

        public Collection<Cookie> readCookiesFromFile(string nameFileCookie)
        {
            Collection<Cookie> cookieColletion = new Collection<Cookie>();
            Cookie cookie;
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

                    cookie = new Cookie(name, value, domain, path, expire);
                    cookieColletion.Add(cookie);
                }
            }
            return cookieColletion;
        }
    }
}

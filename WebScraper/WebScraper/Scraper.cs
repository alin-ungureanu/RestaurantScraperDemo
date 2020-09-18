using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;

namespace WebScraper
{
    public class Scraper
    {
        private String url;
        private static Scraper _instance;

        private LinkedList<Dictionary<String, String>> scrapedContents;
        private Scraper()
        {
            scrapedContents = new LinkedList<Dictionary<String, String>>();
            url = null;
        }

        public static Scraper getInstance()
        {
            if (_instance == null)
            {
                _instance = new Scraper();
            }

            return _instance;
        }


        public void setUrl(String url)
        {
            this.url = url;//https://www.pure.co.uk/menus/breakfast/
        }

        public async void startScraping()
        {
            IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl(url);

            Thread.Sleep(1000);

            var submenu = driver.FindElement(By.XPath("/html/body/main/section/div[1]/div/div[1]/a/h3"));

            Console.WriteLine("First categ is " + submenu.Text);

            driver.Close();
        }
    }
}

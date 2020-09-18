using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebScraper
{
    public class Scraper
    {
        private String url;
        public Scraper(String url)
        {
            this.url = url;//https://www.pure.co.uk/menus/breakfast/
        }

        public async void startScraping()
        {
            IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver();
            driver.Navigate().GoToUrl(url);

            Thread.Sleep(3000);

            var submenu = driver.FindElement(By.XPath("/html/body/main/section/div[1]/div/div[1]/a/h3"));

            //var submenuTitle = submenu.FindElement(By.TagName("h2"));

            Console.WriteLine("First categ is " + submenu.Text);

            driver.Close();
        }
    }
}

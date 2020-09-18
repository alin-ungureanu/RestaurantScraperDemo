using OpenQA.Selenium;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Builder;

namespace WebScraper
{
    public class Scraper
    {
        private String url;
        private static Scraper _instance;
        private IWebDriver driver;

        private LinkedList<Dictionary<String, String>> scrapedContents;
        private Scraper()
        {
            scrapedContents = new LinkedList<Dictionary<String, String>>();
            url = null;
            //init chrome driver
            ChromeOptions options = new ChromeOptions();
            //options.AddArgument("--start-maximized");
            driver = new ChromeDriver(options);
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
            String menuTitle = "";
            String menuSectionTitle = "";
            String menuDescription = "";
            String dishName = "";

            driver.Navigate().GoToUrl(url);

            //wait for the page to reload
            Thread.Sleep(2000);


            var closeSubscription = driver.FindElement(By.XPath("//*[@id=\"popmake-4332\"]/button"));
            closeSubscription.Click();

            

            var section = driver.FindElement(By.TagName("section"));
            var items = section.FindElements(By.XPath("*"));
            Console.WriteLine("number of items " + items.Count);
            //extracting the menu title, e.g. "Breakfast"
            menuTitle = items[1].FindElement(By.TagName("h1")).Text;
            menuDescription = items[1].FindElement(By.TagName("p")).Text;
            //replace html tags and quotes from the menuDescription
            menuDescription = Regex.Replace(menuDescription, "<br>", " ");
            menuDescription = Regex.Replace(menuDescription, "\"", String.Empty);
            //skipping the first item, which is unrelated
            for (int i = 2; i < items.Count; ++i)
            {
                var item = items[i];
                Console.WriteLine("current item " + i);
                Console.WriteLine(item.GetAttribute("class"));
                if (item.GetAttribute("class") == "menu-title")
                {
                    menuSectionTitle = item.FindElement(By.TagName("span")).Text;
                }
                else if (item.GetAttribute("class") == "collapse in")
                {
                    var foods = item.FindElements(By.ClassName("menu-item"));
                    foreach(var food in foods)
                    {
                        Console.WriteLine("Scraping item " + i + " in a new tab");
                        dishName = food.FindElement(By.TagName("h3")).Text;
                        dishName = dishName.Substring(dishName.IndexOf('\n') + 1);
                        //var link = food.FindElement(By.TagName("a"));
                        parseItem(food, menuTitle, menuSectionTitle, menuDescription, dishName);
                    }
                    

                }
            }


            Console.WriteLine("Finished Scraping");

            printScrapedContents();

            Console.WriteLine("Finished printing results");


            Thread.Sleep(10000);
            driver.Close();
        }


        private void parseItem(IWebElement webElement, String menuTitle, String menuSectionTitle, String menuDescription, String dishName)
        {
            //webElement.SendKeys(Keys.Control + "t");

            Dictionary<String, String> data = new Dictionary<String, String>();

            //open link in new tab
            //Actions action = new Actions(driver);
            //action.KeyDown(Keys.Control).MoveToElement(webElement).Click().Perform();
            webElement.Click();
            Thread.Sleep(1000);
            //move focus to new tab
            if (driver.WindowHandles.Count > 1)
            {
                driver.SwitchTo().Window(driver.WindowHandles[1]);
            }
            else
            {
                Console.WriteLine("Driver does not have second window");
            }
            Thread.Sleep(500);
            var foodItem = driver.FindElement(By.ClassName("menu-item-details"));

            data["MenuTitle"] = menuTitle;
            data["MenuDescription"] = menuDescription;
            data["MenuSectionTitle"] = menuSectionTitle;

            data["DishName"] = dishName;
            var dishDescription = foodItem.FindElements(By.TagName("p"));
            data["DishDescription"] = dishDescription[1].Text;


            printItem(data);

            scrapedContents.AddLast(data);
            //close the newly opened tab
            /*action = new Actions(driver);
            //driver.Actions().keyDown(protractor.Key.CONTROL).sendKeys('w').Perform();
            action.KeyDown(Keys.Control).MoveToElement(foodItem).SendKeys("W").Perform();
            Thread.Sleep(500);
            driver.SwitchTo().Window(driver.WindowHandles[0]);*/

            driver.Navigate().Back();
            Thread.Sleep(1000);
        }


        public void printScrapedContents()
        {
            foreach(Dictionary<String,String> data in scrapedContents)
            {
                printItem(data);
                Console.WriteLine();
            }
        }

        private void printItem(Dictionary<String, String> data)
        {
            foreach (KeyValuePair<String, String> pair in data)
            {
                Console.WriteLine(pair.Key + " = " + pair.Value);
            }
        }
    }
}

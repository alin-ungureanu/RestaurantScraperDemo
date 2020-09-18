Project name
============
Demo scraper web app for a restaurant menu.

Description
===========
The app scrapes a restaurant menu page and extracts the products info.
Target page Url is https://www.pure.co.uk/menus/breakfast/


Implementation details
======================
The scraper identifies each product as a child of the "section" class container.
Each of these children is either a menu section title or that menu section title's foods list.
Each food item from the foodlist is open in a new tab, where its dish description is extracted.
Once finished with an item in a new tab, the scraper closes that tab and returns to the previous
tab. The other 4 attributes (Menu Title, Menu Description, Menu Section Title, Dish Name) are
scraped in the Breakfast menu page. Dish Name is also present on the food item page, but in CAPS,
so I decided to parse it in the main Breakfast page.
The scraper stores the scraped data in a List of Dictionaries containing (key,value) pairs.
This scraped data is then sent in JSON format to the http endpoint and it is also stored in an
SQLite database.

Usage
=====
The web app starts on http://localhost:8080
There is an input text which requires a restaurant menu URL via a POST request.
The Current scraper is a demo and works only with the following link: https://www.pure.co.uk/menus/breakfast/



# Digital_Humanities
# Project: Israeli cinema industry social network

## Description

As part of the course "Topics in Digital Humanities" we upgraded the database of the website "the Book of Israeli Cinema" (https://www.cinemaofisrael.co.il/). 
We started by fixing the database and making it more structured and readable by a machine. 
Next, by accessing the API of "Wikipedia" and "Wikidata" we added information to the database including biography, date of birth, as well as the wikidata id and url for each person.
We then created a local site that enables the user to make queries, based on this database, about israeli movies and people from the industry (e.g. collaborations between people - including actors, directors, producers etc.).
Finally, we enabled more complexed queries based on information from wikidata.

We extracted the data for the social network from [cinemaofisrael site](https://www.cinemaofisrael.co.il/%d7%aa%d7%95%d7%9b%d7%9f-%d7%a2%d7%a0%d7%99%d7%99%d7%a0%d7%99%d7%9d/) by a web crawler writen in JavaScript.
??????The data has expanded by Wikidata in order to create a wide and comprehensive MongoDB database (database details below). <br />
Web crawler files:
+ [Movies scraper](https://github.com/shaybensimon/tdh192/blob/master/movie-page-scraper.js)
+ [Actors scraper](https://github.com/shaybensimon/tdh192/blob/master/actors-scraper.js)
+ [Wikidata scraper](https://github.com/shaybensimon/tdh192/blob/master/wikidata-handler.js)

Crawlers and Database integation:
+ [JS index file](https://github.com/shaybensimon/tdh192/blob/master/index.js)
?????


## The database
[cinema-of-israel-db](https://github.com/shaybensimon/tdh192/tree/master/db-backup/cinema-of-israel-db) with MongoDB.
Includes two collections:
+ movies: 1021 movie records. You can find information (Hebrew) like: cast, characters, brief, years etc.
+ persons: 16343 cinema entity records. You can find information like: gender, years, acting career etc.

To restore [DB](https://github.com/shaybensimon/tdh192/tree/master/db-backup/cinema-of-israel-db): install mongodb server and then from project directory (tdh192) type in console 'mongorestore --db cinema-of-israel-db ./db-backup/cinema-of-israel-db'.


## Queries and outputs

When running the program a local site will appear. At the top right part of the screen you will be able to choose out of 2 options:

1) Multiple Query
In this option you can get all movies by a certain actor, director, producer or screenwriter, as well as all movies in a certain year.
You can also get collaborations between people by pressing "more query". When done press "submit".
After pressing submit a JSON file and a csv file will be downloaded to your computer at directory C:\OutputForIsraeliMoviesLibrary.

Example: 
קטגוריה: שם שחקן - הכנס שם שחקן
ערך: יהודה ברקן
more query

קטגוריה: שנה - הכנס שנה
ערך: 1968
more query

קטגוריה: שם במאי - הכנס שם במאי
ערך: יוסף שלחין
submit

In this case the output will be all movies from 1968 with Yehuda Barkan as an actor and Yosef Shlachin as director.
(The category "שם אמן - הכנס שם אמן" will return all movies of an artist regardless of his role there - actor, director, producer or screenwriter)

2) Query
In this option you can choose to get information about a movie or an actor, or all actors playing in a specific movie.
After pressing submit a JSON file and a csv file will be downloaded to your computer at directory C:\OutputForIsraeliMoviesLibrary.

Example: 
קטגוריה: שחקנים לפי סרט - הכנס שם סרט
ערך: אבא גנוב
submit

In this case the output will be a list of all actors that played in "אבא גנוב".

??????????Queries from [this file](https://github.com/shaybensimon/tdh192/blob/master/network%20graph.py).?????????????
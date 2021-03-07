using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace WebAppMovies.Logic
{
    public class Queries
    {
        public MoviesOperations moviesOperations { get; set; }
        public PersonOperations personOperations { get; set; }

        public string outputDirPath { get; set; }

        public Queries()
        {
            this.outputDirPath = @"C:\OutputForIsraeliMoviesLibrary";
            if (!Directory.Exists(outputDirPath))
            {
                Directory.CreateDirectory(outputDirPath);
            }

            // var mongodb = new MongoCRUD("cinema-of-israel-db");
            var client = new MongoClient("mongodb://localhost:27017");
            var db = client.GetDatabase("cinema-of-israel-db");

            this.moviesOperations = new MoviesOperations(db.GetCollection<movie>("movies"));
            this.personOperations = new PersonOperations(db.GetCollection<Person>("persons"));
        }

        public void QueriesSolution(Query[] queries)
        {
            int iter_counter = 0;
            string file_name = "";
            var moviesOutput = new List<movie>();
            var personsOutput = new List<Person>();
            if (queries == null)
            {
                return;
            }
            if (queries[0] == null)
            {
                return;
            }
            if (queries[0].Type == "complex")
            {
                WikiDataQuries(queries);
                return;
            }
            foreach (var q in queries)
            {
                if (q == null)
                {
                    break;
                }

                if (q.Type == "multiple")
                {
                    switch (q.Key1)
                    {
                        case "שנה - הכנס שנה":
                            if (iter_counter > 0)
                            {
                                moviesOutput = moviesOperations.GetAllMoviesByYear(moviesOutput, Convert.ToDouble(q.Value));
                                file_name += "All_Movies_In_Year_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = moviesOperations.GetallmoviesInYear(Convert.ToDouble(q.Value));
                                file_name = "All_Movies_In_Year_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "שם שחקן - הכנס שם שחקן":

                            if (iter_counter > 0)
                            {
                                moviesOutput = personOperations.GetAllMoviesByActor(moviesOutput, q.Value);
                                file_name += "All_Movies_With_The_Actor_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = moviesOperations.MovieObject_to_movie(personOperations.GetAllMoviesByActor(q.Value));
                                file_name = "All_Movies_With_The_Actor_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "שם אמן - הכנס שם אמן":

                            if (iter_counter > 0)
                            {
                                List<movie> NewMovies = new List<movie>();
                                var personMovies = Get_all_movies_by_person(q.Value);
                                if (moviesOutput != null && personMovies != null)
                                {
                                    foreach (var m in moviesOutput)
                                    {
                                        var movieName = m.MovieName;

                                        foreach (var x in personMovies)
                                        {
                                            if (movieName.Equals(x))
                                            {
                                                NewMovies.Add(m);
                                            }
                                        }
                                    }
                                    moviesOutput = NewMovies;
                                }
                                else moviesOutput = null;
                                file_name += "All_Movies_With_The_Person_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = Get_all_movies_by_person(q.Value);
                                file_name = "All_Movies_With_The_Person_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "שם במאי - הכנס שם במאי":
                            if (iter_counter > 0)
                            {
                                moviesOutput = personOperations.GetAllMoviesByDirector(moviesOutput, q.Value);
                                file_name += "All_Movies_With_The_Director_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByDirector(q.Value));
                                file_name = "All_Movies_With_The_Director_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "שם מפיק - הכנס שם מפיק":
                            if (iter_counter > 0)
                            {
                                moviesOutput = personOperations.GetAllMoviesByProducer(moviesOutput, q.Value);
                                file_name += "All_Movies_With_The_Producer_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByProducer(q.Value));
                                file_name = "All_Movies_With_The_Producer_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "שם תסריטאי - הכנס שם תסריטאי":
                            if (iter_counter > 0)
                            {
                                moviesOutput = personOperations.GetAllMoviesByScreenwriter(moviesOutput, q.Value);
                                file_name += "All_Movies_With_The_Screenwriter_" + q.Value;
                            }
                            else
                            {
                                moviesOutput = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByScreenwriter(q.Value));
                                file_name = "All_Movies_With_The_Screenwriter_" + q.Value;
                            }
                            iter_counter++;
                            break;

                        case "done":
                            moviesOperations.Build_act_and_char_field(moviesOutput);
                            Converts.ConvertDataToCsv(moviesOutput, outputDirPath + "\\" + file_name + ".csv");
                            Converts.ConvertDataToJson(moviesOutput, outputDirPath + "\\" + file_name + ".json");
                            file_name = "";
                            moviesOutput = new List<movie>();
                            break;
                    }
                }
                else if (q.Type == "single")
                {
                    switch (q.Key1)
                    {
                        case "מידע על סרט - הכנס שם סרט":

                            movie single_movie = moviesOperations.GetMovieByName(q.Value);//special case in which you want a specific movie - in this case "single movie" is the output
                            if (single_movie != null)
                            {
                                List<movie> the_movie = new List<movie>();
                                the_movie.Add(single_movie);
                                moviesOutput = the_movie;
                                file_name = "Info_About_The_Movie_" + single_movie.MovieName;
                            }
                            else
                            {
                                file_name = "NoMatchingMovies";
                            }
                            moviesOperations.Build_act_and_char_field(moviesOutput);
                            Converts.ConvertDataToCsv(moviesOutput, outputDirPath + "\\" + file_name + ".csv");
                            Converts.ConvertDataToJson(moviesOutput, outputDirPath + "\\" + file_name + ".json");
                            file_name = "";
                            moviesOutput = new List<movie>();
                            break;

                        case "שחקנים לפי סרט - הכנס שם סרט":
                            var actors = moviesOperations.Actors_in_a_movie(q.Value);

                            foreach (var act in actors)
                            {
                                personsOutput.Add(personOperations.GetPerson(act.name));
                            }

                            file_name = "All_The_Actors_In_The_Movie_" + q.Value;
                            Converts.ConvertDataToCsv(personsOutput, outputDirPath + "\\" + file_name + ".csv");
                            Converts.ConvertDataToJson(personsOutput, outputDirPath + "\\" + file_name + ".json");
                            file_name = "";
                            personsOutput = new List<Person>();
                            break;

                        case "מידע על אמן - הכנס שם אמן":
                            var artist = personOperations.GetPerson(q.Value);
                            personsOutput.Add(artist);

                            file_name = "Info_About_The_Artist_" + q.Value;
                            Converts.ConvertDataToCsv(personsOutput, outputDirPath + "\\" + file_name + ".csv");
                            Converts.ConvertDataToJson(personsOutput, outputDirPath + "\\" + file_name + ".json");
                            file_name = "";
                            personsOutput = new List<Person>();
                            break;
                    }
                }
            }
        }

        public void WikiDataQuries(Query[] queries)
        {
            string[] file_name = new string[1];
            int i = 0;
            var allTheQueries = "";
            var filter = "";
            //a person from israel
            allTheQueries = allTheQueries + "wdt:P31%20wd:Q5;%20wdt:P27%20wd:Q801;%20";

            foreach (var q in queries)
            {
                if (q == null)
                {
                    break;
                }
                switch (q.Key1)
                {
                    case "שחקנים הישראליים":
                        file_name[0] = file_name[0] + "All_Actors";
                        allTheQueries = allTheQueries + "wdt:P106%20wd:" + getId("actors") + ";%20";
                        break;

                    case "במאים הישראליים":
                        file_name[0] = file_name[0] + "All_Director";
                        allTheQueries = allTheQueries + "wdt:P106%20wd:" + getId("director") + ";%20";
                        break;

                    case "מפיקים הישראליים":
                        file_name[0] = file_name[0] + "All_Producer";
                        allTheQueries = allTheQueries + "wdt:P106%20wd:" + getId("producer") + ";%20";
                        break;
                }
                switch (q.Key2)
                {
                    case "שנולדו בעיר":
                        allTheQueries = allTheQueries + "wdt:P19%20wd:" + getId(q.Value) + ";%20";
                        file_name[0] = file_name[0] + "That_Was_Born_In" + q.Value;
                        break;

                    case "שנולדו בשנה":
                        filter = filter + "FILTER%20(YEAR(?birthdate)%20=%20" + q.Value + ")";
                        file_name[0] = file_name[0] + "That_Was_Born_at" + q.Value;
                        break;

                    case "שנולדו לאחר שנת":
                        filter = filter + "FILTER%20(YEAR(?birthdate)%20%3E=%20" + q.Value + ")";
                        file_name[0] = file_name[0] + "That_Was_Born_after" + q.Value;
                        break;
                }
            }

            var x = complexQueries(allTheQueries, filter);
            Converts.ConvertDataToJson(x, outputDirPath + "\\" + file_name[0] + "_WikiData_Format" + ".json");
        }

        public List<movie> Get_all_movies_by_person(string name)
        {
            List<movie> MovieList = moviesOperations.MovieObject_to_movie(personOperations.GetAllMoviesByActor(name));
            List<movie> tmp = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByDirector(name));
            if (tmp != null)
            {
                foreach (movie m in tmp)
                {
                    if (!MovieList.Contains(m))
                        MovieList.Add(m);
                }
            }
            tmp = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByProducer(name));
            if (tmp != null)
            {
                foreach (movie m in tmp)
                {
                    if (!MovieList.Contains(m))
                        MovieList.Add(m);
                }
            }
            tmp = moviesOperations.Names_to_movies(personOperations.GetAllMoviesByScreenwriter(name));
            if (tmp != null)
            {
                foreach (movie m in tmp)
                {
                    if (!MovieList.Contains(m))
                        MovieList.Add(m);
                }
            }
            return MovieList;
        }

        public string getId(string name)
        {
            var results = WikiDataQueries("https://www.wikidata.org/w/api.php?action=wbsearchentities&search=" + name + "&language=he&format=json");

            //add !!!! try with another name

            string search = JObject.Parse(results)["search"].First().ToString();
            return JObject.Parse(search)["id"].ToString();
        }

        public static string complexQueries(string queries, string filter)
        {
            return WikiDataQueries("https://query.wikidata.org/sparql?query=select%20distinct%20?item%20?itemLabel%20?itemDescription%20(year(?birthdate)%20as%20?birthyear)%20(year(?deathdate)%20as%20?deathyear)%20?genderLabel%20where%20{%20?item%20" + queries + "%20OPTIONAL{?item%20wdt:P569%20?birthdate%20.}%20OPTIONAL{?item%20wdt:P570%20?deathdate%20.}%20OPTIONAL{?item%20wdt:P21%20?gender%20.}%20SERVICE%20wikibase:label%20{%20bd:serviceParam%20wikibase:language%20%22he%22%20}%20" + filter + "%20}%20ORDER%20BY%20DESC(?birthdate)&format=json");
        }

        public static string WikiDataQueries(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UseDefaultCredentials = true;
            request.UserAgent = "[any words that is more than 5 characters]";
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
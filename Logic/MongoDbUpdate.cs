using MongoDB.Driver;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebAppMovies.Logic;
using WikiClientLibrary.Client;
using WikiClientLibrary.Pages;
using WikiClientLibrary.Pages.Queries;
using WikiClientLibrary.Sites;

namespace WebAppMovies.Logic
{
    public class MongoDbUpdate
    {
        private static async Task MainAsync()
        {
            var outputDirPath = @"C:\OutputForIsraeliMoviesLibrary";
            if (!Directory.Exists(outputDirPath))
            {
                Directory.CreateDirectory(outputDirPath);
            }

            var mongoClient = new MongoClient("mongodb://localhost:27017");
            var db = mongoClient.GetDatabase("cinema-of-israel-db");
            var personCollection = db.GetCollection<Person>("persons");
            var moviesCollection = db.GetCollection<movie>("movies");

            var moviesOperations = new MoviesOperations(moviesCollection);

            var personOperations = new PersonOperations(personCollection);

            Set_alona_ben_keren(personCollection);

            var all = personOperations.Get_all_persons();

            personOperations.Update_directed();

            List<string> non_biography = new List<string>();

            non_biography = new List<string>(personOperations.Get_all_persons_without_biography());

            List<string> no_date_birth = new List<string>(personOperations.Get_all_persons_without_date_of_birth());

            // A WikiClient has its own CookieContainer.
            var client = new WikiClient
            {
                ClientUserAgent = "WCLQuickStart/1.0 orit"
            };
            // You can create multiple WikiSite instances on the same WikiClient to share the state.
            var site = new WikiSite(client, "https://he.wikipedia.org/w/api.php");
            // Wait for initialization to complete
            await site.Initialization;

            int c = 0;
            foreach (Person p in all)
            {
                if (p.wdId == null || p.wdId[0] != 'Q' && p.wdId != "noWikiData")
                {
                    try
                    {
                        var results = Queries.WikiDataQueries("https://www.wikidata.org/w/api.php?action=wbsearchentities&search=" + p.Name + "&language=he&format=json");
                        string search = JObject.Parse(results)["search"].First().ToString();
                        var id = JObject.Parse(search)["id"].ToString();
                        //update mongo
                        var filter = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                        var update = Builders<Person>.Update.Set(x => x.wdId, id);
                        var options = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filter, update, options);

                        var filter1 = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                        var result1 = GetUrl(p.wdId);
                        var update1 = Builders<Person>.Update.Set(x => x.WikiDataUrl, result1.Result);
                        var options1 = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filter1, update1, options1);
                    }
                    catch { }
                }

                if (p.wdId[0] == 'Q')
                {
                    var filter = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                    var result = GetUrl(p.wdId);
                    var update = Builders<Person>.Update.Set(x => x.WikiDataUrl, result.Result);
                    var options = new FindOneAndUpdateOptions<Person>();
                    personCollection.FindOneAndUpdate(filter, update, options);
                }
                else if (p.wdId == "noWikiData")
                {
                    var filter = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                    var result = GetUrl(p.wdId);
                    var update = Builders<Person>.Update.Set(x => x.WikiDataUrl, result.Result);
                    var options = new FindOneAndUpdateOptions<Person>();
                    personCollection.FindOneAndUpdate(filter, update, options);
                }

                c++;
                Console.WriteLine(c);
            }

            c = 0;

            foreach (var p in no_date_birth)
            {
                if (p != null)
                {
                    if (p.Contains("["))
                    {
                        try
                        {
                            //update date of birth
                            string y = Char.ToString(p[0]) + getBetween(p, Char.ToString(p[0]), "[") + "(" + getBetween(p, "[", "]") + ")";
                            var filter = Builders<Person>.Filter.Where(x => x.Name == p);
                            var result = GetPageDateOfBirth(y, site);
                            var update = Builders<Person>.Update.Set(x => x.DateOfBirth, result.Result);
                            var options = new FindOneAndUpdateOptions<Person>();
                            personCollection.FindOneAndUpdate(filter, update, options);
                        }
                        catch { }
                    }
                    else
                    {
                        var filter = Builders<Person>.Filter.Where(x => x.Name == p);
                        var result = GetPageDateOfBirth(p, site);
                        var update = Builders<Person>.Update.Set(x => x.DateOfBirth, result.Result);
                        var options = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filter, update, options);
                        c++;
                        Console.WriteLine(p);
                        Console.WriteLine(c);
                    }
                }
            }

            c = 0;

            foreach (var p in non_biography)
            {
                if (p != null)
                {
                    if (p.Contains("["))
                    {
                        try
                        {
                            string y = Char.ToString(p[0]) + getBetween(p, Char.ToString(p[0]), "[") + "(" + getBetween(p, "[", "]") + ")";
                            var filter = Builders<Person>.Filter.Where(x => x.Name == p);
                            var result = GetPageBiography(y, site);
                            var update = Builders<Person>.Update.Set(x => x.Biography, result.Result);
                            var options = new FindOneAndUpdateOptions<Person>();
                            personCollection.FindOneAndUpdate(filter, update, options);
                        }
                        catch { }
                    }
                    else
                    {
                        //update biography
                        var filter = Builders<Person>.Filter.Where(x => x.Name == p);
                        var result = GetPageBiography(p, site);
                        var update = Builders<Person>.Update.Set(x => x.Biography, result.Result);
                        var options = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filter, update, options);
                        c++;
                        Console.WriteLine(p);
                        Console.WriteLine(c);
                    }
                }
            }

            foreach (var p in all)
            {
                //update the wdId
                try
                {
                    var results = Queries.WikiDataQueries("https://www.wikidata.org/w/api.php?action=wbsearchentities&search=" + p.Name + "&language=he&format=json");
                    string search = JObject.Parse(results)["search"].First().ToString();
                    var id = JObject.Parse(search)["id"].ToString();
                    var url = JObject.Parse(search)["url"].ToString();
                    //update mongo
                    var filter = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                    var update = Builders<Person>.Update.Set(x => x.wdId, id);
                    var options = new FindOneAndUpdateOptions<Person>();
                    personCollection.FindOneAndUpdate(filter, update, options);
                    ////update mongo
                    var filterUrl = Builders<Person>.Filter.Where(x => x.WikiDataUrl == null | x.WikiDataUrl == "");
                    var updateUrl = Builders<Person>.Update.Set(x => x.WikiDataUrl, url);
                    var optionsUrl = new FindOneAndUpdateOptions<Person>();
                    personCollection.FindOneAndUpdate(filterUrl, updateUrl, optionsUrl);
                }
                catch
                {
                    try
                    {
                        var results = Queries.WikiDataQueries("https://www.wikidata.org/w/api.php?action=wbsearchentities&search=" + p.DiferentName + "&language=he&format=json");
                        string search = JObject.Parse(results)["search"].First().ToString();
                        var id = JObject.Parse(search)["id"].ToString();
                        var url = JObject.Parse(search)["url"].ToString();
                        //update mongo
                        var filter = Builders<Person>.Filter.Where(x => x.Name == p.Name);
                        var update = Builders<Person>.Update.Set(x => x.wdId, id);
                        var options = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filter, update, options);
                        ////update mongo
                        var filterUrl = Builders<Person>.Filter.Where(x => x.WikiDataUrl == null | x.WikiDataUrl == "");
                        var updateUrl = Builders<Person>.Update.Set(x => x.WikiDataUrl, url);
                        var optionsUrl = new FindOneAndUpdateOptions<Person>();
                        personCollection.FindOneAndUpdate(filterUrl, updateUrl, optionsUrl);
                    }
                    catch { }
                }
            }
        }

        private async static Task<string> GetPageBiography(string name, WikiSite site)
        {
            var page = new WikiPage(site, name);
            var provider = WikiPageQueryProvider.FromOptions(PageQueryOptions.FetchContent | PageQueryOptions.ResolveRedirects);
            await page.RefreshAsync(provider);
            if (page.Content != null)
            {
                try
                {
                    var str = getBetween(page.Content, "==ביוגרפיה==", "==");
                    var output = String.Join("", str.Split(']', '['));
                    if (output == "" || output == "\n")
                        throw new DivideByZeroException();
                    return String.Join(" ", output.Split('|'));
                }
                catch (DivideByZeroException)
                {
                    var str = getBetween(page.Content, "== ביוגרפיה ==", "==");
                    var output = String.Join("", str.Split(']', '['));
                    return String.Join(" ", output.Split('|'));
                }
            }
            return null;
        }

        private async static Task<string> GetPageDateOfBirth(string name, WikiSite site)
        {
            var page = new WikiPage(site, name);
            var provider = WikiPageQueryProvider.FromOptions(PageQueryOptions.FetchContent | PageQueryOptions.ResolveRedirects);
            await page.RefreshAsync(provider);
            if (page.Content != null)
            {
                try
                {
                    var str = getBetween(page.Content, "תאריך לידה=", "\n");
                    var output = String.Join("", str.Split(']', '['));
                    if (output == "" || output == "\n")
                        throw new DivideByZeroException();
                    return String.Join(" ", output.Split('|'));
                }
                catch (DivideByZeroException)
                {
                    var str = getBetween(page.Content, "תאריך לידה =", "\n");
                    var output = String.Join("", str.Split(']', '['));
                    return String.Join(" ", output.Split('|'));
                }
            }
            return null;
        }

        private async static Task<string> GetUrl(string wdid)
        {
            if (wdid[0] == 'Q')
                return "www.wikidata.org/wiki/" + wdid;
            return "noWikiData";
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            try
            {
                if (strSource.Contains(strStart) && strSource.Contains(strEnd))
                {
                    int Start, End;
                    Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                    End = strSource.IndexOf(strEnd, Start);
                    return strSource.Substring(Start, End - Start);
                }

                return "";
            }
            catch
            {
                return "";
            }
        }

        private static void Set_alona_ben_keren(IMongoCollection<Person> personCollection)
        {
            var a1 = new MovieObject("אישה זרה", "1992", null);
            var a2 = new MovieObject("רחובות האתמול", "1989", null);
            var a3 = new MovieObject("כח משיכה", "1989", null);
            var a4 = new MovieObject("אבא גנוב 2", "1989", null);
            var a5 = new MovieObject("חימו מלך ירושלים", "1987", null);
            var a6 = new MovieObject("אבא גנוב", "1987", null);
            var a7 = new MovieObject("שוברים", "1985", null);
            var b1 = new MovieObject("אבא גנוב", "1987", null);
            var b2 = new MovieObject("אבא גנוב 2", "1989", null);
            var b3 = new MovieObject("אבא גנוב 3", "1991", null);
            var k1 = new MovieObject("בוקר טוב ילד", "2018", null);
            var k2 = new MovieObject("המועדון לספרות יפה של הגברת ינקלובה", "2017", null);
            var k3 = new MovieObject("ליליאן [תיעודי]", "2016", null);
            var k4 = new MovieObject("פרינסס", "2014", null);
            var k5 = new MovieObject("חלונות", "2014", null);
            var k6 = new MovieObject("בלילה 2", "2012", null);
            var k7 = new MovieObject("כלבת", "2010", null);
            var k8 = new MovieObject("שבעה", "2008", null);
            var k9 = new MovieObject("תנועה מגונה", "2006", null);
            var k10 = new MovieObject("מוכרחים להיות שמח", "2005", null);
            var k11 = new MovieObject("שנת אפס", "2004", null);
            var k12 = new MovieObject("יום יום", "1998", null);
            var k13 = new MovieObject("ביפ", "1997", null);
            var k14 = new MovieObject("כלבים לא נובחים בירוק", "1996", null);
            var k15 = new MovieObject("קופסא שחורה", "1993", null);
            var k16 = new MovieObject("כבלים", "1992", null);
            var k17 = new MovieObject("שורו", "1990", null);
            var k18 = new MovieObject("ברלין - ירושלים", "1989", null);
            var k19 = new MovieObject("אבא גנוב", "1987", null);

            var filter = Builders<Person>.Filter.Where(x => x.Name == "אלונה קמחי");
            MovieObject[] result = { a1, a2, a3, a4, a5, a6, a7 };
            var update = Builders<Person>.Update.Set(x => x.Acting, result);
            var options = new FindOneAndUpdateOptions<Person>();
            personCollection.FindOneAndUpdate(filter, update, options);

            var filter1 = Builders<Person>.Filter.Where(x => x.Name == "בן ציון");
            MovieObject[] result1 = { b1, b2, b3 };
            var update1 = Builders<Person>.Update.Set(x => x.Acting, result1);
            var options1 = new FindOneAndUpdateOptions<Person>();
            personCollection.FindOneAndUpdate(filter1, update1, options1);

            var filter2 = Builders<Person>.Filter.Where(x => x.Name == "קרן מור");
            MovieObject[] result2 = { k1, k2, k3, k4, k5, k6, k7, k8, k9, k10, k11, k12, k13, k14, k15, k16, k17, k18, k19 };
            var update2 = Builders<Person>.Update.Set(x => x.Acting, result2);
            var options2 = new FindOneAndUpdateOptions<Person>();
            personCollection.FindOneAndUpdate(filter2, update2, options2);

            var filter3 = Builders<Person>.Filter.Where(x => x.Name == "יהודה ברקן");
            var update3 = Builders<Person>.Update.Set(x => x.DiferentName, "יהודה בארקן");
            var options3 = new FindOneAndUpdateOptions<Person>();
            personCollection.FindOneAndUpdate(filter3, update3, options3);
        }
    }
}
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace WebAppMovies.Logic
{
    public class MoviesOperations
    {
        public IMongoCollection<movie> Collection { get; set; }

        public MoviesOperations(IMongoCollection<movie> collection)
        {
            this.Collection = collection;
        }

        public movie GetMovieById(string id)
        {
            var filter = Builders<movie>.Filter.Eq("_id", id);

            return Collection.Find(filter).First();
        }

        public movie GetMovieByName(string name)
        {
            var filter = Builders<movie>.Filter.Eq("MovieName", name);

            try
            {
                return Collection.Find(filter).First();
            }
            catch
            {
                return null;
            }
        }

        public dynamic GetAllMoviesNames()
        {
            var result = Collection.Aggregate().Project(c => new
            {
                c.MovieName
            }).ToList();

            return result;
        }

        public List<movie> GetallmoviesInYear(double year)
        {
            var result = Collection.Aggregate().Match(c => c.Year == year).ToList();

            return result;
        }

        public dynamic GetallmoviesByNumberOfWatchers(string num)
        {
            var result = Collection.Aggregate().Match(c => c.WatchNum == num).Project(c => new
            {
                c.Id,
                c.MovieName
            }).ToList();

            return result;
        }

        public List<movie> GetAllMoviesByYear(List<movie> movies, double year)
        {
            List<movie> NewMovies = new List<movie>();

            var MoviesInYear = GetallmoviesInYear(year);

            foreach (var movie in movies)
            {
                var movieName = movie.MovieName;

                foreach (var x in MoviesInYear)
                {
                    if (movieName.Equals(x.MovieName))
                    {
                        NewMovies.Add(movie);
                    }
                }
            }

            return NewMovies;
        }

        //public List<movie> GetAllMoviesByName(List<movie> movies, string name)
        //{
        //    List<movie> NewMovies = new List<movie>();

        //    var MoviesByName = GetMovieByName(name);

        //    foreach (var movie in movies)
        //    {
        //        var movieName = movie.MovieName;
        //        if (movieName.Equals(MoviesByName))
        //        {
        //            NewMovies.Add(movie);
        //        }

        //    }

        //    return NewMovies;
        //}
        public List<movie> MovieObject_to_movie(MovieObject[] input)
        {
            List<movie> NewMovies = new List<movie>();
            if (input != null)
            {
                foreach (var x in input)
                {
                    if (x.name != null)
                    {
                        var tmp = GetMovieByName(x.name);
                        if (tmp != null)
                            NewMovies.Add(GetMovieByName(x.name));
                    }
                    else if (x.name2 != null)
                    {
                        var tmp = GetMovieByName(x.name2);
                        if (tmp != null)
                            NewMovies.Add(GetMovieByName(x.name2));
                    }
                }

                return NewMovies;
            }
            return null;
        }

        public List<Actor> Actors_in_a_movie(string name)
        {
            List<Actor> output = new List<Actor>();
            movie CurrMovie = GetMovieByName(name);
            if (CurrMovie != null)
            {
                Actor[] actors = CurrMovie.actors;
                if (actors != null)
                {
                    foreach (Actor a in actors)
                    {
                        if (a != null)
                            output.Add(a);
                    }
                }
            }
            return output;
        }

        public List<movie> Names_to_movies(string[] names)
        {
            List<movie> NewMovies = new List<movie>();
            if (names != null)
            {
                foreach (var x in names)
                {
                    if (x != null)
                    {
                        var tmp = GetMovieByName(x);
                        if (tmp != null)
                            NewMovies.Add(tmp);
                    }
                }
                return NewMovies;
            }
            return null;
        }

        public void Build_act_and_char_field(List<movie> movies)
        {
            //List<movie> all_movies = Collection.Aggregate().ToList();

            foreach (movie m in movies)
            {
                var actors = m.actors;
                if (actors != null)
                {
                    foreach (Actor a in actors)
                    {
                        if (a.character != null)
                            m.actor_and_character += "actor: " + a.name + "\ncharacter: " + a.character + "\n";
                        else m.actor_and_character += "actor: " + a.name + "\n";
                    }
                }
            }
        }

        //public Actor[] GetAllActorsOfMovie(dynamic name)
        //{
        //    var Name = name.MovieName as string;
        //    var filter = Builders<movie>.Filter.Eq("MovieName", Name);

        //    var movie = collection.Find(filter).First();

        //    return movie.actors;
        //}

        //public Actor[] GetAllActorsOfMovie(string name)
        //{
        //    var filter = Builders<movie>.Filter.Eq("MovieName", name);

        //    var movie = collection.Find(filter).First();

        //    return movie.actors;
        //}

        //public List<movie> GetMoviesByActor(string actorName)
        //{
        //    List<movie> movies = new List<movie>();
        //    var miviesname = GetAllMoviesNames();
        //    foreach (var name in miviesname)
        //    {
        //        Actor[] actors = GetAllActorsOfMovie(name);
        //        if (actors == null)
        //            break;

        //        foreach (Actor actor in actors)
        //        {
        //            if (actor.name == actorName)
        //            {
        //                movies.Add(GetMovieByName(name));
        //                break;
        //            }
        //        }
        //    }

        //    return movies;
        //}

        //public dynamic allmoviesInYearand(Predicate<movie> p1, Predicate<movie> p2)
        //{
        //    var result = collation.Aggregate().Match(c => p1(c) & p2(c)).Project(c => new
        //    {
        //        c.MovieName,
        //    }).ToList();

        //    return result;
        //}

        //public void SaveToCsv<T>(List<T> reportData, string path)
        //{
        //    var lines = new List<string>();
        //    IEnumerable<PropertyDescriptor> props = TypeDescriptor.GetProperties(typeof(T)).OfType<PropertyDescriptor>();
        //    var header = string.Join(",", props.ToList().Select(x => x.Name));
        //    lines.Add(header);
        //    var valueLines = reportData.Select(row => string.Join(",", header.Split(',').Select(a => row.GetType().GetProperty(a).GetValue(row, null))));
        //    lines.AddRange(valueLines);
        //    File.WriteAllLines(path, lines.ToArray());
        //}
    }
}
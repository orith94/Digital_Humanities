using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAppMovies.Logic
{
    public class PersonOperations
    {
        private IMongoCollection<Person> Collection { get; set; }

        public PersonOperations(IMongoCollection<Person> collection)
        {
            this.Collection = collection;
        }

        public MovieObject[] GetAllMoviesByActor(string name)
        {
            var result = Collection.Aggregate().Match(c => c.Name == name);
            try
            {
                return result.First().Acting;
            }
            catch
            {
                return null;
            }
        }

        public List<movie> GetAllMoviesByActor(List<movie> movies, string name)
        {
            List<movie> NewMovies = new List<movie>();

            var actorsMovies = GetAllMoviesByActor(name);
            if (movies != null && actorsMovies != null)
            {
                foreach (var movie in movies)
                {
                    var movieName = movie.MovieName;

                    foreach (var x in actorsMovies)
                    {
                        if (x.name == movieName)
                        {
                            NewMovies.Add(movie);
                        }
                    }
                }

                return NewMovies;
            }
            return null;
        }

        public string[] GetAllMoviesByDirector(string name)
        {
            var result = Collection.Aggregate().Match(c => c.Name == name);
            try
            {
                return result.First().Directed;
            }
            catch
            {
                return null;
            }
        }

        public List<movie> GetAllMoviesByDirector(List<movie> movies, string name)
        {
            List<movie> NewMovies = new List<movie>();

            var directorMovies = GetAllMoviesByDirector(name);
            if (movies != null && directorMovies != null)
            {
                foreach (var movie in movies)
                {
                    var movieName = movie.MovieName;

                    foreach (var x in directorMovies)
                    {
                        if (movieName.Equals(x))
                        {
                            NewMovies.Add(movie);
                        }
                    }
                }
                return NewMovies;
            }
            return null;
        }

        public string[] GetAllMoviesByProducer(string name)
        {
            var result = Collection.Aggregate().Match(c => c.Name == name);
            try
            {
                return result.First().Production;
            }
            catch
            {
                return null;
            }
        }

        public List<movie> GetAllMoviesByProducer(List<movie> movies, string name)
        {
            List<movie> NewMovies = new List<movie>();

            var producerMovies = GetAllMoviesByProducer(name);
            if (movies != null && producerMovies != null)
            {
                foreach (var movie in movies)
                {
                    var movieName = movie.MovieName;

                    foreach (var x in producerMovies)
                    {
                        if (movieName.Contains(x))
                        {
                            NewMovies.Add(movie);
                        }
                    }
                }
                return NewMovies;
            }
            return null;
        }

        public string[] GetAllMoviesByScreenwriter(string name)
        {
            var result = Collection.Aggregate().Match(c => c.Name == name);

            try
            {
                return result.First().תסריט;
            }
            catch
            {
                return null;
            }
        }

        public List<movie> GetAllMoviesByScreenwriter(List<movie> movies, string name)
        {
            List<movie> NewMovies = new List<movie>();

            var screenwriterMovies = GetAllMoviesByScreenwriter(name);
            if (movies != null && screenwriterMovies != null)
            {
                foreach (var movie in movies)
                {
                    var movieName = movie.MovieName;

                    foreach (var x in screenwriterMovies)
                    {
                        if (movieName == x)
                        {
                            NewMovies.Add(movie);
                        }
                    }
                }

                return NewMovies;
            }
            return null;
        }

        public Person GetPerson(string name)
        {
            try
            {
                var result = Collection.Aggregate().Match(c => c.Name == name).Single();
                return result;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<string> Get_all_persons_without_biography()
        {
            List<string> all_names = new List<string>();
            //string[] all_names;
            try
            {
                var all_persons = Collection.Aggregate().Match(c => c.Biography == null || c.Biography == "").ToList();
            }
            catch (Exception ex)
            {
                throw;
            }

            return all_names;
        }

        public List<string> Get_all_persons_string()
        {
            List<string> all_names = new List<string>();
            //string[] all_names;
            var all_persons = Collection.Aggregate().ToList();
            foreach (var p in all_persons)
            {
                if (p != null)
                    all_names.Add(p.Name);
            }
            return all_names;
        }

        public List<Person> Get_all_persons()
        {
            List<Person> all_names = new List<Person>();
            //string[] all_names;
            var all_persons = Collection.Aggregate().ToList();
            foreach (var p in all_persons)
            {
                if (p != null)
                    all_names.Add(p);
            }
            return all_names;
        }

        public List<string> Get_all_persons_without_date_of_birth()
        {
            List<string> all_names = new List<string>();
            //string[] all_names;
            var all_persons = Collection.Aggregate().Match(c => c.DateOfBirth == null || c.DateOfBirth == "").ToList();
            foreach (var p in all_persons)
            {
                if (p != null)
                    all_names.Add(p.Name);
            }
            return all_names;
        }

        public void Update_directed()
        {
            int count = 0;
            List<Person> all_persons = Collection.Aggregate().ToList();
            foreach (Person p in all_persons)
            {
                count++;
                if (p.Directed != null)
                {
                    p.Directed2 = new string[] { };
                    List<string> tmp = new List<string>();
                    try
                    {
                        foreach (var i in p.Directed)
                            tmp.Add((string)i);
                        p.Directed2 = tmp.ToArray();
                    }
                    catch
                    {
                        Console.WriteLine(p.Name);
                        Console.WriteLine(count);
                    }
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IMDBRipper
{
    class Program
    {
        static void Main(string[] args)
        {
            var moviePath = "Data/data_movies.tsv";
            var movies = ExtractMovies(moviePath);

            var actorPath = "Data/data_actors.tsv";
            ExtractActors(actorPath,movies);

            Console.ReadKey();
        }

        private static void ExtractActors(string actorPath, List<Movie> movies)
        {
            string line; 
            var reader = new StreamReader(actorPath);
            var actorList = new List<Actor>();

            while((line = reader.ReadLine()) != null)
            {
                var actor = Actor.Parse(line);
                if(movies.Select(x=> x.Id).Contains(actor.MovieId))
                {
                    actorList.Add(actor);
                }
            }

            GenerateActorTSV(actorList);
        }

        public static List<Movie> ExtractMovies(string moviePath)
        {
            var fileLines = File.ReadAllLines(moviePath);
            var movieList = new List<Movie>();

            //skip first line (titles)
            var isTitle = true;

            foreach (var line in fileLines)
            {
                if (isTitle)
                {
                    isTitle = false;
                    continue;
                }

                movieList.Add(Movie.Parse(line));
            }


            var newDataset = movieList.Where(x => x.StartYear > 2000 && x.RuntimeMinutes > 120 && x.TitleType == "movie").ToList();
            GenerateMovieTSV(newDataset);
            return newDataset;
        }

        public static void GenerateActorTSV(List<Actor> actors)
        {
            List<string> lines = new List<string>();
            lines.Add($"tconst	ordering	nconst	category	job	characters");
            foreach (var actor in actors)
            {
                lines.Add(actor.MovieId+ "\t" +
                    actor.Ordering + "\t" +
                    actor.Id + "\t" +
                    actor.Category + "\t" +
                    actor.Job + "\t" +
                    "["+actor.Characters.Aggregate((x,y) => x+","+y)+"]"+ "\t" 
                    );
            }
            File.WriteAllText("C:\\temp\\actors.tsv", lines.Aggregate((x, y) => x + Environment.NewLine + y));
        }


        public static void GenerateMovieTSV(List<Movie> movies)
        {
            List<string> lines = new List<string>();
            lines.Add($"tconst	titleType	primaryTitle	originalTitle	isAdult	startYear	endYear	runtimeMinutes	genres");
            foreach(var movie in movies)
            {
                lines.Add(movie.Id + "\t"+
                    movie.TitleType + "\t"+
                    movie.PrimaryTitle + "\t"+
                    movie.OriginalTitle + "\t"+
                    movie.IsAdult +"\t"+
                    movie.StartYear + "\t" +
                    "X" + "\t" +
                    movie.RuntimeMinutes+ "\t" +
                    movie.Genres.Aggregate((x,y) => x+","+y) + "\t" 
                    );
            }
            File.WriteAllText("C:\\temp\\movies.tsv", lines.Aggregate((x,y) => x +Environment.NewLine+y));
        }
        
    }
}

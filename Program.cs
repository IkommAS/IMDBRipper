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
            var moviePath = "Data/movies.tsv";
            var movies = ExtractMovies(moviePath);

            var actorMovieRelationPath = "Data/movies_actors.tsv";
            var relations = ExtractMoviesActorsRelation(actorMovieRelationPath,movies);

            var actorPath = "Data/data_actors_name.tsv";
            ExtractActorsRelation(actorPath, relations);

            Console.ReadKey();
        }

        private static void ExtractActorsRelation(string actorPath, List<MoviesActors> relations)
        {
            var actorHash = relations.Select(x => x.ActorId).ToHashSet();

            string line;
            var reader = new StreamReader(actorPath);
            var actorList = new List<Actor>();

            while ((line = reader.ReadLine()) != null)
            {
                var actor = Actor.Parse(line);
                if (actorHash.Contains(actor.Id))
                {
                    actorList.Add(actor);
                }
            }
            GenerateActorTSV(actorList);
        }

        private static List<MoviesActors> ExtractMoviesActorsRelation(string actorMovieRelationPath, List<Movie> movies)
        {
            string line; 
            var reader = new StreamReader(actorMovieRelationPath);
            var moviesActorList = new List<MoviesActors>();
            var moviesActorHash = movies.Select(x => x.Id).ToHashSet();

            while ((line = reader.ReadLine()) != null)
            {
                var actor = MoviesActors.Parse(line);
                if(moviesActorHash.Contains(actor.MovieId))
                {
                    moviesActorList.Add(actor);
                }
            }

            GenerateMoviesActorTSV(moviesActorList);
            return moviesActorList;
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

        public static void GenerateMoviesActorTSV(List<MoviesActors> movieActors)
        {
            List<string> lines = new List<string>();
            lines.Add($"tconst	ordering	nconst	category	job	characters");
            foreach (var ma in movieActors)
            {
                lines.Add(ma.MovieId+ "\t" +
                    ma.Ordering + "\t" +
                    ma.ActorId + "\t" +
                    ma.Category + "\t" +
                    ma.Job + "\t" +
                    "["+(ma.Characters.Count()>0?ma.Characters.Aggregate((x,y) => x+","+y):"")+"]"+ "\t" 
                    );
            }
            File.WriteAllText("C:\\temp\\movies_actors.tsv", lines.Aggregate((x, y) => x + Environment.NewLine + y));
        }

        public static void GenerateActorTSV(List<Actor> actors)
        {
            List<string> lines = new List<string>();
            lines.Add($"nconst primaryName birthYear deathYear   primaryProfession knownForTitles");
            foreach (var actor in actors)
            {
                lines.Add(actor.Id + "\t" +
                    actor.Name + "\t" +
                    actor.BirthYear + "\t" +
                    actor.DeathYear + "\t" +
                    actor.PrimaryProfession + "\t" +
                    actor.KnownForTitles + "\t"
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

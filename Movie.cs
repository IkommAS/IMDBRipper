using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBRipper
{
    public class Movie
    {
        public string Id { get; set; }
        public string TitleType { get; set; }
        public string OriginalTitle { get; set; }
        public string PrimaryTitle { get; set; }
        public bool IsAdult { get; set; }
        public int StartYear { get; set; }
        public int RuntimeMinutes { get; set; }
        public List<string> Genres { get; set; }


        public static Movie Parse(string lineInput)
        {
            var movie = new Movie();

            var lineTabs = lineInput.Split('\t');

            movie.Id = lineTabs[0];
            movie.TitleType = lineTabs[1];
            movie.PrimaryTitle= lineTabs[2];
            movie.OriginalTitle = lineTabs[3];
            movie.IsAdult = string.Equals(lineTabs[4], "1");
            movie.StartYear = string.IsNullOrWhiteSpace(lineTabs[5]) || string.Equals(lineTabs[5], "\\N") ? 0 : int.Parse(lineTabs[5]);
            //skipping endYear
            var runTimeParsed = 0;
            int.TryParse(lineTabs[7], out runTimeParsed);
            movie.RuntimeMinutes = runTimeParsed;
            movie.Genres = ParseGenreTab(lineTabs[8]);

            return movie;
        }

        

        private static List<string> ParseGenreTab(string v)
        {
            return v.Split(',').OfType<string>().ToList();
        }
    }
}

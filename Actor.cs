using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBRipper
{
    //tconst	ordering	nconst	category	job	characters
    class Actor
    {
        public string MovieId { get; set; }
        public int Ordering { get; set; }
        public string Id { get; set; }
        public string Category { get; set; }
        public string Job { get; set; }
        public List<string> Characters { get; set; }


        public static Actor Parse(string lineInput)
        {
            var actor = new Actor();

            var lineTabs = lineInput.Split('\t');

            actor.MovieId = lineTabs[0];
            var ordParsed = 0;
            var orderParsedOK =int.TryParse(lineTabs[1], out ordParsed);
            if (orderParsedOK)
            {
                actor.Ordering = ordParsed;
            }
            
            actor.Id = lineTabs[2];
            actor.Category = lineTabs[3];
            actor.Job = lineTabs[4];
            actor.Characters = ParseCharacters(lineTabs[5]);

            return actor;
        }

        private static List<string> ParseCharacters(string v)
        {
            if (string.Equals(v, "\\N")|| v.Length < 1 || !v.Contains('['))
            {
                return new List<string>();
            }

            //remove brackets '[' ']'
            var cleansedString = v.Remove(0, 1);
            cleansedString = cleansedString.Remove(cleansedString.Length - 1, 1);

            var split = cleansedString.Split(',');

            return split.ToList();
        }
    }
}

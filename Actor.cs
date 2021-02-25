using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDBRipper
{
    //nconst primaryName birthYear deathYear   primaryProfession knownForTitles
    public class Actor
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int BirthYear { get; set; }
        public int DeathYear { get; set; }
        public string PrimaryProfession { get; set; }
        public string KnownForTitles { get; set; }

        public static Actor Parse(string lineInput)
        {
            var actor = new Actor();

            var lineTabs = lineInput.Split('\t');

            actor.Id = lineTabs[0];

            actor.Name = lineTabs[1];

            var birthParsed = 0;
            var orderParsedOK = int.TryParse(lineTabs[2], out birthParsed);
            if (orderParsedOK)
            {
                actor.BirthYear = birthParsed;
            }

            var deathParsed = 0;
            var deathParsedOk = int.TryParse(lineTabs[3], out deathParsed);
            if (deathParsedOk)
            {
                actor.DeathYear = deathParsed;
            }
            actor.PrimaryProfession = lineTabs[4];
            actor.KnownForTitles = lineTabs[5];

            return actor;
        }
    }
}

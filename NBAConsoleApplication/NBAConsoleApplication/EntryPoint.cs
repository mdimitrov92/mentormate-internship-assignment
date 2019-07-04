namespace NBAConsoleApplication
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Newtonsoft.Json;

    public class EntryPoint
    {
        public static void Main()
        {
            IEnumerable<Player> players = ParsePlayers();
            IEnumerable<Player> qualifiedPlayers = GetQualifiedPlayersInCorrectOrder(players);
            SaveToFile(qualifiedPlayers.ToList());
        }

        private static void SaveToFile(ICollection<Player> players)
        {
            string path = Console.ReadLine();

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.WriteLine("Name,Rating");

                foreach (Player p in players)
                {
                    sw.WriteLine(p.Name + ", " + p.Rating);
                }
            }
        }

        private static IEnumerable<Player> GetQualifiedPlayersInCorrectOrder(IEnumerable<Player> players)
        {
            var playersWhoQualify = new List<Player>();

            int maxYearsPlayed = int.Parse(Console.ReadLine());
            int minRating = int.Parse(Console.ReadLine());

            foreach (Player p in players)
            {
                int yearsPlayed = DateTime.Now.Year - p.PlayingSince + 1;

                bool hasEnoughRating = p.Rating >= minRating;
                bool isInRangeYears = yearsPlayed <= maxYearsPlayed;

                if (hasEnoughRating && isInRangeYears)
                {
                    playersWhoQualify.Add(p);
                }
            }

            return playersWhoQualify.OrderByDescending(p => p.Rating);
        }

        private static IEnumerable<Player> ParsePlayers()
        {
            List<Player> players = new List<Player>();
            string path = Console.ReadLine();

            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();

                players = JsonConvert.DeserializeObject<List<Player>>(json);
            }

            return players;
        }
    }
}
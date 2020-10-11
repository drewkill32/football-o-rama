using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class Game
    {
        public string Id { get; set; }


        public DateTimeOffset Scheduled { get; set; }

        public string Home { get; set; }

        public string Away { get; set; }

        public GameStatus Status { get; set; }

        public int? AwayPoints { get; set; }

        public int? HomePoints { get; set; }

    }

    public class Week
    {
        public string Id { get; set; }

        public int Number { get; set; }

        public string Season { get; set; }

        public string Type { get; set; }

        public List<Game> Games { get; set; } = new List<Game>();
    }
}

using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class LeagueStanding
    {
        [PrimaryKey]
        public string? TeamId { get; set; }
        public string? TeamName { get; set; }
        public int? Ranking { get; set; }
        public int Games { get; set; }
        public int Wins { get; set; }
        public int Ties { get; set; }
        public int Losses { get; set; }
        public int Goals { get; set; }
        public int GoalsAgainst { get; set; }
        public int GoalDifference { get; set; }
        public int? Points { get; set; }
        public double? PointsPerGame { get; set; }
        public DateTime LastUpdated { get; set; }

    }
}

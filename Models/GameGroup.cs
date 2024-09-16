using Sporttiporssi.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sporttiporssi.Models
{
    public class GameGroup : ObservableCollection<GameStatsPlayer>
    {
        public string LineName { get; set; }
        public GameGroup(string lineName, IEnumerable<GameStatsPlayer> players) : base(players)
        {
            LineName = lineName;
        }
    }
}

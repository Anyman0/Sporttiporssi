using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace Sporttiporssi.Models.DTOs
{
    public class LiigaGameDto : INotifyPropertyChanged
    {
        private bool _isRosterExpanded;
        private bool _isStatsExpanded;
        //public ObservableCollection<GameStatsPlayer> _homeTeamPlayers;
        //public ObservableCollection<GameStatsPlayer> _awayTeamPlayers;
        public int GameId { get; set; }
        public int Id { get; set; }
        public int Season { get; set; }
        public DateTime Start { get; set; }
        public Team HomeTeam { get; set; }
        public Team AwayTeam { get; set; }
        public string HomeTeamLogo { get; set; }
        public string AwayTeamLogo { get; set; }
        public string FinishedType { get; set; }
        public string LastHomeGames { get; set; }
        public string LastAwayGames { get; set; }
        public bool Started { get; set; }
        public bool Ended { get; set; }
        public string BuyTicketsUrl { get; set; }
        public bool Stale { get; set; }
        public string Serie { get; set; }
        public DateTime LastUpdated { get; set; }

        public bool IsRosterExpanded
        {
            get => _isRosterExpanded;
            set
            {
                if (_isRosterExpanded != value)
                {
                    _isRosterExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsStatsExpanded
        {
            get => _isStatsExpanded;
            set
            {
                if (_isStatsExpanded != value)
                {
                    _isStatsExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        //public ObservableCollection<GameStatsPlayer> HomeTeamPlayers
        //{
        //    get => _homeTeamPlayers;
        //    set
        //    {
        //        _homeTeamPlayers = value;
        //        OnPropertyChanged(nameof(HomeTeamPlayers));
        //    }
        //}

        //public ObservableCollection<GameStatsPlayer> AwayTeamPlayers
        //{
        //    get => _awayTeamPlayers;
        //    set
        //    {
        //        _awayTeamPlayers = value;
        //        OnPropertyChanged(nameof(AwayTeamPlayers));
        //    }
        //}

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public class Team
    {
        public string TeamId { get; set; }
        public string TeamPlaceholder { get; set; }
        public string TeamName { get; set; }
        public int Goals { get; set; }
        public string TimeOut { get; set; }
        public int PowerplayInstances { get; set; }
        public int PowerplayGoals { get; set; }
        public int ShortHandedInstances { get; set; }
        public int ShortHandedGoals { get; set; }
        public int? Ranking { get; set; }
        public DateTime GameStartDateTime { get; set; }
    }
  
}

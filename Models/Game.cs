using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace Sporttiporssi.Models
{
    public class Game : INotifyPropertyChanged
    {
        private bool _isRosterExpanded;
        private bool _isStatsExpanded;
       
        [PrimaryKey]
        public int Id { get; set; }
        public string? Serie { get; set; }
        public int Season { get; set; }
        public string HomeTeamName { get; set; }
        public string AwayTeamName { get; set; }
        public string HomeTeamId { get; set; }
        public string AwayTeamId { get; set; }
        public string HomeTeamLogo {  get; set; }
        public string AwayTeamLogo { get; set; }
        public string HomeTeamPlaceholder { get; set; }
        public string AwayTeamPlaceholder { get; set; }
        public int HomeTeamGoals { get; set; }
        public int AwayTeamGoals { get; set; }
        public int GameTime { get; set; }
        public bool Started { get; set; }
        public bool Ended { get; set; }
        public bool HomeTeamWinner { get; set; }
        public int? Spectators { get; set; }
        public string? HomeTeamRanking { get; set; }
        public string? AwayTeamRanking { get; set; }
        public string PlayOffPair { get; set; }
        public string PlayOffPhase { get; set; }
        public string PlayOffReqWins { get; set; }
        public DateTime Start { get; set; }
        public string FinishedType { get; set; }
        public string LastHomeGames { get; set; }
        public string LastAwayGames { get; set; }
        public DateTime LastUpdated { get; set; }
        private ObservableCollection<GameGroup> _groupedPlayers;
        public ObservableCollection<GameGroup> GroupedPlayers
        {
            get => _groupedPlayers;
            set
            {
                _groupedPlayers = value;
                OnPropertyChanged(nameof(GroupedPlayers));
            }
        }
        private ObservableCollection<GameGroup> _awayGroupedPlayers;
        public ObservableCollection<GameGroup> AwayGroupedPlayers
        {
            get => _awayGroupedPlayers;
            set
            {
                _awayGroupedPlayers = value;
                OnPropertyChanged(nameof(AwayGroupedPlayers));
            }
        }

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
        
        private int _homeTeamRank;
        private int _awayTeamRank;

        public int HomeTeamRank
        {
            get => _homeTeamRank;
            set
            {
                _homeTeamRank = value;
                OnPropertyChanged(nameof(HomeTeamRank));
            }
        }

        public int AwayTeamRank
        {
            get => _awayTeamRank;
            set
            {
                _awayTeamRank = value;
                OnPropertyChanged(nameof(AwayTeamRank));
            }
        }

        public string EventStartLocalTime
        {
            get
            {
                if (!Ended)
                {
                    try
                    {
                        DateTime eventDateTimeUtc = Start;
                        // Find the Finnish time zone
                        TimeZoneInfo finlandTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Europe/Helsinki");
                        // Convert the UTC time to Finnish local time
                        DateTime localDateTime = TimeZoneInfo.ConvertTimeFromUtc(eventDateTimeUtc, finlandTimeZone);

                        // Return time in "HH:mm" format
                        return localDateTime.ToString("HH:mm");
                    }
                    catch (TimeZoneNotFoundException ex)
                    {
                        // Handle specific time zone not found exception
                        return $"Time zone not found: {ex.Message}";
                    }
                    catch (InvalidTimeZoneException ex)
                    {
                        // Handle invalid time zone data
                        return $"Invalid time zone data: {ex.Message}";
                    }
                    catch (Exception ex)
                    {
                        // Handle general exceptions
                        return $"Error: {ex.Message}";
                    }
                }
                else
                {
                    var homeGoals = HomeTeamGoals;
                    var awayGoals = AwayTeamGoals;
                    return $"{HomeTeamGoals} - {AwayTeamGoals}";
                }
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class GamePlayer
    {
        public string? Name { get; set; }
        public int Number { get; set; }
        public string? Position { get; set; }
    }

    public class GameEvent
    {
        public string? Name { get; set; }        
        public string? Event { get; set; } // For example score, penalty
        public double? EventTime { get; set; }
    }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Sporttiporssi.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using Sporttiporssi.Models.DTOs;
using Sporttiporssi.Helpers;
using Sporttiporssi.Services;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using CommunityToolkit.Maui.Core.Extensions;

namespace Sporttiporssi.ViewModels
{
    public class GamesViewModel : INotifyPropertyChanged
    {
        private readonly LeagueGamesService _gamesService;
        private ObservableCollection<Game> _gamesByDate { get; set; }
        private ObservableCollection<Event> _hockeyGamesByDate { get; set; }
        private LiigaGameStatsDto _gameStats {  get; set; }

        private readonly string Liiga_baseApiUrl = "https://www.liiga.fi/api/v2/";

        private List<string> roleOrder = new List<string> { "OP", "VP", "OL", "KH", "VL" };
              
       
        public ObservableCollection<Game> GamesByDate
        {
            get => _gamesByDate;
            set
            {
                _gamesByDate = value;
                OnPropertyChanged(nameof(GamesByDate));
            }
        }

        public ObservableCollection<Event> HockeyGamesByDate
        {
            get => _hockeyGamesByDate;
            set
            {
                _hockeyGamesByDate = value;
                OnPropertyChanged(nameof(HockeyGamesByDate));
            }
        }

        public LiigaGameStatsDto GameStats
        {
            get => _gameStats;
            set
            {
                _gameStats = value;
                OnPropertyChanged(nameof(GameStats));
            }
        }

        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged(nameof(IsLoading));
            }
        }
      
        public GamesViewModel(LeagueGamesService gamesService) 
        {
            _gamesService = gamesService;          
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async Task LoadGamesByDate(DateTime date)
        {
            IsLoading = true;           
            var games = await _gamesService.LoadGamesByDate(date);          
            GamesByDate = new ObservableCollection<Game>(games);
            IsLoading = false;
        }
        public async Task GetGameStats(string hometeam, string awayteam, DateTime gameDate, int gameId)
        {
            var needToUpdate = false;         
            var firstGameDate = GamesByDate.OrderBy(g => g.Start).Select(s => s.Start).FirstOrDefault();
            var dateToCompare = new DateTime(
                firstGameDate.Year,
                firstGameDate.Month,
                firstGameDate.Day,
                13, // hour in 24-hour format
                5,  // minutes
                0   // seconds
            );
            if (DateTime.UtcNow.ToLocalTime() > dateToCompare)
            {
                needToUpdate = true;
                if (GameStats != null)
                {
                    needToUpdate = true;
                }
                if (needToUpdate)
                {
                    IsLoading = true;
                    var gameStats = await _gamesService.GetGameRosterAsync(hometeam, awayteam, gameDate);
                    var game = GamesByDate.Where(g => g.Id == gameId).FirstOrDefault();
                    // Group home team players
                    var GroupedPlayers = new ObservableCollection<GameGroup>
                    {
                    new GameGroup("Line 1", gameStats.HomeTeamPlayers.Where(p => p.Line == 1 && p.RoleCode != "MV")
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 2", gameStats.HomeTeamPlayers.Where(p => p.Line == 2 && p.RoleCode != "MV")
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 3", gameStats.HomeTeamPlayers.Where(p => p.Line == 3)
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 4", gameStats.HomeTeamPlayers.Where(p => p.Line == 4)
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Goalie", gameStats.HomeTeamPlayers.Where(p => p.Line == 1 && p.RoleCode == "MV")),
                    };

                    var awayGroupedPlayers = new ObservableCollection<GameGroup>
                    {
                    new GameGroup("Line 1", gameStats.AwayTeamPlayers.Where(p => p.Line == 1 && p.RoleCode != "MV")
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 2", gameStats.AwayTeamPlayers.Where(p => p.Line == 2 && p.RoleCode != "MV")
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 3", gameStats.AwayTeamPlayers.Where(p => p.Line == 3)
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Line 4", gameStats.AwayTeamPlayers.Where(p => p.Line == 4)
                    .OrderBy(p => roleOrder.IndexOf(p.RoleCode))),
                    new GameGroup("Goalie", gameStats.AwayTeamPlayers.Where(p => p.Line == 1 && p.RoleCode == "MV")),
                    };

                    // Assign grouped players to the game
                    game.GroupedPlayers = GroupedPlayers;
                    game.AwayGroupedPlayers = awayGroupedPlayers;
                    GameStats = gameStats;
                    IsLoading = false;
                }
            }
        }
       
        public void ToggleRosterExpansion(Game game)
        {
            game.IsRosterExpanded = !game.IsRosterExpanded;
        }

        public void ToggleStatsExpansion(Event game)
        {
            game.IsStatsExpanded = !game.IsStatsExpanded;
        }
      
    }
}

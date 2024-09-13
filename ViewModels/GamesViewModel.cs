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

namespace Sporttiporssi.ViewModels
{
    public class GamesViewModel : INotifyPropertyChanged
    {
        private readonly LeagueGamesService _gamesService;
        private ObservableCollection<Game> _gamesByDate { get; set; }
        private ObservableCollection<Event> _hockeyGamesByDate { get; set; }
        private LiigaGameStatsDto _gameStats {  get; set; }

        private readonly string Liiga_baseApiUrl = "https://www.liiga.fi/api/v2/";

        // ROSTER SETUP
        // Properties for binding in XAML
        private GameStatsPlayer _team1Player1;
        private GameStatsPlayer _team1Player2;
        private GameStatsPlayer _team1Player3;
        private GameStatsPlayer _team1Player4;
        private GameStatsPlayer _team1Player5;       
        private GameStatsPlayer _team1Player6;
        private GameStatsPlayer _team1Player7;
        private GameStatsPlayer _team1Player8;
        private GameStatsPlayer _team1Player9;
        private GameStatsPlayer _team1Player10;
        private GameStatsPlayer _team1Player11;
        private GameStatsPlayer _team1Player12;
        private GameStatsPlayer _team1Player13;
        private GameStatsPlayer _team1Player14;
        private GameStatsPlayer _team1Player15;
        private GameStatsPlayer _team1Player16;
        private GameStatsPlayer _team1Player17;
        private GameStatsPlayer _team1Player18;
        private GameStatsPlayer _team1Player19;
        private GameStatsPlayer _team1Player20;
        private GameStatsPlayer _team2Player1;
        private GameStatsPlayer _team2Player2;
        private GameStatsPlayer _team2Player3;
        private GameStatsPlayer _team2Player4;
        private GameStatsPlayer _team2Player5;
        private GameStatsPlayer _team2Player6;
        private GameStatsPlayer _team2Player7;
        private GameStatsPlayer _team2Player8;
        private GameStatsPlayer _team2Player9;
        private GameStatsPlayer _team2Player10;
        private GameStatsPlayer _team2Player11;
        private GameStatsPlayer _team2Player12;
        private GameStatsPlayer _team2Player13;
        private GameStatsPlayer _team2Player14;
        private GameStatsPlayer _team2Player15;
        private GameStatsPlayer _team2Player16;
        private GameStatsPlayer _team2Player17;
        private GameStatsPlayer _team2Player18;
        private GameStatsPlayer _team2Player19;
        private GameStatsPlayer _team2Player20;
        private GameStatsPlayer _homeGoalie;
        private GameStatsPlayer _awayGoalie;


        public GameStatsPlayer Team1Player1
        {
            get => _team1Player1;
            set
            {
                _team1Player1 = value;
                OnPropertyChanged(nameof(Team1Player1));
            }
        }
        public GameStatsPlayer Team1Player2
        {
            get => _team1Player2;
            set
            {
                _team1Player2 = value;
                OnPropertyChanged(nameof(Team1Player2));
            }
        }

        public GameStatsPlayer Team1Player3
        {
            get => _team1Player3;
            set
            {
                _team1Player3 = value;
                OnPropertyChanged(nameof(Team1Player3));
            }
        }

        public GameStatsPlayer Team1Player4
        {
            get => _team1Player4;
            set
            {
                _team1Player4 = value;
                OnPropertyChanged(nameof(Team1Player4));
            }
        }

        public GameStatsPlayer Team1Player5
        {
            get => _team1Player5;
            set
            {
                _team1Player5 = value;
                OnPropertyChanged(nameof(Team1Player5));
            }
        }

        public GameStatsPlayer Team2Player1
        {
            get => _team2Player1;
            set
            {
                _team2Player1 = value;
                OnPropertyChanged(nameof(Team2Player1));
            }
        }
        public GameStatsPlayer Team2Player2
        {
            get => _team2Player2;
            set
            {
                _team2Player2 = value;
                OnPropertyChanged(nameof(Team2Player2));
            }
        }

        public GameStatsPlayer Team2Player3
        {
            get => _team2Player3;
            set
            {
                _team2Player3 = value;
                OnPropertyChanged(nameof(Team2Player3));
            }
        }

        public GameStatsPlayer Team2Player4
        {
            get => _team2Player4;
            set
            {
                _team2Player4 = value;
                OnPropertyChanged(nameof(Team2Player4));
            }
        }

        public GameStatsPlayer Team2Player5
        {
            get => _team2Player5;
            set
            {
                _team2Player5 = value;
                OnPropertyChanged(nameof(Team2Player5));
            }
        }

        public GameStatsPlayer Team1Player6
        {
            get => _team1Player6;
            set
            {
                _team1Player6 = value;
                OnPropertyChanged(nameof(Team1Player6));
            }
        }

        public GameStatsPlayer Team1Player7
        {
            get => _team1Player7;
            set
            {
                _team1Player7 = value;
                OnPropertyChanged(nameof(Team1Player7));
            }
        }

        public GameStatsPlayer Team1Player8
        {
            get => _team1Player8;
            set
            {
                _team1Player8 = value;
                OnPropertyChanged(nameof(Team1Player8));
            }
        }

        public GameStatsPlayer Team1Player9
        {
            get => _team1Player9;
            set
            {
                _team1Player9 = value;
                OnPropertyChanged(nameof(Team1Player9));
            }
        }

        public GameStatsPlayer Team1Player10
        {
            get => _team1Player10;
            set
            {
                _team1Player10 = value;
                OnPropertyChanged(nameof(Team1Player10));
            }
        }

        public GameStatsPlayer Team1Player11
        {
            get => _team1Player11;
            set
            {
                _team1Player11 = value;
                OnPropertyChanged(nameof(Team1Player11));
            }
        }

        public GameStatsPlayer Team1Player12
        {
            get => _team1Player12;
            set
            {
                _team1Player12 = value;
                OnPropertyChanged(nameof(Team1Player12));
            }
        }

        public GameStatsPlayer Team1Player13
        {
            get => _team1Player13;
            set
            {
                _team1Player13 = value;
                OnPropertyChanged(nameof(Team1Player13));
            }
        }

        public GameStatsPlayer Team1Player14
        {
            get => _team1Player14;
            set
            {
                _team1Player14 = value;
                OnPropertyChanged(nameof(Team1Player14));
            }
        }

        public GameStatsPlayer Team1Player15
        {
            get => _team1Player15;
            set
            {
                _team1Player15 = value;
                OnPropertyChanged(nameof(Team1Player15));
            }
        }

        public GameStatsPlayer Team1Player16
        {
            get => _team1Player16;
            set
            {
                _team1Player16 = value;
                OnPropertyChanged(nameof(Team1Player16));
            }
        }

        public GameStatsPlayer Team1Player17
        {
            get => _team1Player17;
            set
            {
                _team1Player17 = value;
                OnPropertyChanged(nameof(Team1Player17));
            }
        }

        public GameStatsPlayer Team1Player18
        {
            get => _team1Player18;
            set
            {
                _team1Player18 = value;
                OnPropertyChanged(nameof(Team1Player18));
            }
        }

        public GameStatsPlayer Team1Player19
        {
            get => _team1Player19;
            set
            {
                _team1Player19 = value;
                OnPropertyChanged(nameof(Team1Player19));
            }
        }

        public GameStatsPlayer Team1Player20
        {
            get => _team1Player20;
            set
            {
                _team1Player20 = value;
                OnPropertyChanged(nameof(Team1Player20));
            }
        }

        public GameStatsPlayer Team2Player6
        {
            get => _team2Player6;
            set
            {
                _team2Player6 = value;
                OnPropertyChanged(nameof(Team2Player6));
            }
        }

        public GameStatsPlayer Team2Player7
        {
            get => _team2Player7;
            set
            {
                _team2Player7 = value;
                OnPropertyChanged(nameof(Team2Player7));
            }
        }

        public GameStatsPlayer Team2Player8
        {
            get => _team2Player8;
            set
            {
                _team2Player8 = value;
                OnPropertyChanged(nameof(Team2Player8));
            }
        }

        public GameStatsPlayer Team2Player9
        {
            get => _team2Player9;
            set
            {
                _team2Player9 = value;
                OnPropertyChanged(nameof(Team2Player9));
            }
        }

        public GameStatsPlayer Team2Player10
        {
            get => _team2Player10;
            set
            {
                _team2Player10 = value;
                OnPropertyChanged(nameof(Team2Player10));
            }
        }

        public GameStatsPlayer Team2Player11
        {
            get => _team2Player11;
            set
            {
                _team2Player11 = value;
                OnPropertyChanged(nameof(Team2Player11));
            }
        }

        public GameStatsPlayer Team2Player12
        {
            get => _team2Player12;
            set
            {
                _team2Player12 = value;
                OnPropertyChanged(nameof(Team2Player12));
            }
        }

        public GameStatsPlayer Team2Player13
        {
            get => _team2Player13;
            set
            {
                _team2Player13 = value;
                OnPropertyChanged(nameof(Team2Player13));
            }
        }

        public GameStatsPlayer Team2Player14
        {
            get => _team2Player14;
            set
            {
                _team2Player14 = value;
                OnPropertyChanged(nameof(Team2Player14));
            }
        }

        public GameStatsPlayer Team2Player15
        {
            get => _team2Player15;
            set
            {
                _team2Player15 = value;
                OnPropertyChanged(nameof(Team2Player15));
            }
        }

        public GameStatsPlayer Team2Player16
        {
            get => _team2Player16;
            set
            {
                _team2Player16 = value;
                OnPropertyChanged(nameof(Team2Player16));
            }
        }

        public GameStatsPlayer Team2Player17
        {
            get => _team2Player17;
            set
            {
                _team2Player17 = value;
                OnPropertyChanged(nameof(Team2Player17));
            }
        }

        public GameStatsPlayer Team2Player18
        {
            get => _team2Player18;
            set
            {
                _team2Player18 = value;
                OnPropertyChanged(nameof(Team2Player18));
            }
        }

        public GameStatsPlayer Team2Player19
        {
            get => _team2Player19;
            set
            {
                _team2Player19 = value;
                OnPropertyChanged(nameof(Team2Player19));
            }
        }

        public GameStatsPlayer Team2Player20
        {
            get => _team2Player20;
            set
            {
                _team2Player20 = value;
                OnPropertyChanged(nameof(Team2Player20));
            }
        }

        public GameStatsPlayer HomeGoalie
        {
            get => _homeGoalie;
            set
            {
                _homeGoalie = value;
                OnPropertyChanged(nameof(HomeGoalie));
            }
        }

        public GameStatsPlayer AwayGoalie
        {
            get => _awayGoalie;
            set
            {
                _awayGoalie = value;
                OnPropertyChanged(nameof(AwayGoalie));
            }
        }



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
                OrganizePlayers();
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
            //HockeyGamesByDate = new ObservableCollection<Event>(games);
            GamesByDate = new ObservableCollection<Game>(games);
            IsLoading = false;
        }
        public async Task GetGameStats(string hometeam, string awayteam, DateTime gameDate)
        {
            var needToUpdate = false;
            //var firstGame = GamesByDate.OrderBy(g => g.Start).Select(s => s.Start).FirstOrDefault().ToString();
            //var firstGameDate = DateTime.ParseExact(firstGame, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            var firstGameDate = GamesByDate.OrderBy(g => g.Start).Select(s => s.Start).FirstOrDefault();
            var dateToCompare = new DateTime(
                firstGameDate.Year,
                firstGameDate.Month,
                firstGameDate.Day,
                13, // hour in 24-hour format (13 means 1 PM)
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
                    var gameStats = await _gamesService.GetGameStatsAsync(hometeam, awayteam, gameDate);
                    GameStats = gameStats;
                }
            }
        }

        private void OrganizePlayers()
        {
            if (_gameStats?.HomeTeamPlayers == null || _gameStats?.AwayTeamPlayers == null)
                return;
            if(_gameStats?.HomeTeamPlayers.Where(p => p.Line == 1 || p.Line == 2 || p.Line == 3 || p.Line == 4).ToList().Count == 0)
            {
                return;
            }
            if (_gameStats?.AwayTeamPlayers.Where(p => p.Line == 1 || p.Line == 2 || p.Line == 3 || p.Line == 4).ToList().Count == 0)
            {
                return;
            }

            var line1Players = _gameStats.HomeTeamPlayers
                .Where(p => p.Line == 1)
                .ToList();
            var line1AwayPlayers = _gameStats.AwayTeamPlayers
                .Where(p => p.Line == 1)
                .ToList();

            var line2Players = _gameStats.HomeTeamPlayers
                .Where(p => p.Line == 2)
                .ToList();
            var line2AwayPlayers = _gameStats.AwayTeamPlayers
                .Where (p => p.Line == 2)
                .ToList();

            var line3Players = _gameStats.HomeTeamPlayers
                .Where(p => p.Line == 3)
                .ToList();
            var line3AwayPlayers = _gameStats.AwayTeamPlayers
                .Where(p => p.Line == 3)
                .ToList();

            var line4Players = _gameStats.HomeTeamPlayers
                .Where(p => p.Line == 4)
                .ToList();
            var line4AwayPlayers = _gameStats.AwayTeamPlayers
                .Where(p => p.Line == 4)
                .ToList();

            var homeGoalie = _gameStats.HomeTeamPlayers
                .Where(p => p.Line == 1 && p.RoleCode == "MV")
                .FirstOrDefault();
            var awayGoalie = _gameStats.AwayTeamPlayers
                .Where(p => p.Line == 1 && p.RoleCode == "MV")
                .FirstOrDefault();

            Team1Player1 = line1Players.FirstOrDefault(p => p.RoleCode == "OL");
            Team1Player2 = line1Players.FirstOrDefault(p => p.RoleCode == "KH");
            Team1Player3 = line1Players.FirstOrDefault(p => p.RoleCode == "VL");
            Team1Player4 = line1Players.FirstOrDefault(p => p.RoleCode == "OP");
            Team1Player5 = line1Players.FirstOrDefault(p => p.RoleCode == "VP");
            Team1Player6 = line2Players.FirstOrDefault(p => p.RoleCode == "OL");
            Team1Player7 = line2Players.FirstOrDefault(p => p.RoleCode == "KH");
            Team1Player8 = line2Players.FirstOrDefault(p => p.RoleCode == "VL");
            Team1Player9 = line2Players.FirstOrDefault(p => p.RoleCode == "OP");
            Team1Player10 = line2Players.FirstOrDefault(p => p.RoleCode == "VP");
            Team1Player11 = line3Players.FirstOrDefault(p => p.RoleCode == "OL");
            Team1Player12 = line3Players.FirstOrDefault(p => p.RoleCode == "KH");
            Team1Player13 = line3Players.FirstOrDefault(p => p.RoleCode == "VL");
            Team1Player14 = line3Players.FirstOrDefault(p => p.RoleCode == "OP");
            Team1Player15 = line3Players.FirstOrDefault(p => p.RoleCode == "VP");
            Team1Player16 = line4Players.FirstOrDefault(p => p.RoleCode == "OL");
            Team1Player17 = line4Players.FirstOrDefault(p => p.RoleCode == "KH");
            Team1Player18 = line4Players.FirstOrDefault(p => p.RoleCode == "VL");
            Team1Player19 = line4Players.FirstOrDefault(p => p.RoleCode == "OP") ?? line4Players.Skip(1).FirstOrDefault(p => p != Team1Player16 && p != Team1Player17 && p != Team1Player18);
            Team1Player20 = line4Players.FirstOrDefault(p => p.RoleCode == "VP") ?? line4Players.Skip(1).FirstOrDefault(p => p != Team1Player16 && p != Team1Player17 && p != Team1Player18
                                                                                    && p != Team1Player19);

            Team2Player1 = line1AwayPlayers.FirstOrDefault(p => p.RoleCode == "OL");
            Team2Player2 = line1AwayPlayers.FirstOrDefault(p => p.RoleCode == "KH");
            Team2Player3 = line1AwayPlayers.FirstOrDefault(p => p.RoleCode == "VL");
            Team2Player4 = line1AwayPlayers.FirstOrDefault(p => p.RoleCode == "OP");
            Team2Player5 = line1AwayPlayers.FirstOrDefault(p => p.RoleCode == "VP");
            Team2Player6 = line2AwayPlayers.FirstOrDefault(p => p.RoleCode == "OL");
            Team2Player7 = line2AwayPlayers.FirstOrDefault(p => p.RoleCode == "KH");
            Team2Player8 = line2AwayPlayers.FirstOrDefault(p => p.RoleCode == "VL");
            Team2Player9 = line2AwayPlayers.FirstOrDefault(p => p.RoleCode == "OP");
            Team2Player10 = line2AwayPlayers.FirstOrDefault(p => p.RoleCode == "VP");
            Team2Player11 = line3AwayPlayers.FirstOrDefault(p => p.RoleCode == "OL");
            Team2Player12 = line3AwayPlayers.FirstOrDefault(p => p.RoleCode == "KH");
            Team2Player13 = line3AwayPlayers.FirstOrDefault(p => p.RoleCode == "VL");
            Team2Player14 = line3AwayPlayers.FirstOrDefault(p => p.RoleCode == "OP");
            Team2Player15 = line3AwayPlayers.FirstOrDefault(p => p.RoleCode == "VP");
            Team2Player16 = line4AwayPlayers.FirstOrDefault(p => p.RoleCode == "OL");
            Team2Player17 = line4AwayPlayers.FirstOrDefault(p => p.RoleCode == "KH");
            Team2Player18 = line4AwayPlayers.FirstOrDefault(p => p.RoleCode == "VL");
            Team2Player19 = line4AwayPlayers.FirstOrDefault(p => p.RoleCode == "OP") ?? line4AwayPlayers.Skip(1).FirstOrDefault(p => p != Team2Player16 && p != Team2Player17 && p != Team2Player18);
            Team2Player20 = line4AwayPlayers.FirstOrDefault(p => p.RoleCode == "VP") ?? line4AwayPlayers.Skip(1).FirstOrDefault(p => p != Team2Player16 && p != Team2Player17 && p != Team2Player18
                                                                                     && p != Team2Player19);

            HomeGoalie = homeGoalie;
            AwayGoalie = awayGoalie;
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

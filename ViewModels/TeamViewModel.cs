using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Sporttiporssi.Services;
using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Diagnostics;
using CommunityToolkit.Maui.Core.Views;

namespace Sporttiporssi.ViewModels
{
    public class TeamViewModel : INotifyPropertyChanged
    {
        private readonly TeamService _teamService;
        private readonly LeaguePlayersService _leaguePlayersService;
        private readonly GroupService _groupService;
        private ObservableCollection<FantasyTeam> _fantasyTeams {  get; set; }
        private FantasyTeam _currentTeam { get; set; }
        private ObservableCollection<Player> _currentTeamPlayers { get; set; }
        private ObservableCollection<Player> _currentSoldPlayers { get; set; }
        private bool _userHasTeams { get; set; }
        private int _fundsLeft { get; set; }
        private string _fundsLeftString { get; set; }
        private int _maxTrades { get; set; }
        private string _tradesLeftString { get; set; }
        private bool _canTrade { get; set; }
        private string _groupName { get; set; }
        private string _groupStanding {  get; set; }
        private int _totalFTP {  get; set; }
        private HockeyDefaultFTP _hockeyDefaults {  get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NotifyHasSoldPlayersChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(HasSoldPlayers)));
        }

        public ObservableCollection<FantasyTeam> FantasyTeams
        {
            get => _fantasyTeams;
            set
            {
                _fantasyTeams = value;
                OnPropertyChanged(nameof(FantasyTeams));
            }
        }

        public FantasyTeam CurrentTeam
        {
            get => _currentTeam;
            set
            {
                _currentTeam = value;
                OnPropertyChanged(nameof(CurrentTeam));
            }
        }

        public bool HasSoldPlayers => CurrentTeamPlayers?.Any(player => player.IsSold) ?? false;

        public ObservableCollection<Player> CurrentTeamPlayers
        {
            get => _currentTeamPlayers;
            set
            {
                _currentTeamPlayers = value;
                OnPropertyChanged(nameof(CurrentTeamPlayers));
                OnPropertyChanged(nameof(HasSoldPlayers));
            }
        }

        public ObservableCollection<Player> CurrentSoldPlayers
        {
            get => _currentSoldPlayers;
            set
            {
                _currentSoldPlayers = value;
                OnPropertyChanged(nameof(CurrentSoldPlayers));
            }
        }

        public int MaxTrades
        {
            get => _maxTrades;
            set
            {
                _maxTrades = value;
                OnPropertyChanged(nameof(MaxTrades));
            }
        }

        public string TradesLeftString
        {
            get => _tradesLeftString;
            set
            {
                _tradesLeftString = value;
                OnPropertyChanged(nameof(TradesLeftString));
            }
        }

        public bool CanTrade
        {
            get => _canTrade;
            set
            {
                _canTrade = value;
                OnPropertyChanged(nameof(CanTrade));
            }
        }

        public int FundsLeft
        {
            get => _fundsLeft;
            set
            {
                _fundsLeft = value;
                OnPropertyChanged(nameof(FundsLeft));
            }
        }

        public string FundsLeftString
        {
            get => _fundsLeftString;
            set
            {
                _fundsLeftString = value;
                OnPropertyChanged(nameof(FundsLeftString));
            }
        }

        public bool UserHasTeams
        {
            get => _userHasTeams;
            set
            {
                _userHasTeams = value;
                OnPropertyChanged(nameof(UserHasTeams));
            }
        }

        public string GroupName
        {
            get => _groupName;
            set
            {
                _groupName = value;
                OnPropertyChanged(nameof(GroupName));
            }
        }

        public string GroupStanding
        {
            get => _groupStanding;
            set
            {
                _groupStanding = value;
                OnPropertyChanged(nameof(GroupStanding));
            }
        }

        public int TotalFTP
        {
            get => _totalFTP;
            set
            {
                _totalFTP = value;
                OnPropertyChanged(nameof(TotalFTP));
            }
        }

        public HockeyDefaultFTP HockeyDefaults
        {
            get => _hockeyDefaults;
            set
            {
                _hockeyDefaults = value;
                OnPropertyChanged(nameof(HockeyDefaults));
            }
        }

        public TeamViewModel(TeamService teamService, LeaguePlayersService leaguePlayersService, GroupService groupService)
        {
            _teamService = teamService;
            _leaguePlayersService = leaguePlayersService;
            _groupService = groupService;           
        }

        private async Task GetHockeyDefaults()
        {
            var defaults = await _teamService.GetHockeyDefaultFTPBySerie();
            HockeyDefaults = defaults;
        }

        private async Task<bool> CanWeTrade()
        {
            var currentSerie = Preferences.Get("currentserie", string.Empty);
            bool canTrade = false;
            if (string.IsNullOrEmpty(currentSerie))
            {
                canTrade = false;
            }
            else
            {
                canTrade = await _teamService.CanTrade(currentSerie);
            }           
            return canTrade;
        }

        public async Task<bool> MarkAsSold(Player player)
        {
            bool canTrade = await CanWeTrade();
            if(!canTrade)
            {
                return false;
            }             
            if (player != null && CurrentTeamPlayers.Contains(player) && canTrade)
            {
                // Set isSold based on its status
                //CurrentTeamPlayers.Remove(player);
                Debug.WriteLine("SOLD PLAYER: " + player.Name);
                player.IsSold = !player.IsSold;
                OnPropertyChanged(nameof(HasSoldPlayers));
                if(player.IsSold)
                {
                    CurrentSoldPlayers.Add(player);
                    FundsLeft += player.Price;
                    FundsLeftString = $"{FundsLeft} / 2000000";
                }
                else
                {
                    CurrentSoldPlayers.Remove(player);
                    FundsLeft -= player.Price;
                    FundsLeftString = $"{FundsLeft} / 2000000";
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<string> Roles = new List<string> { "VL", "KH", "OL", "VP", "MV", "OP" };

     
        public async Task LoadCurrentUserTeam()
        { 
            var chosenTeam = Preferences.Get("chosen_team", string.Empty);
            bool updatePlayers = false;
            if(string.IsNullOrEmpty(chosenTeam) || CurrentTeamPlayers == null)
            {
                CurrentTeamPlayers = new ObservableCollection<Player>();
            }           
            if(CurrentSoldPlayers == null)
            {
                CurrentSoldPlayers = new ObservableCollection<Player>();
            }
            FantasyTeams = await _teamService.AllFantasyTeamsByUserAndSerie();
            UserHasTeams = FantasyTeams != null && FantasyTeams.Any();
            bool chosenTeamExists = FantasyTeams.Where(t => t.FantasyTeamId.ToString() == chosenTeam).Any();
            if(UserHasTeams)
            { 
                if(string.IsNullOrEmpty(chosenTeam) || !chosenTeamExists)
                {
                    CurrentTeam = FantasyTeams.FirstOrDefault();
                    Preferences.Set("chosen_team", CurrentTeam.FantasyTeamId.ToString());
                    CurrentTeamPlayers = await _leaguePlayersService.GetPlayersByFantasyTeamName(CurrentTeam.TeamName);
                }
                else
                {
                    if(CurrentTeam != null && CurrentTeam.FantasyTeamId.ToString() != chosenTeam)
                    {
                        Debug.WriteLine("Team has changed!");
                        CurrentTeam = FantasyTeams.Where(t => t.FantasyTeamId.ToString() == chosenTeam).FirstOrDefault();
                        CurrentTeamPlayers = await _leaguePlayersService.GetPlayersByFantasyTeamName(CurrentTeam.TeamName);
                        updatePlayers = true;
                    }
                    else
                    {
                        CurrentTeam = FantasyTeams.Where(t => t.FantasyTeamId.ToString() == chosenTeam).FirstOrDefault();
                    }
                }
                if (CurrentTeamPlayers != null && CurrentTeamPlayers.Count == 0)
                {
                    CurrentTeamPlayers = await _leaguePlayersService.GetPlayersByFantasyTeamName(CurrentTeam.TeamName);
                    updatePlayers = true;
                }
                //CurrentTeamPlayers = await _leaguePlayersService.GetPlayersByFantasyTeamName(CurrentTeam.TeamName);
                //updatePlayers = true;
                var orderedPlayers = new ObservableCollection<Player>();
                if (HockeyDefaults == null)
                {
                    await GetHockeyDefaults();
                }
                FundsLeft = HockeyDefaults.StartMoney;                
                foreach (var role in Roles)
                {
                    var player = CurrentTeamPlayers.FirstOrDefault(p => p.Role == role);
                    if (player != null)
                    {
                        orderedPlayers.Add(player);
                        FundsLeft -= player.Price;
                    }
                    else
                    {
                        orderedPlayers.Add(new Player
                        {
                            Role = role,
                            FirstName = "Dummy",
                            LastName = "Player",
                            PictureUrl = "ritchie.jpg",
                            Price = 0,
                            FTP = 0
                        });
                    }
                }
                var groupDataResults = await _groupService.GetGroupDataAndStandingByTeamId(CurrentTeam.FantasyTeamId);
                if(groupDataResults != null && groupDataResults.Any())
                {
                    var groupData = groupDataResults.First();                  
                    GroupName = groupData.GroupName;
                    GroupStanding = $"{groupData.Standing} / {groupData.TeamsInGroup}";
                    MaxTrades = groupData.TradesPerPhase;
                }
                else // Group not found, so resetting groupvalues
                {
                    GroupName = "";
                    GroupStanding = "";
                    MaxTrades = HockeyDefaults.TradesPerPhase;
                }
                                
                FundsLeftString = $"{FundsLeft} / {HockeyDefaults.StartMoney}";
                TradesLeftString = $"{(MaxTrades - CurrentTeam.TradesThisPhase)} / {MaxTrades}";
                if(!CurrentTeamPlayers.SequenceEqual(orderedPlayers))
                {
                    CurrentTeamPlayers = orderedPlayers;
                }               
            }   
            else
            {
                FundsLeftString = "";
                TotalFTP = 0;
                TradesLeftString = "";
                GroupName = "";
                GroupStanding = "";   
                if(CurrentTeam != null)
                {
                    CurrentTeam = null;
                }
            }
        }

        public async Task<FantasyTeam> LoadAllUserTeams()
        {
            var chosenTeam = Preferences.Get("chosen_team", string.Empty);
            FantasyTeams = await _teamService.AllFantasyTeamsByUserAndSerie();           
            UserHasTeams = FantasyTeams != null && FantasyTeams.Any();
            if (HockeyDefaults == null)
            {
                await GetHockeyDefaults();
            }
            if (UserHasTeams)
            {
                if (chosenTeam != null || !string.IsNullOrEmpty(chosenTeam))
                {
                    CurrentTeam = FantasyTeams.Where(t => t.FantasyTeamId.ToString() == chosenTeam).FirstOrDefault();                    
                }
                foreach(var team in FantasyTeams)
                {
                    team.TeamValue = HockeyDefaults.StartMoney - team.FundsLeft;
                }
                return CurrentTeam;
            }
            return null;
        }

        public async Task<bool> LeaveGroup()
        {
            if(CurrentTeam.FantasyTeamId != null && !string.IsNullOrEmpty(GroupName))
            {
                bool success = await _groupService.LeaveGroup(GroupName, CurrentTeam.FantasyTeamId);
                if(success)
                {
                    GroupName = string.Empty;
                    GroupStanding = string.Empty;
                    MaxTrades = HockeyDefaults.TradesPerPhase;
                    TradesLeftString = $"{(MaxTrades - CurrentTeam.TradesThisPhase)} / {MaxTrades}";
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> SaveNewTeam(TeamRegisterDto dto)
        {
            if (HockeyDefaults == null)
            {
                HockeyDefaults = await _teamService.GetHockeyDefaultFTPBySerie();
            }
            dto.FundsLeft = HockeyDefaults.StartMoney;
            dto.TradesPerPhase = HockeyDefaults.TradesPerPhase;
            bool success = await _teamService.SaveNewTeam(dto);
            return success;
        }

    }
}

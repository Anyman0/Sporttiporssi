using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sporttiporssi.Services;
using Sporttiporssi.Models;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Sporttiporssi.ViewModels
{
    public class PlayerViewModel : INotifyPropertyChanged
    {
        private readonly LeaguePlayersService _playersService;       
        private ObservableCollection<Player> _players { get; set; }
        private ObservableCollection<Player> _selectedPlayers { get; set; }
        private ObservableCollection<Player> _teamPlayers { get; set; }   
        public ICommand SortCommand { get; private set; }
        public List<string> AvailableFields { get; set; } = new List<string>
        {
            "Name", "Role", "Goals", "Assists", "Points", "Shots", "FormattedPrice", "TeamName"
        };

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _fundsLeft;
        public int FundsLeft
        {
            get => _fundsLeft;
            set
            {
                _fundsLeft = value;
                OnPropertyChanged(nameof(FundsLeft));
            }
        }

        private List<string> _availablePlayerRoles;
        public List<string> AvailablePlayerRoles
        {
            get => _availablePlayerRoles;
            set
            {
                _availablePlayerRoles = value;
                foreach (var role in _availablePlayerRoles)
                {
                    RoleString += role + " / ";
                }
                OnPropertyChanged(nameof(AvailablePlayerRoles));              
                FilterAvailablePlayers();
            }
        }

        private bool _hasChosenAllAvailable;
        public bool HasChosenAllAvailable
        {
            get => _hasChosenAllAvailable;
            set
            {
                _hasChosenAllAvailable = value;
                OnPropertyChanged(nameof(HasChosenAllAvailable));
            }
        }

        private ObservableCollection<Player> _availablePlayers;
        public ObservableCollection<Player> AvailablePlayers
        {
            get => _availablePlayers;
            set
            {
                _availablePlayers = value;
                OnPropertyChanged(nameof(AvailablePlayers));
            }
        }

        private string _rolesString { get; set; }
        public string RoleString
        {
            get => _rolesString;
            set
            {
                _rolesString = value;
                OnPropertyChanged(nameof(RoleString));
            }
        }

        public ObservableCollection<Player> Players
        {
            get => _players;
            set
            {
                _players = value;
                OnPropertyChanged(nameof(Players));
            }
        }

        public ObservableCollection<Player> SelectedPlayers
        {
            get => _selectedPlayers;
            set
            {
                _selectedPlayers = value;
                OnPropertyChanged(nameof(SelectedPlayers));
            }
        }

        public ObservableCollection<Player> TeamPlayers
        {
            get => _teamPlayers;
            set
            {
                _teamPlayers = value;
                OnPropertyChanged(nameof(TeamPlayers));
            }
        }
  
        public PlayerViewModel(LeaguePlayersService playersService)
        {
            _playersService = playersService;
            SortCommand = new Command<string>(SortPlayers);
        }
            
        public async Task LoadAllPlayersAsync()
        {
            SelectedPlayers = new ObservableCollection<Player>();
            //Players = await _playersService.GetPlayersAsync();
            Players = await _playersService.GetPlayersByRole(AvailablePlayerRoles.ToArray());
            FilterAvailablePlayers();
        }  

        public async Task<bool> AddPlayersToTeam()
        {
            bool allSuccess = true;

            foreach (var player in SelectedPlayers)
            {
                bool success = await _playersService.AddPlayersToTeam(player.PlayerId);
                if (!success)
                {
                    allSuccess = false;
                    // log failure?
                }
            }

            return allSuccess;
        }
              
        public string AddPlayerToSelected(Player player)
        {
            if(SelectedPlayers.Contains(player))
            {
                return "Exists";
            }
            else
            {
                if(player.Price > FundsLeft)
                {
                    return "NoFunds";
                }
                SelectedPlayers.Add(player);
                AvailablePlayerRoles.Remove(player.Role);
                FilterAvailablePlayers();
                FundsLeft -= player.Price;
                return "Success";
            }            
        }       
        public bool RemovePlayerFromSelected(Player player)
        {
            if(SelectedPlayers.Contains(player))
            {
                SelectedPlayers.Remove(player);
                AvailablePlayerRoles.Add(player.Role);
                FilterAvailablePlayers();
                FundsLeft += player.Price;
                return true;
            }
            else
            {
                return false;
            }
        }

        private async void FilterAvailablePlayers()
        {          
            if (_players == null || _availablePlayerRoles == null)
            {
                AvailablePlayers = new ObservableCollection<Player>();               
                return;
            }
            if(_availablePlayerRoles != null)
            {
                RoleString = "";
                foreach (var role in _availablePlayerRoles)
                {
                    RoleString += role + " / ";
                }
                if(_availablePlayerRoles.Count > 0)
                {
                    HasChosenAllAvailable = false;
                }
                else
                {
                    HasChosenAllAvailable = true;
                }
            }            
            var filteredPlayers = _players.Where(p => _availablePlayerRoles.Contains(p.Role)).ToList();
            AvailablePlayers = new ObservableCollection<Player>(filteredPlayers);
        }

        private bool _isAscending = true;

        public void SortPlayers(string sortBy)
        {
            switch (sortBy)
            {
                case "Name":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Name) :
                        AvailablePlayers.OrderByDescending(p => p.Name));
                    break;
                case "Role":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Role) :
                        AvailablePlayers.OrderByDescending(p => p.Role));
                    break;
                case "Goals":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Goals) :
                        AvailablePlayers.OrderByDescending(p => p.Goals));
                    break;
                case "Assists":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Assists) :
                        AvailablePlayers.OrderByDescending(p => p.Assists));
                    break;
                case "Points":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Points) :
                        AvailablePlayers.OrderByDescending(p => p.Points));
                    break;
                case "Price":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.Price) :
                        AvailablePlayers.OrderByDescending(p => p.Price));
                    break;
                case "TeamShortName":
                    AvailablePlayers = new ObservableCollection<Player>(_isAscending ?
                        AvailablePlayers.OrderBy(p => p.TeamShortName) :
                        AvailablePlayers.OrderByDescending(p => p.TeamShortName));
                    break;
            }

            _isAscending = !_isAscending; // Toggle sorting direction
        }
    }
}

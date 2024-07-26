using CommunityToolkit.Maui.Views;
using Sporttiporssi.Views.Popups;
using Sporttiporssi.ViewModels;
using System.Diagnostics;
using Sporttiporssi.Models;
using Sporttiporssi.Models.DTOs;

namespace Sporttiporssi.Views
{
    public partial class MainPage : ContentPage
    {
        private GamesViewModel _gamesViewModel;
        private readonly IServiceProvider _serviceProvider;
        private DateTime gameDate;
        public MainPage(GamesViewModel gamesViewModel, IServiceProvider serviceProvider)
        {
            //Shell.SetNavBarIsVisible(this, false);
            InitializeComponent();

            _gamesViewModel = gamesViewModel;
            BindingContext = _gamesViewModel;
            _serviceProvider = serviceProvider;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            gameDate = DateTime.Now.Date;
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
            await  _gamesViewModel.LoadGamesAsync(gameDate);          
        }

        private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            var popup = new SportPopup();
            this.ShowPopup(popup);     
        }

        private void Stats_Tapped(object sender, TappedEventArgs e)
        {
            Debug.WriteLine("Stats tapped!");
            var image = sender as Image;
            var game = image.BindingContext as LiigaGameDto;
            if (game != null)
            {
                if(game.IsRosterExpanded)
                {
                    _gamesViewModel.ToggleRosterExpansion(game);
                }
                _gamesViewModel.ToggleStatsExpansion(game);
            }
        }

        private async void Roster_Tapped(object sender, TappedEventArgs e)
        {
            Debug.WriteLine("Roster tapped!");
            var image = sender as Image;
            var game = image.BindingContext as LiigaGameDto;
            if (game != null) 
            { 
                if(game.IsStatsExpanded)
                {
                    _gamesViewModel.ToggleStatsExpansion(game);
                }
                else
                {
                    _gamesViewModel.ToggleRosterExpansion(game);
                }
                if(game.IsRosterExpanded)
                {
                    // Load labels with rosterdata
                    await _gamesViewModel.GetGameStats(game.Id, game.Season);                    
                }
            }
        }

        private async void RightArrow_Tapped(object sender, TappedEventArgs e)
        {
            gameDate = gameDate.AddDays(1);
            await _gamesViewModel.LoadGamesAsync(gameDate);
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
        }

        private async void LeftArrow_Tapped(object sender, TappedEventArgs e)
        {
            gameDate = gameDate.AddDays(-1);
            await _gamesViewModel.LoadGamesAsync(gameDate);
            GamesLabel.Text = $"Ottelut {gameDate.ToString("dd.M.yyyy")}";
        }
       
    }

}

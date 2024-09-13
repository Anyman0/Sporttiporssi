using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;

namespace Sporttiporssi.Views;

public partial class MyTeamPage : ContentPage
{
    private readonly IServiceProvider _serviceProvider;
    private readonly TeamViewModel _teamViewModel;
	public MyTeamPage(IServiceProvider serviceProvider, TeamViewModel teamViewModel)
	{
        _serviceProvider = serviceProvider;
        Shell.SetNavBarIsVisible(this, false);
        InitializeComponent();
        _teamViewModel = teamViewModel;
        BindingContext = _teamViewModel;
    }

    private async void TradeButton_Clicked(object sender, EventArgs e)
    {        
        var playerViewModel = _serviceProvider.GetRequiredService<PlayerViewModel>();        
        playerViewModel.FundsLeft = _teamViewModel.FundsLeft;
        playerViewModel.AvailablePlayerRoles = _teamViewModel.CurrentSoldPlayers.Select(p => p.Role).ToList();
        var tradePlayerPage = new TradePlayerPage(playerViewModel, _serviceProvider);
        await Shell.Current.Navigation.PushAsync(tradePlayerPage);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        _teamViewModel.CurrentSoldPlayers = new ObservableCollection<Player>();
        LoadUserTeams();
        if (_teamViewModel.CurrentTeamPlayers.Count > 0)
        {
            foreach(var player in _teamViewModel.CurrentTeamPlayers)
            {
                player.IsSold = false;
            }
            _teamViewModel.NotifyHasSoldPlayersChanged();
        }       
    }

    private async void JoinGroup_Tapped(object sender, TappedEventArgs e)
    {
        if (sender is Label label)
        {
            if (label.Text == "Leave Group")
            {
                bool result = await DisplayAlert("Leave Group", "Are you sure you want to leave your current group?", "YES", "NO");
                if (result)
                {
                    // Handle the leave group logic here                   
                    bool success = await _teamViewModel.LeaveGroup();
                    if (!success)
                    {
                        await DisplayAlert("Failed", "Failed to leave group", "OK");
                    }
                }
                else
                {
                    // User chose not to leave the group, no further action needed
                    return;
                }
            }
            else
            {
                var groupPage = _serviceProvider.GetRequiredService<GroupPage>();
                await Shell.Current.Navigation.PushAsync(groupPage);
            }
        }
    }

    private async void ChangeTeam_Tapped(object sender, TappedEventArgs e)
    {
        var changeTeamPage = _serviceProvider.GetRequiredService<ChangeTeamPage>();
        await Shell.Current.Navigation.PushAsync(changeTeamPage);
    }

    private async void LoadUserTeams()
    {
        await _teamViewModel.LoadCurrentUserTeam();
    }

    private void LoadGroupTeams()
    {
        var serie = Preferences.Get("currentserie", string.Empty);

    }

    private async void CreateTeam_Clicked(object sender, EventArgs e)
    {
        var createTeamPage = _serviceProvider.GetRequiredService<CreateTeamPage>();
        await Shell.Current.Navigation.PushAsync(createTeamPage);
    }

    private async void SellPlayer_Tapped(object sender, TappedEventArgs e)
    {
        if(sender is Image image && image.BindingContext is Player player)
        {           
            bool success = await _teamViewModel.MarkAsSold(player);
            if(!success)
            {
                await DisplayAlert("Failed", "Trades closed. Games have started.", "OK");
            }
        }
    }

    private void Group_Tapped(object sender, TappedEventArgs e)
    {
        // Navigate to mygroup-page. (Here we have listed all teams in the group + their data and standings)
        Debug.WriteLine("To mygroup-page");
    }
}
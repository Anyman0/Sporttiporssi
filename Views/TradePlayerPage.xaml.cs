 using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;
using System.Collections.ObjectModel;
using System.Diagnostics;
namespace Sporttiporssi.Views;

public partial class TradePlayerPage : ContentPage
{
    private PlayerViewModel _playerViewModel;
    private IServiceProvider _serviceProvider;
     
    public TradePlayerPage(PlayerViewModel playerViewModel, IServiceProvider serviceProvider)
	{
		InitializeComponent();
		_playerViewModel = playerViewModel;      
        _serviceProvider = serviceProvider;
        BindingContext = _playerViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        await _playerViewModel.LoadAllPlayersAsync();
    }

    private async void PlayerList_Tapped(object sender, ItemTappedEventArgs e)
    {
        Debug.WriteLine("..");
        if(e.Item != null)
        {
            var player = e.Item as Player;
            Debug.WriteLine("Tapped player is: " + player.LastName);
            var success = _playerViewModel.AddPlayerToSelected(player);
            if(success == "Exists")
            {
                await DisplayAlert("Failed.", "Player is already added.", "OK");
            }
            else if(success == "NoFunds")
            {
                await DisplayAlert("Failed.", "Insufficient funds..", "OK");
            }           
        }
    }

    private async void SelectedPlayerList_Tapped(object sender, ItemTappedEventArgs e)
    {
        Debug.WriteLine("..");     
        if(e.Item != null)
        {
            var player = e.Item as Player;
            var success = _playerViewModel.RemovePlayerFromSelected(player);
            if(!success)
            {
                await DisplayAlert("Error!", "Error occured when deleting player from selected..", "OK");
            }
        }
    }

    private async void FinishButton_Clicked(object sender, EventArgs e)
    {
        bool success = await _playerViewModel.AddPlayersToTeam();
        if(success)
        {
            var myTeamPage = _serviceProvider.GetRequiredService<MyTeamPage>();
            await Shell.Current.Navigation.PushAsync(myTeamPage);
        }   
        else
        {
            await DisplayAlert("Failed.", "Failed to add selected players to team.", "OK");
        }
    }
}
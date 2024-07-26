using Sporttiporssi.Models;
using Sporttiporssi.ViewModels;

namespace Sporttiporssi.Views;

public partial class TradesPage : ContentPage
{
    private PlayerViewModel _playerViewModel;
	public TradesPage(PlayerViewModel playerViewModel)
	{
		Shell.SetNavBarIsVisible(this, false);
		InitializeComponent();
        _playerViewModel = playerViewModel;
        BindingContext = _playerViewModel;
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
    }
	protected override async void OnDisappearing() 
	{ 
		base.OnDisappearing();
    }

    private async void CompleteButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(MyTeamPage));
    }
}